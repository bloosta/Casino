using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CasinoFrameworkApp.Infrastructure;
using CasinoFrameworkApp.Services;

namespace CasinoFrameworkApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IApiClientService _api;
        private string _username = string.Empty;
        private string _password = string.Empty;

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IApiClientService api)
        {
            _api = api;
            LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => CanLogin());
        }

        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password);


        public event Action? LoginSucceeded;
        private async Task LoginAsync()
        {
            bool ok = await _api.LoginAsync(Username, Password);
            if (ok)
            {
                LoginSucceeded?.Invoke();     
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
