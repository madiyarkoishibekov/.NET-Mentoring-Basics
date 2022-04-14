using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using Task2_Pluggable_settings_provider;

namespace Plugin_ConfigurationManager
{
    public class ConfigurationManagerUtilities : ISettingsProvider
    {
        public ConfigurationManagerUtilities()
        {
            ProviderName = "ConfigurationManagerConfiguration";
        }

        public string ProviderName { get; set; }

        public string Load(string propertyName)
        {
            var value = System.Configuration.ConfigurationManager.AppSettings.Get(propertyName);
            return value;
        }
        public void Save(string propertyName, string value)
        {
            System.Configuration.ConfigurationManager.AppSettings[propertyName] = value;
        }

    }
}
