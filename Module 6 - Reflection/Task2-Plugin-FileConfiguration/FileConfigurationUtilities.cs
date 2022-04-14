using Microsoft.Extensions.Configuration;
using System;
using Task2_Pluggable_settings_provider;

namespace Task2_Plugin_FileConfiguration
{
    public class FileConfigurationUtilities : ISettingsProvider
    {
        private IConfiguration _configurationFromFile;
        public FileConfigurationUtilities()
        {
            ProviderName = "FileConfiguration";
            _configurationFromFile = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public string ProviderName { get; set; }

        public string Load(string propertyName)
        {
            var value = _configurationFromFile[propertyName];
            return value;
        }

        public void Save(string propertyName, string value)
        {
            _configurationFromFile[propertyName] = value;
        }
    }
}
