using System;
using System.Collections.Generic;
using System.Text;

namespace Task1_Custom_attribute
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
}
