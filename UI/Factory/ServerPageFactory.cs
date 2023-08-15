using Core.Configuration;
using Core.Logger.Interface;
using Server.RealTimeTransmission.Interface;
using UI.Factory.Interfaces;
using UI.Windows.Server;

namespace UI.Factory
{
    internal class ServerPageFactory : IServerPageFactory
    {
        private readonly ICastScreenServer _cast;
        private readonly IPreferences _preferences;
        private readonly ILogger _logger;

        public ServerPageFactory(ICastScreenServer cast, IPreferences preferences, ILogger logger)
        {
            _cast = cast;
            _preferences = preferences;
            _logger = logger;
        }

        public ServerPage Create()
        {
            return new ServerPage(_cast, _preferences, _logger);
        }
    }
}
