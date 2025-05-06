using CasinoFrameworkApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CasinoFrameworkApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            vm.LoginSucceeded += () =>
            {
                // Получаем GamesWindow из DI
                var games = App.Services.GetRequiredService<MainWindow>();

                // Делаем его главным окном приложения
                Application.Current.MainWindow = games;

                // Показываем GamesWindow
                games.Show();

                // Теперь, когда GamesWindow — MainWindow, можно закрыть LoginWindow без shutdown
                this.Close();
            };
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }
    }


}
