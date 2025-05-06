using System.Windows;
using CasinoFrameworkApp.ViewModels;

namespace CasinoFrameworkApp
{
    public partial class GamesWindow : Window
    {
        public GamesWindow(GamesViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
