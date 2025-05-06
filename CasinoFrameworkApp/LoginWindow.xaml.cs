using CasinoFrameworkApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CasinoFrameworkApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }

}
