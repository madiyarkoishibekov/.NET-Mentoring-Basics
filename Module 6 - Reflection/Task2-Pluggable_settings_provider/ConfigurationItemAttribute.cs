using System;
using System.Collections.Generic;
using System.Text;

namespace Task2_Pluggable_settings_provider
{
    [AttributeUsage(AttributeTargets.Property)]
    class ConfigurationItemAttribute : Attribute
    {
        private ProviderType _providerType;

        public ConfigurationItemAttribute(ProviderType providerType)
        {
            ProviderType = providerType;
        }

        public ProviderType ProviderType { get; set; }
    }


    public enum ProviderType
    {
        FileConfiguration,
        ConfigurationManagerConfiguration
    }
}
