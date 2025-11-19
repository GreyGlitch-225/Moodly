using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace Moodly
{
    public partial class MainPage : ContentPage
    {
        private readonly MoodService _moodService = new();
        private readonly ObservableCollection<MoodEntry> _entries = new();

        public MainPage()
        {
            InitializeComponent();

            MoodList.ItemsSource = _entries;
            LoadMoodHistory();
            VersionLabel.Text = $"Версия: {AppInfo.VersionString}";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ApplyPlatformLayoutAdjustments();
        }

        private void ApplyPlatformLayoutAdjustments()
        {
            
            var header = this.FindByName<Layout?>("HeaderLayout");
         
            var fillAndExpand = LayoutOptions.Fill;
            fillAndExpand.Expands = true;

            if (OperatingSystem.IsAndroid())
            {
                if (header is not null)
                    header.HorizontalOptions = fillAndExpand;

                EmojiPicker.HorizontalOptions = fillAndExpand;
                NoteEntry.HorizontalOptions = fillAndExpand;
                MoodList.HorizontalOptions = fillAndExpand;
                MoodList.VerticalOptions = fillAndExpand;
            }

            else if (OperatingSystem.IsWindows())
            {
                if (header is not null)
                {
                    header.HorizontalOptions = LayoutOptions.Center;
                    header.WidthRequest = 950;
                }

                EmojiPicker.WidthRequest = 900;
                NoteEntry.WidthRequest = 900;
                OnSave.WidthRequest = 250;
                MoodList.WidthRequest = 1000;
            }
        }

        private void LoadMoodHistory()
        {
            var loaded = _moodService.LoadEntries() ?? new ObservableCollection<MoodEntry>();
            var recentEntries = loaded
                .Where(e => e.Date.Date == DateTime.Today)
                .OrderByDescending(e => e.Date);

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
                Emoji = EmojiPicker.SelectedItem?.ToString() ?? string.Empty,
                Note = NoteEntry.Text ?? string.Empty
            };

            _entries.Add(newEntry);
            _moodService.SaveEntries(_entries);

            EmojiPicker.SelectedItem = null;
            NoteEntry.Text = string.Empty;

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
