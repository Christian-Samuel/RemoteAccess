using Client.RealTimeTransmission;
using Client.RealTimeTransmission.Interfaces;
using Client.TCP;
using Client.TCP.Interface;
using Core.Configuration;
using Core.DataProcessing;
using Core.DataProcessing.Interfaces;
using Core.Logger;
using Core.Logger.Interface;
using Core.TCP;
using Core.TCP.Interface;
using Microsoft.Extensions.DependencyInjection;
using Server.RealTimeTransmission;
using Server.RealTimeTransmission.Interface;
using Server.TCP;
using Server.TCP.Interface;
using System.Windows;
using UI.Factory;
using UI.Factory.Interfaces;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<IPreferences, Preferences>();
            
            services.AddSingleton<ITcpConnection, TcpConnection>();
            services.AddSingleton<ITcpServerConnection, TcpServerConnection>();
            services.AddSingleton<ITcpClientConnection, TcpClientConnection>();

            services.AddSingleton<IDataProcessing, DataProcessing>();
            services.AddSingleton<ICastScreenServer, CastScreenServer>();
            services.AddSingleton<ICastScreenClient, CastScreenClient>();
            
            services.AddSingleton<IServerWindowFactory, ServerWindowFactory>();
            services.AddSingleton<IFindClientPageFactory, FindClientPageFactory>();
            services.AddSingleton<IServerPageFactory, ServerPageFactory>();
            services.AddSingleton<IClientWindowFactory, ClientWindowFactory>();

            services.AddTransient<MainWindow>();
        }
    }
}
