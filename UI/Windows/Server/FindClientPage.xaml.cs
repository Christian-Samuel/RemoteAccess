using Core.Logger.Interface;
using Core.TCP.Interface;
using Server.TCP.Interface;
using Server.TCP.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UI.Factory.Interfaces;

namespace UI.Windows.Server
{
    /// <summary>
    /// Interação lógica para FindClientPage.xam
    /// </summary>
    public partial class FindClientPage : Page
    {
        private readonly ITcpServerConnection _server;
        private readonly ITcpConnection _connection;
        private readonly IServerPageFactory _serverPageFactory;
        private readonly ILogger _logger;

        public FindClientPage(ITcpServerConnection server, ITcpConnection connection, IServerPageFactory serverPageFactory, ILogger logger)
        {
            _logger = logger;
            _connection = connection;
            InitializeComponent();

            _server = server;
            _serverPageFactory = serverPageFactory;

            label_IpServer.Content = $"Seu IP é: {_connection.ipServer}";

            _server.WaitClientConnectionChanged += WaitClientConnectionChanged;
            _server.WaitForConnectionAsync();
        }

        #region Events

        private void WaitClientConnectionChanged(TcpServerResponse<string> response)
        {
            Dispatcher.Invoke(() =>
            {
                if (response.IsSuccess)
                {
                    MessageBox.Show($"{_connection.ipClient} se conectou");

                    label_ClienteConectou.Visibility = Visibility.Visible;
                    label_ClienteConectou.Content = "Conectado a : " + _connection.ipClient;

                    ResizeWindow();

                    NavigationService.Navigate(_serverPageFactory.Create());
                }
                else
                {
                    MessageBox.Show($"Erro: {response.Error}" +
                        $"\nTentando novamente...");

                    button_TentarNovamente.IsEnabled = true;
                }
            });
        }

        #endregion

        private void button_TentarNovamente_Click(object sender, RoutedEventArgs e)
        {
            button_TentarNovamente.IsEnabled = false;

            _server.WaitForConnectionAsync();
        }

        private void ResizeWindow()
        {
            var parentWindow = Window.GetWindow(this);

            if (parentWindow is not null)
            {
                parentWindow.SizeToContent = SizeToContent.Manual;
                parentWindow.Width = 1024;
                parentWindow.Height = 768;
            }
        }
    }
}
