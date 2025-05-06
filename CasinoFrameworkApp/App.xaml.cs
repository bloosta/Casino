    using System;
    using System.Windows;
    using CasinoFrameworkApp.ViewModels;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using CasinoFrameworkApp.Services;


    namespace CasinoFrameworkApp
    {
        public partial class App : Application
        {
            private readonly IHost _host;

            public App()
            {
                _host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        services.AddHttpClient("CasinoApi", client =>
                        {
                            client.BaseAddress = new Uri("http://localhost:49726/");
                        });
                        services.AddScoped<IApiClientService, ApiClientService>();

                        services.AddTransient<LoginWindow>();
                        services.AddTransient<LoginViewModel>();
                        services.AddTransient<GamesWindow>();
                        services.AddTransient<GamesViewModel>();

                    })
                    .Build();
            }

            protected override async void OnStartup(StartupEventArgs e)
            {
                await _host.StartAsync();
                var login = _host.Services.GetRequiredService<LoginWindow>();
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
