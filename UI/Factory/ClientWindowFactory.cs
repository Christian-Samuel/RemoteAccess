using Client.RealTimeTransmission.Interfaces;
using Client.TCP.Interface;
using Core.Logger.Interface;
using UI.Factory.Interfaces;
using UI.Windows;

namespace UI.Factory
{
    internal class ClientWindowFactory : IClientWindowFactory
    {
        private readonly ITcpClientConnection _client;
        private readonly ICastScreenClient _castScreenClient;
        private readonly ILogger _logger;

        public ClientWindowFactory(ITcpClientConnection client, ILogger logger, ICastScreenClient castScreenClient)
        {
            _client = client;
            _castScreenClient = castScreenClient;
            _logger = logger;
        }

        public ClientWindow Create()
        {
            return new ClientWindow(_client, _logger, _castScreenClient);
        }
    }
}
