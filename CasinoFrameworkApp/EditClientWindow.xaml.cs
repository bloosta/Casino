using CasinoFrameworkApp.Services;
using System.Windows;

namespace CasinoFrameworkApp
{
    public partial class EditClientWindow : Window
    {
        public string NewName { get; private set; } = string.Empty;

        public EditClientWindow(ClientDto dto)
        {
            InitializeComponent();
            TextBoxName.Text = dto.Name;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            var name = TextBoxName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите имя клиента.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            NewName = name;
            DialogResult = true;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
