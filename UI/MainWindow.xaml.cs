using Client.TCP.Interface;
using Core.Logger.Interface;
using Server.TCP.Interface;
using Server.TCP.Model;
using System.Threading.Tasks;
using System.Windows;
using UI.Factory.Interfaces;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ITcpClientConnection _client;
        private readonly ITcpServerConnection _server;

        private readonly IServerWindowFactory _serverWindowFactory;
        private readonly IClientWindowFactory _clientWindowFactory;
        private readonly ILogger _logger;
        //private readonly ISettingsWindowFactory _settingsWindowFactory;

        public MainWindow(ITcpClientConnection tcpClientConnection, ITcpServerConnection tcpServerConnection, IServerWindowFactory serverWindowFactory, IClientWindowFactory clientWindowFactory, ILogger logger)
        {
            _logger = logger;

            InitializeComponent();
            _client = tcpClientConnection;
            _server = tcpServerConnection;

            _serverWindowFactory = serverWindowFactory;
            _clientWindowFactory = clientWindowFactory;

            _server.StartServerConnectionChanged += OnConnectionChanged;
        }

        private void OnConnectionChanged(TcpServerResponse<string> response)
        {
            Dispatcher.Invoke(() =>
            {
                if (response.IsSuccess )
                {
                    var serverWindow = _serverWindowFactory.Create();

                    serverWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show($"Erro: {response.Error}");

                    buttonStartServer.IsEnabled = true;
                }
            });
        }

        private void ButtonStartServer_Click(object sender, RoutedEventArgs e)
        {
            buttonStartServer.IsEnabled = false;

            Task.Run(() =>
            {
                _server.StartServerAsync();
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = _clientWindowFactory.Create();
            client.Show();
            Close();
        }
    }
}
