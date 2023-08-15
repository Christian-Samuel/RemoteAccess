using System.Net.Sockets;
using System.Net;
using Core.Logger.Interface;
using Core.Logger;
using Core.Logger.Enum;
using Core.TCP;
using Client.TCP.Interface;
using Core.TCP.Interface;

namespace Client.TCP
{
    public class TcpClientConnection : ITcpClientConnection
    {
        public delegate void ConnectionSuccessHandler();
        public event ConnectionSuccessHandler ConnectionSuccess;

        public delegate void ConnectionErrorHandler(BaseResponse errorMessage);
        public event ConnectionErrorHandler ConnectionError;

        private readonly ITcpConnection _connection;
        private readonly ILogger _logger;

        public TcpClientConnection(ILogger logger, ITcpConnection connection)
        {
            _connection = connection;
            _logger = logger;
            _connection.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task ConnectAsync(string ipServer, int port)
        {
            try
            {
                await _connection.socket.ConnectAsync(new IPEndPoint(IPAddress.Parse(ipServer), port));

                ConnectionSuccess?.Invoke();
            }
            catch (SocketException ex)
            {
                _logger.WriteToLogFile("SocketException: " + ex.Message);

                ConnectionError?.Invoke(new BaseResponse()
                {
                    Error = ErrorEnum.SocketException,
                    IsSuccess = false
                });
            }
            catch (ArgumentException ex)
            {
                _logger.WriteToLogFile("ArgumentException: " + ex.Message);

                ConnectionError?.Invoke(new BaseResponse()
                {
                    Error = ErrorEnum.ArgumentException,
                    IsSuccess = false
                });
            }
            catch (ObjectDisposedException ex)
            {
                _logger.WriteToLogFile("ObjectDisposedException: " + ex.Message);

                ConnectionError?.Invoke(new BaseResponse() { Error = ErrorEnum.ObjectDisposedException, IsSuccess = false });
            }
            catch (NotSupportedException ex)
            {
                _logger.WriteToLogFile("NotSupportedException: " + ex.Message);

                ConnectionError?.Invoke(new BaseResponse() { Error = ErrorEnum.NotSupportedException, IsSuccess = false });
            }
            catch (InvalidOperationException ex)
            {
                _logger.WriteToLogFile("InvalidOperationException: " + ex.Message);

                ConnectionError?.Invoke(new BaseResponse() { Error = ErrorEnum.InvalidOperationException, IsSuccess = false });
            }
            catch (Exception ex)
            {
                _logger.WriteToLogFile("Exception: " + ex.Message);
                ConnectionError?.Invoke(new BaseResponse() { Error = ErrorEnum.UnknownError, IsSuccess = false });
            }
        }

        public void Close()
        {
            _connection.socket.Close();
            _connection.socket = null;
        }
    }
}
