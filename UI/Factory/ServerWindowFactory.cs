using Core.Logger.Interface;
using UI.Factory.Interfaces;
using UI.Windows;

namespace UI.Factory
{
    internal class ServerWindowFactory : IServerWindowFactory
    {
        private readonly IFindClientPageFactory _findClientPage;
        private readonly ILogger _logger;

        public ServerWindowFactory(IFindClientPageFactory findClientPage, ILogger logger)
        {
            _findClientPage = findClientPage;
            _logger = logger;
        }

        public ServerWindow Create()
        {
            return new ServerWindow(_findClientPage, _logger);
        }
    }
}
