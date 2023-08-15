using Core.Logger.Interface;
using Core.TCP.Interface;
using Server.TCP.Interface;
using UI.Factory.Interfaces;
using UI.Windows.Server;

namespace UI.Factory
{
    internal class FindClientPageFactory : IFindClientPageFactory
    {
        private readonly ITcpServerConnection _server;
        private readonly ITcpConnection _connection;
        private readonly IServerPageFactory _serverPageFactory;
        private readonly ILogger _logger;

        public FindClientPageFactory(ITcpServerConnection server, ITcpConnection connection, IServerPageFactory serverPageFactory, ILogger logger)
        {
            _server = server;
            _serverPageFactory = serverPageFactory;
            _connection = connection;
            _logger = logger;
        }

        public FindClientPage Create()
        {
            return new FindClientPage(_server, _connection, _serverPageFactory, _logger);
        }
    }
}
