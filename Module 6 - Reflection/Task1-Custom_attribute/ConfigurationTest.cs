using System;
using System.Collections.Generic;
using System.Text;

namespace Task1_Custom_attribute
{
    /// <summary>
    /// Setting values as properties.
    /// </summary>
    class ConfigurationTest : ConfigurationComponentBase
    {
        [ConfigurationItem(ProviderType.FileConfiguration)]
        public int IntProperty { get ; set; }
        [ConfigurationItem(ProviderType.ConfigurationManagerConfiguration)]
        public float FloatProperty { get; set; }
        [ConfigurationItem(ProviderType.FileConfiguration)]
        public string StringProperty { get; set; }
        [ConfigurationItem(ProviderType.ConfigurationManagerConfiguration)]
        public TimeSpan TimeSpanProperty { get; set; }
    }
}
