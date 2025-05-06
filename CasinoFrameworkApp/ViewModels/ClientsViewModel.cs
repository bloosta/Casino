// ViewModels/ClientsViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CasinoFrameworkApp.Infrastructure;
using CasinoFrameworkApp.Services;

namespace CasinoFrameworkApp.ViewModels
{
    public class ClientsViewModel : BaseViewModel
    {
        private readonly IApiClientService _api;
        public ObservableCollection<ClientDto> Clients { get; } = new();

        private string _filter = string.Empty;
        public string Filter
        {
            get => _filter;
            set { if (Set(ref _filter, value)) _ = LoadClientsAsync(); }
        }

        public ICommand CreateCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        public ClientsViewModel(IApiClientService api)
        {
            _api = api;
            CreateCommand = new RelayCommand(async _ => await CreateAsync());
            UpdateCommand = new RelayCommand(async id => await UpdateAsync((int)id));
            DeleteCommand = new RelayCommand(async id => await DeleteAsync((int)id));
            RefreshCommand = new RelayCommand(async _ => await LoadClientsAsync());

            _ = LoadClientsAsync();
        }

        private async Task LoadClientsAsync()
        {
            try
            {
                var list = await _api.SearchClientsAsync(Filter);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Clients.Clear();
                    foreach (var c in list) Clients.Add(c);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CreateAsync()
        {
            // TODO: спросить имя через диалог
            await _api.CreateClientAsync("NewClient");
            await LoadClientsAsync();
        }

        private async Task UpdateAsync(int id)
        {
            // TODO: спросить новое имя
            await _api.UpdateClientAsync(id, "RenamedClient");
            await LoadClientsAsync();
        }

        private async Task DeleteAsync(int id)
        {
            if (MessageBox.Show("Удалить клиента?", "", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            await _api.DeleteClientAsync(id);
            await LoadClientsAsync();
        }
    }
}
