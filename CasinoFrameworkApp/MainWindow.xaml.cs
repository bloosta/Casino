using System.Windows;
using CasinoFrameworkApp.ViewModels;

namespace CasinoFrameworkApp
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }

}
