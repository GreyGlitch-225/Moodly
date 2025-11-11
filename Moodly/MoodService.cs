using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Maui.Storage;
using System.Text.Json;

namespace Moodly
{
    public class MoodService
    {
        private const string StorageKey = "mood_entries";

        public ObservableCollection<MoodEntry> LoadEntries() 
        {
            var json = Preferences.Default.Get(StorageKey, "");
            if (string.IsNullOrEmpty(json))
                return new ObservableCollection<MoodEntry>();

            return JsonSerializer.Deserialize<ObservableCollection<MoodEntry>>(json);
        }

        public void SaveEntries(ObservableCollection<MoodEntry> entries) 
        {
            var json = JsonSerializer.Serialize(entries);
            Preferences.Default.Set(StorageKey, json);
        }
    }
}
