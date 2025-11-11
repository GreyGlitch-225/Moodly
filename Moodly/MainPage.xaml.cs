using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;


namespace Moodly
{
    public partial class MainPage : ContentPage
    {
        private readonly MoodService _moodService = new MoodService();
        private ObservableCollection<MoodEntry> _entries = new();

        public MainPage()
        {
            InitializeComponent();
            MoodList.ItemsSource = _entries;
            LoadMoodHistory();
            VersionLabel.Text = $"Версия: {AppInfo.VersionString}";
        }

        private void LoadMoodHistory()
        {
            var loaded = _moodService.LoadEntries() ?? new ObservableCollection<MoodEntry>();
            var recentEntries = loaded
                .Where(e => e.Date.Date == DateTime.Now.Date)
                .OrderByDescending(e => e.Date)
                .ToList();

            _entries.Clear();
            foreach (var entry in recentEntries)
                _entries.Add(entry);
        }

        private async void OnSave_Clicked(object sender, EventArgs e)
        {
            if (EmojiPicker.SelectedItem == null)
            {
                await DisplayAlert("Ошибка", "Выберите эмодзи!", "OK");
                return;
            }

            var newEntry = new MoodEntry
            {
                Date = DateTime.Now,
                Emoji = EmojiPicker.SelectedItem.ToString(),
                Note = NoteEntry.Text
            };

            _entries.Add(newEntry);
            _moodService.SaveEntries(_entries);

            // Сброс полей
            EmojiPicker.SelectedItem = null;
            NoteEntry.Text = "";

            await DisplayAlert("Сохранено", "Запись добавлена!", "OK");
        }

        private async void OnDelete_Clicked(object sender, EventArgs e)
        {
            if (sender is MenuItem menu && menu.CommandParameter is MoodEntry entry)
            {
                var confirm = await DisplayAlert("Удалить", "Вы уверены, что хотите удалить запись?", "Да", "Нет");
                if (!confirm) return;

                _entries.Remove(entry);
                _moodService.SaveEntries(_entries);
            }
        }
    }
}
