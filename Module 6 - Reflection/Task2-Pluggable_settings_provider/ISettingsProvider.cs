using System;
using System.Collections.Generic;
using System.Text;

namespace Task2_Pluggable_settings_provider
{
    public interface ISettingsProvider
    {
        string ProviderName { get; set; }
        string Load(string propertyName);
        void Save(string propertyName, string value);
    }
}
