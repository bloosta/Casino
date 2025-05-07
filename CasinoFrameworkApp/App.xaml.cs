using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CasinoFrameworkApp.Services;
using CasinoFrameworkApp.ViewModels;

namespace CasinoFrameworkApp
{
    public partial class App : Application
    {
        private readonly IHost _host;
        public static IServiceProvider Services { get; private set; } = null!;  // <- инициализация

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddHttpClient("CasinoApi", c => c.BaseAddress = new Uri("http://localhost:49726/"));
                    services.AddScoped<IApiClientService, ApiClientService>();

                    services.AddTransient<LoginWindow>();
                    services.AddTransient<LoginViewModel>();

                    services.AddTransient<ClientsViewModel>();
                    services.AddTransient<GamesViewModel>();
                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<MainWindow>();
                })
                .Build();

            Services = _host.Services;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            var login = Services.GetRequiredService<LoginWindow>();
            login.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
