using System.IO;
using System.Text.Json;

namespace expense_tracker.Data
{
    public static class SettingsManager
    {
        private static readonly string filePath =
            "settings.json";

        public static AppSettings Load()
        {
            if (!File.Exists(filePath))
            {
                return new AppSettings();
            }

            string json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<AppSettings>(json)
                   ?? new AppSettings();
        }

        public static void Save(AppSettings settings)
        {
            string json = JsonSerializer.Serialize(
                settings,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }
            );

            File.WriteAllText(filePath, json);
        }
    }
}