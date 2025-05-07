using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using CasinoFrameworkApp.Services;
using CasinoFrameworkApp.ViewModels;

namespace CasinoFrameworkApp
{
    public partial class MainWindow : Window
    {
        private readonly GamesViewModel _gamesVm;
        private readonly ClientsViewModel _clientsVm;

        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            _gamesVm = vm.GamesVM;
            _clientsVm = vm.ClientsVM;

            // Маска для From
            DatePickerFrom.Loaded += (_, __) =>
            {
                if (DatePickerFrom.Template.FindName("PART_TextBox", DatePickerFrom)
                    is DatePickerTextBox dpTxt)
                {
                    bool isFormatting = false;
                    dpTxt.PreviewTextInput += (s, e) =>
                        e.Handled = !char.IsDigit(e.Text, 0);
                    dpTxt.TextChanged += (s, e) =>
                    {
                        if (isFormatting) return;
                        isFormatting = true;
                        var tb = (DatePickerTextBox)s;
                        int oldPos = tb.SelectionStart;
                        int digitsLeft = tb.Text.Take(oldPos).Count(char.IsDigit);
                        var digits = new string(tb.Text.Where(char.IsDigit).ToArray());
                        if (digits.Length > 8) digits = digits[..8];

                        string formatted = "";
                        for (int i = 0; i < digits.Length; i++)
                        {
                            if (i == 2 || i == 4) formatted += ".";
                            formatted += digits[i];
                        }

                        tb.Text = formatted;
                        int newPos = digitsLeft;
                        if (newPos > 2) newPos++;
                        if (newPos > 4) newPos++;
                        tb.SelectionStart = newPos <= formatted.Length
                                           ? newPos
                                           : formatted.Length;
                        isFormatting = false;
                    };
                }
            };

            // Маска для To
            DatePickerTo.Loaded += (_, __) =>
            {
                if (DatePickerTo.Template.FindName("PART_TextBox", DatePickerTo)
                    is DatePickerTextBox dpTxt)
                {
                    bool isFormatting = false;
                    dpTxt.PreviewTextInput += (s, e) =>
                        e.Handled = !char.IsDigit(e.Text, 0);
                    dpTxt.TextChanged += (s, e) =>
                    {
                        if (isFormatting) return;
                        isFormatting = true;
                        var tb = (DatePickerTextBox)s;
                        int oldPos = tb.SelectionStart;
                        int digitsLeft = tb.Text.Take(oldPos).Count(char.IsDigit);
                        var digits = new string(tb.Text.Where(char.IsDigit).ToArray());
                        if (digits.Length > 8) digits = digits[..8];

                        string formatted = "";
                        for (int i = 0; i < digits.Length; i++)
                        {
                            if (i == 2 || i == 4) formatted += ".";
                            formatted += digits[i];
                        }

                        tb.Text = formatted;
                        int newPos = digitsLeft;
                        if (newPos > 2) newPos++;
                        if (newPos > 4) newPos++;
                        tb.SelectionStart = newPos <= formatted.Length
                                           ? newPos
                                           : formatted.Length;
                        isFormatting = false;
                    };
                }
            };
        }

        private async void OnEditClient_Click(object sender, RoutedEventArgs e)
        {
            var dto = (ClientDto)((FrameworkElement)sender).DataContext;
            var wnd = new EditClientWindow(dto) { Owner = this };
            if (wnd.ShowDialog() != true) return;

            await _clientsVm.UpdateClientAsync(dto.Id, wnd.NewName);
            await _clientsVm.ExecuteRefreshAsync();
        }
        private async void OnEditGame_Click(object sender, RoutedEventArgs e)
        {
            var dto = (GameDto)((FrameworkElement)sender).DataContext;
            var wnd = new EditGameWindow(dto) { Owner = this };
            if (wnd.ShowDialog() != true) return;

            await _gamesVm.UpdateGameAsync(dto.Id, wnd.PlayedAt, wnd.Type, wnd.PlayerIds);
            await _gamesVm.ExecuteRefreshAsync();
        }


    }
}
