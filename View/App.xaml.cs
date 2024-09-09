using System;
using System.Windows;
using Core.Settings;
using Data.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDatabase();
            serviceCollection.AddService();
            serviceCollection.AddTransient<DepositPage>();
            serviceCollection.AddTransient<MainWindow>();
            serviceCollection.AddTransient<InfoPage>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            ServiceProvider.InitializeDatabase();

            base.OnStartup(e);
        }
    }
}