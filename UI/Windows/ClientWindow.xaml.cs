using Client.RealTimeTransmission.Interfaces;
using Client.TCP.Interface;
using Core.Logger;
using Core.Logger.Interface;
using System;
using System.Windows;
using System.Windows.Threading;

namespace UI.Windows
{
    /// <summary>
    /// Lógica interna para ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private readonly ITcpClientConnection _connection;
        private readonly ICastScreenClient _castScreenClient;
        private readonly ILogger _logger;

        private DispatcherTimer timer;

        public ClientWindow(ITcpClientConnection client, ILogger logger, ICastScreenClient castScreenClient)
        {
            InitializeComponent();
            _connection = client;
            _castScreenClient = castScreenClient;
            _logger = logger;

            _connection.ConnectionError += OnFailedConnection;
            _connection.ConnectionSuccess += OnSucessConnection;
        }

        private void InatializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += (s, e) =>
            {
                _castScreenClient.CaptureScreen(50);
            };

            timer.Start();
        }

        private void OnSucessConnection()
        {
            InatializeTimer();

            label_StatulConnection.Content = "Conectado ao:";
            textbox_InputIpServer.IsEnabled = false;
            button_ClientConnect.IsEnabled = false;

            slider_FPS.IsEnabled = true;
        }

        private void OnFailedConnection(BaseResponse response)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Não foi possivel se conectar a esse IP");
            });
        }

        private void Button_ClientConnect_Click(object sender, RoutedEventArgs e)
        {
            if(textbox_InputIpServer.Text.Length > 0)
            {
                _connection.ConnectAsync(textbox_InputIpServer.Text, 8000);
            }
            else
            {
                MessageBox.Show("Informe o IP.");
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (timer is not null)
            {
                label_FPS.Content = "Frames por segundo: " + (1000 / e.NewValue);
                timer.Interval = TimeSpan.FromMilliseconds(e.NewValue);
            }
        }
    }
}
