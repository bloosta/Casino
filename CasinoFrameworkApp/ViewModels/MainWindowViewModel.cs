using CasinoFrameworkApp.Services;

namespace CasinoFrameworkApp.ViewModels
{
    public class MainWindowViewModel
    {
        public ClientsViewModel ClientsVM { get; }
        public GamesViewModel GamesVM { get; }

        public MainWindowViewModel(
            ClientsViewModel clientsVm,
            GamesViewModel gamesVm)
        {
            ClientsVM = clientsVm;
            GamesVM = gamesVm;
        }
    }
}