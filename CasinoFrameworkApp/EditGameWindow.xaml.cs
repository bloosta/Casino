using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using CasinoFrameworkApp.Services;

namespace CasinoFrameworkApp
{
    public partial class EditGameWindow : Window
    {
        public DateTime PlayedAt { get; private set; }
        public string Type { get; private set; } = string.Empty;
        public List<int> PlayerIds { get; private set; } = new();

        // Флаги для форматирования
        private bool _isDateFormatting = false;
        private bool _isTimeFormatting = false;

        public EditGameWindow(GameDto dto)
        {
            InitializeComponent();

            // Инициализируем поля
            DatePickerPlayedAt.SelectedDate = dto.PlayedAt.Date;
            TextBoxTime.Text = dto.PlayedAt.ToString("HH:mm:ss");
            TextBoxType.Text = dto.Type;
            TextBoxPlayers.Text = string.Join(",", dto.PlayerIds);

            // После загрузки DatePicker, найдём внутри его TextBox и повесим обработчики
            DatePickerPlayedAt.Loaded += (_, __) =>
            {
                if (DatePickerPlayedAt.Template.FindName("PART_TextBox", DatePickerPlayedAt)
                    is DatePickerTextBox dpTextBox)
                {
                    dpTextBox.PreviewTextInput += DatePickerTxt_PreviewTextInput;
                    dpTextBox.TextChanged += DatePickerTxt_TextChanged;
                }
            };
        }

        // Блокируем в дате всё, кроме цифр
        private void DatePickerTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        // Форматируем ввод даты dd.MM.yyyy
        private void DatePickerTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isDateFormatting) return;
            _isDateFormatting = true;

            var txt = (DatePickerTextBox)sender;
            int oldPos = txt.SelectionStart;
            // сколько цифр слева
            int digitsLeft = txt.Text.Take(oldPos).Count(char.IsDigit);

            // берем только цифры, максимум 8
            var digits = new string(txt.Text.Where(char.IsDigit).ToArray());
            if (digits.Length > 8) digits = digits.Substring(0, 8);

            // строим форматированную строку
            string formatted = "";
            for (int i = 0; i < digits.Length; i++)
            {
                if (i == 2 || i == 4) formatted += ".";
                formatted += digits[i];
            }

            txt.Text = formatted;

            // пересчитываем новую позицию курсора
            int newPos = digitsLeft;
            if (newPos > 2) newPos++;
            if (newPos > 4) newPos++;
            txt.SelectionStart = Math.Min(newPos, formatted.Length);

            _isDateFormatting = false;
        }

        // Остальной код для времени, типа и игроков остаётся без изменений:
        private void TextBoxTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void TextBoxTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isTimeFormatting) return;
            _isTimeFormatting = true;

            var tb = (System.Windows.Controls.TextBox)sender;
            int oldCaret = tb.SelectionStart;
            int digitsLeft = tb.Text.Take(oldCaret).Count(char.IsDigit);

            var digits = new string(tb.Text.Where(char.IsDigit).ToArray());
            if (digits.Length > 6) digits = digits.Substring(0, 6);

            string formatted = "";
            for (int i = 0; i < digits.Length; i++)
            {
                if (i == 2 || i == 4) formatted += ":";
                formatted += digits[i];
            }

            tb.Text = formatted;

            int newPos = digitsLeft;
            if (newPos > 2) newPos++;
            if (newPos > 4) newPos++;
            tb.CaretIndex = Math.Min(newPos, formatted.Length);

            _isTimeFormatting = false;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            // Дата
            if (DatePickerPlayedAt.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату.", "Validation",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Парсим дату+время
            var date = DatePickerPlayedAt.SelectedDate.Value;
            if (!TimeSpan.TryParse(TextBoxTime.Text.Trim(), out var ts))
            {
                MessageBox.Show("Введите корректное время (ЧЧ:ММ:СС).",
                                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Тип
            var type = TextBoxType.Text.Trim();
            if (string.IsNullOrEmpty(type))
            {
                MessageBox.Show("Укажите тип игры.", "Validation",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Игроки
            var ids = TextBoxPlayers.Text
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s.Trim(), out var i) ? (int?)i : null)
                .Where(i => i.HasValue)
                .Select(i => i.Value)
                .ToList();
            if (ids.Count == 0)
            {
                MessageBox.Show("Нужно указать хотя бы одного игрока.", "Validation",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем результаты
            PlayedAt = date.Date + ts;
            Type = type;
            PlayerIds = ids;
            DialogResult = true;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
