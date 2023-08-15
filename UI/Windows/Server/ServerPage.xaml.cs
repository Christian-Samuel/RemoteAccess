using Core.Configuration;
using Core.Logger.Interface;
using Server.RealTimeTransmission.Interface;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace UI.Windows.Server
{
    /// <summary>
    /// Interação lógica para ServerPage.xam
    /// </summary>
    public partial class ServerPage : Page
    {
        private readonly ICastScreenServer _castScreen;
        private readonly IPreferences _preferences;
        private readonly ILogger _logger;

        public ServerPage(ICastScreenServer castScreen, IPreferences preferences, ILogger logger)
        {
            InitializeComponent();

            _logger = logger;
            _castScreen = castScreen;
            _preferences = preferences;
            
            _castScreen.FrameReceiveCompleted += OnFrameReceive;
            _logger.WriteToLogFile("Exibição iniciada");
        }

        private void OnFrameReceive(BitmapImage response)
        {
            Dispatcher.Invoke(() =>
            {
                screen_server.Source = response;
                screen_server.Stretch = Stretch.Fill;
            });
        }
    }
}
