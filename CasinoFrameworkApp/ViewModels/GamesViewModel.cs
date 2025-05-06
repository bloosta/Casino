using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CasinoFrameworkApp.Infrastructure;
using CasinoFrameworkApp.Services;

namespace CasinoFrameworkApp.ViewModels
{
    public class GamesViewModel : BaseViewModel
    {
        private readonly IApiClientService _api;

        public ObservableCollection<GameDto> Games { get; } = new();

        // Фильтры
        private DateTime? _from;
        public DateTime? From
        {
            get => _from;
            set { if (Set(ref _from, value)) _ = LoadGamesAsync(); }
        }

        private DateTime? _to;
        public DateTime? To
        {
            get => _to;
            set { if (Set(ref _to, value)) _ = LoadGamesAsync(); }
        }

        private string _typeFilter = string.Empty;
        public string TypeFilter
        {
            get => _typeFilter;
            set { if (Set(ref _typeFilter, value)) _ = LoadGamesAsync(); }
        }

        private int? _clientIdFilter;
        public int? ClientIdFilter
        {
            get => _clientIdFilter;
            set { if (Set(ref _clientIdFilter, value)) _ = LoadGamesAsync(); }
        }

        // Команды
        public ICommand CreateCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        public GamesViewModel(IApiClientService api)
        {
            _api = api;
            CreateCommand = new RelayCommand(async _ => await CreateGameAsync());
            UpdateCommand = new RelayCommand(async id => await UpdateGameAsync((int)id));
            DeleteCommand = new RelayCommand(async id => await DeleteGameAsync((int)id));
            RefreshCommand = new RelayCommand(async _ => await LoadGamesAsync());

            _ = LoadGamesAsync();
        }

        private async Task LoadGamesAsync()
        {
            try
            {
                var list = await _api.SearchGamesAsync(From, To, TypeFilter, ClientIdFilter);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Games.Clear();
                    foreach (var g in list) Games.Add(g);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке игр: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CreateGameAsync()
        {
            // TODO: спросить у пользователя параметры новой игры (дату, тип, список клиентов)
            var now = DateTime.Now;
            await _api.AddGameAsync(now, "NewGame", new());
            await LoadGamesAsync();
        }

        private async Task UpdateGameAsync(int id)
        {
            // TODO: спросить у пользователя новые параметры
            var now = DateTime.Now;
            await _api.UpdateGameAsync(id, now, "UpdatedType", new());
            await LoadGamesAsync();
        }

        private async Task DeleteGameAsync(int id)
        {
            if (MessageBox.Show("Удалить игру?", "", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            await _api.DeleteGameAsync(id);
            await LoadGamesAsync();
        }

        public async Task UpdateGameAsync(int id, DateTime playedAt, string type, List<int> playerIds)
        {
            await _api.UpdateGameAsync(id, playedAt, type, playerIds);
        }

        public async Task ExecuteRefreshAsync()
        {
            await LoadGamesAsync();
        }

    }
}
