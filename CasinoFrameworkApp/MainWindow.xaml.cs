using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows;
using CasinoFrameworkApp.Services;
using CasinoFrameworkApp.ViewModels;

namespace CasinoFrameworkApp
{
    public partial class MainWindow : Window
    {
        private readonly GamesViewModel _gamesVm;

        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            _gamesVm = vm.GamesVM;
        }

        private async void OnEditGame_Click(object sender, RoutedEventArgs e)
        {
            var dto = (GameDto)((FrameworkElement)sender).DataContext;

            var wnd = new EditGameWindow(dto) { Owner = this };
            if (wnd.ShowDialog() != true) return;

            await _gamesVm.UpdateGameAsync(
                dto.Id,
                wnd.PlayedAt,
                wnd.Type,
                wnd.PlayerIds);

            await _gamesVm.ExecuteRefreshAsync();
        }


    }
}
