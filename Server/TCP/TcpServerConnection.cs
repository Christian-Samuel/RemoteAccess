using System.Net;
using System.Net.Sockets;
using Core.Logger.Interface;
using Core.Logger.Enum;
using Core.TCP;
using Server.TCP.Interface;
using Server.TCP.Model;
using Server.TCP.Status;
using Core.TCP.Interface;

namespace Server.TCP
{
    public class TcpServerConnection : ITcpServerConnection
    {
        public delegate void ConnectionChangedHandler(TcpServerResponse<string> response);
        public event ConnectionChangedHandler StartServerConnectionChanged;
        public event ConnectionChangedHandler WaitClientConnectionChanged;

        private Socket _listener;
        private int _timeOut;

        private readonly ITcpConnection _connection;
        private readonly ILogger _logger;

        public TcpServerConnection(ILogger logger, ITcpConnection connection)
        {
            _connection = connection;
            _logger = logger;

            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _timeOut = 60000;
        }

        public void StartServerAsync()
        {
            try
            {
                var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                _connection.ipServer = hostEntry.AddressList.FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork)?.ToString();

                if (string.IsNullOrEmpty(_connection.ipServer))
                {
                    throw new Exception("Não foi possível encontrar um endereço IP IPv4 local");
                }

                _listener.Bind(new IPEndPoint(IPAddress.Parse(_connection.ipServer), 8000));
                _listener.Listen(1);

                _logger.WriteToLogFile("Servidor criado");
                StartServerConnectionChanged?.Invoke(new TcpServerResponse<string>()
                {
                    IsSuccess = true,
                    Error = ErrorEnum.None,
                    Status = TcpServerStatus.WaitingForConnection,
                    ObjResponse = _connection.ipServer
                });
            }
            catch (Exception ex)
            {
                _logger.WriteToLogFile($"Ocorreu uma exceção ao iniciar o servidor: {ex.Message}");

                StartServerConnectionChanged?.Invoke(new TcpServerResponse<string>()
                {
                    IsSuccess = false,
                    Error = ErrorEnum.NoConnection,
                    Status = TcpServerStatus.Disconnected
                });
            }
        }

        public async Task WaitForConnectionAsync()
        {
            try
            {
                var acceptTask = _listener.AcceptAsync();
                var timeoutTask = Task.Delay(_timeOut);

                await Task.WhenAny(acceptTask, timeoutTask);

                if (acceptTask.IsCompleted)
                {
                    _connection.socket = acceptTask.Result;

                    _connection.ipClient = ((IPEndPoint)_connection.socket.RemoteEndPoint).Address.ToString();

                    _listener.Close();

                    _connection.InitializeSocketEvent();
                    _connection.StartReceive();

                    _logger.WriteToLogFile("Conexão com cliente efetuada");
                    WaitClientConnectionChanged?.Invoke(new TcpServerResponse<string>()
                    {
                        IsSuccess = true,
                        Error = ErrorEnum.None,
                        Status = TcpServerStatus.Connected,
                        ObjResponse = _connection.ipClient
                    });
                }
                else
                {
                    _listener.Close();

                    WaitClientConnectionChanged?.Invoke(new TcpServerResponse<string>()
                    {
                        IsSuccess = false,
                        Error = ErrorEnum.TcpServerConnectionTimeOut,
                        Status = TcpServerStatus.Disconnected
                    });
                }
            }
            catch (SocketException ex)
            {
                _logger.WriteToLogFile($"Erro de socket: {ex.SocketErrorCode}. Mensagem: {ex.Message}");

                WaitClientConnectionChanged?.Invoke(new TcpServerResponse<string>()
                {
                    Error = ErrorEnum.SocketException,
                    IsSuccess = false,
                    Status = TcpServerStatus.Disconnected
                });
            }
            catch (Exception ex)
            {
                _logger.WriteToLogFile($"Ocorreu uma exceção ao aguardar uma conexão: {ex.Message}");

                WaitClientConnectionChanged?.Invoke(new TcpServerResponse<string>()
                {
                    Error = ErrorEnum.UnknownError,
                    IsSuccess = false,
                    Status = TcpServerStatus.Disconnected
                });
            }
        }
    }
}
