using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace CallList
{
    internal static class LocalizationSubstitutioner
    {
        private static string _directoryPath = "../../../../LocalizationsFolder";
        private static string _defaultFileName = "default_en";
        private static string _fileFormat = ".json";
        private static DirectoryInfo _directory = new (_directoryPath);
        public static Regex SwitchLanguage(CultureInfo culture)
        {
            string configFile;
            Config config;
            foreach (var file in _directory.GetFiles())
            {
                string locale = culture.TwoLetterISOLanguageName;
                if (locale == file.Name.Substring(0, 2))
                {
                    configFile = File.ReadAllText($"{_directoryPath}/{locale}{_fileFormat}");
                    config = JsonConvert.DeserializeObject<Config>(configFile);
                    return config.Locale.Alphabet;
                }
            }

            configFile = File.ReadAllText($"{_directoryPath}/{_defaultFileName}{_fileFormat}");
            config = JsonConvert.DeserializeObject<Config>(configFile);
            return config.Locale.Alphabet;
        }
    }
}
