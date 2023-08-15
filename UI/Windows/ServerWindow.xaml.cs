using Core.Logger.Interface;
using Server.TCP.Interface;
using System.Windows.Navigation;
using UI.Factory.Interfaces;
using UI.Windows.Server;

namespace UI.Windows
{
    /// <summary>
    /// Lógica interna para ServerWindow.xaml
    /// </summary>
    public partial class ServerWindow : NavigationWindow
    {
        private readonly ILogger _logger;
        public ServerWindow(IFindClientPageFactory findClientPage, ILogger logger)
        {
            _logger = logger;

            InitializeComponent();

            Navigate(findClientPage.Create());
        }
    }
}
