using Core.Logger.Enum;
using Core.Logger;
using System.Net.Sockets;
using Core.Logger.Interface;
using Core.TCP.Interface;
using Core.TCP.Model;
using System.Diagnostics;

namespace Core.TCP
{
    public class TcpConnection : ITcpConnection
    {
        private const int HEADER_BYTES = 4;

        public Socket socket {get; set;}
        protected ILogger _logger;

        public string ipClient { get; set; }
        public string ipServer { get; set; }

        private SocketAsyncEventArgs socketEvent;

        private byte[] headerBuffer = new byte[HEADER_BYTES];

        private List<byte> receiveBuffer = new List<byte>();
        private int receivedDataSize = 0;

        private int expectedDataSize;

        private TcpReceiveState receiveState;

        public delegate void ReceiveDataHandler(byte[] response);
        public event ReceiveDataHandler ReceiveDataCompletedHandler;

        public Guid InstanceId { get; } = Guid.NewGuid();


        public TcpConnection(ILogger logger)
        {
            _logger = logger;
            _logger.WriteToLogFile($"TcpConnection instance created with ID: {InstanceId}");
        }

        public void InitializeSocketEvent()
        {
            try
            {
                socketEvent = new SocketAsyncEventArgs();
                socketEvent.Completed += ReceiveEventArgs_Completed;
                socketEvent.SetBuffer(headerBuffer, 0, HEADER_BYTES);
            }
            catch (Exception ex)
            {
                _logger.WriteToLogFile($"Erro ao inicializar o evento do socket: {ex.Message}");
            }
        }

        #region SendData
        public async Task<BaseResponse> SendAsync(byte[] data)
        {
            try
            {
                var dataWithHeader = CombineHeaderAndData(data);
                return await SendData(dataWithHeader);
            }
            catch (Exception ex)
            {
                return (BaseResponse)HandleException(ex);
            }
        }

        private byte[] CombineHeaderAndData(byte[] data)
        {
            var header = BitConverter.GetBytes(data.Length);
            var dataWithHeader = new byte[header.Length + data.Length];

            Buffer.BlockCopy(header, 0, dataWithHeader, 0, header.Length);
            Buffer.BlockCopy(data, 0, dataWithHeader, header.Length, data.Length);

            return dataWithHeader;
        }

        private async Task<BaseResponse> SendData(byte[] data)
        {
            try
            {
                var bytesSent = await socket.SendAsync(data, SocketFlags.None);

                _logger.WriteToLogFile($"Sending data: {bytesSent}");
                
                return bytesSent > 0
                    ? new BaseResponse { Error = ErrorEnum.None, IsSuccess = true }
                    : new BaseResponse { Error = ErrorEnum.SocketException, IsSuccess = false };
            }
            catch (Exception ex)
            {
                return (BaseResponse)HandleException(ex);
            }
        }
        #endregion


        #region ReceiveData
        public void StartReceive()
        {
            try
            {
                receiveState = TcpReceiveState.Header;
                socket.ReceiveAsync(socketEvent);
            }
            catch (Exception ex)
            {
                _logger.WriteToLogFile($"Erro ao iniciar a recepção de dados: {ex.Message}");
            }
        }

        private void ReceiveEventArgs_Completed(object? sender, SocketAsyncEventArgs e)
        {
            _logger.WriteToLogFile("Dados recebidos");
            if (e.SocketError != SocketError.Success && e.LastOperation == SocketAsyncOperation.Receive)
            { 
                // Handle error
            }
            else
            {
                try
                {
                    switch (receiveState)
                    {
                        case TcpReceiveState.Header:
                            HandleHeaderReceived(e);
                            break;

                        case TcpReceiveState.Data:
                            HandleDataReceived(e);
                            break;

                        case TcpReceiveState.Stop:
                            HandleStopState(e);
                            break;
                    }

                    var willRaiseEvent = socket.ReceiveAsync(e);
                    while (!willRaiseEvent)
                    {
                        ProcessReceivedData(e);
                        willRaiseEvent = socket.ReceiveAsync(e);
                    }
                }
                catch (Exception ex)
                {
                    _logger.WriteToLogFile($"Erro ao processar dados recebidos: {ex.Message}");
                }
            }
        }

        private void ProcessReceivedData(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                // Handle error
                _logger.WriteToLogFile($"Erro ao processar dados recebidos: {e.SocketError}");
            }
            else
            {
                switch (receiveState)
                {
                    case TcpReceiveState.Header:
                        HandleHeaderReceived(e);
                        break;

                    case TcpReceiveState.Data:
                        HandleDataReceived(e);
                        break;

                    case TcpReceiveState.Stop:
                        HandleStopState(e);
                        break;
                }
            }
        }

        private void HandleHeaderReceived(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == HEADER_BYTES)
            {
                _logger.WriteToLogFile($"Cabeçalho - {e.BytesTransferred}");
                Array.Copy(e.Buffer, headerBuffer, HEADER_BYTES);

                expectedDataSize = BitConverter.ToInt32(headerBuffer);
                receiveBuffer.Clear();
                socketEvent.SetBuffer(new byte[expectedDataSize], 0, expectedDataSize);
                receiveState = TcpReceiveState.Data;
            }
        }

        private void HandleDataReceived(SocketAsyncEventArgs e)
        {
            receiveBuffer.AddRange(e.Buffer.Take(e.BytesTransferred));
            receivedDataSize += e.BytesTransferred;

            if (receivedDataSize == expectedDataSize)
            {
                _logger.WriteToLogFile($"{receivedDataSize} - {expectedDataSize}");

                receivedDataSize = 0;
                e.SetBuffer(headerBuffer, 0, HEADER_BYTES);
                receiveState = TcpReceiveState.Header;
                ReceiveDataCompletedHandler?.Invoke(receiveBuffer.ToArray());
            }
            else
            {
                e.SetBuffer(new byte[expectedDataSize - receivedDataSize], 0, expectedDataSize - receivedDataSize);
            }

        }

        private void HandleStopState(SocketAsyncEventArgs e)
        {
            e.SetBuffer(new byte[250000], 0, 250000);
        }
        #endregion

        private BaseResponse<byte[]> HandleException(Exception ex)
        {
            switch (ex)
            {
                case SocketException socketException:
                    _logger.WriteToLogFile($"Erro de socket: {socketException.SocketErrorCode}. Mensagem: {socketException.Message}");
                    return new BaseResponse<byte[]> { Error = ErrorEnum.SocketException, IsSuccess = false };

                case ObjectDisposedException disposedException:
                    _logger.WriteToLogFile($"Objeto Socket descartado. Mensagem: {disposedException.Message}");
                    return new BaseResponse<byte[]> { Error = ErrorEnum.ObjectDisposedException, IsSuccess = false };

                default:
                    _logger.WriteToLogFile($"Ocorreu uma exceção não esperada: {ex.Message}");
                    return new BaseResponse<byte[]> { Error = ErrorEnum.UnknownError, IsSuccess = false };
            }
        }
    }
}
