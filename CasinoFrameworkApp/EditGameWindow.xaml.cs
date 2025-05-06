using CasinoFrameworkApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CasinoFrameworkApp
{
    public partial class EditGameWindow : Window
    {
        public DateTime PlayedAt { get; private set; }
        public string Type { get; private set; } = string.Empty;
        public List<int> PlayerIds { get; private set; } = new();

        public EditGameWindow(GameDto dto)
        {
            InitializeComponent();
            DatePickerPlayedAt.SelectedDate = dto.PlayedAt;
            TextBoxType.Text = dto.Type;
            TextBoxPlayers.Text = string.Join(",", dto.PlayerIds);
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            if (DatePickerPlayedAt.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PlayedAt = DatePickerPlayedAt.SelectedDate.Value;
            Type = TextBoxType.Text.Trim();
            if (string.IsNullOrEmpty(Type))
            {
                MessageBox.Show("Введите тип игры.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Разбираем список игроков
            var text = TextBoxPlayers.Text;
            PlayerIds = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(t =>
                                int.TryParse(t.Trim(), out var id) ? id : (int?)null)
                            .Where(x => x.HasValue)
                            .Select(x => x!.Value)
                            .ToList();

            DialogResult = true;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
