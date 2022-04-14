using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Task2_Pluggable_settings_provider
{
    class ConfigurationComponentBase
    {
        protected List<ISettingsProvider> providersList = new List<ISettingsProvider>();
        public ConfigurationComponentBase()
        {
            LoadProviders();
        }

        public void PrintProperties()
        {
            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                Console.WriteLine($"Property name: {property.Name}\nProperty value: {property.GetValue(this)}\n");
            }
        }

        /// <summary>
        /// Loads settings from settings provider into properties of class for all properties.
        /// </summary>
        public void LoadProperties()
        {
            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyAttributes = property.GetCustomAttributes(typeof(ConfigurationItemAttribute), false);
                if (propertyAttributes.Length > 0)
                {
                    var providerTypeName = ((ConfigurationItemAttribute)propertyAttributes[0]).ProviderType.ToString();
                    var provider = providersList.FirstOrDefault(p => p.ProviderName == providerTypeName) as ISettingsProvider;
                    if (provider == null)
                    {
                        throw new InvalidOperationException($"No plugin {providerTypeName} found");
                    }

                    var value = provider.Load(property.Name);

                    if (property.PropertyType == typeof(Int32) &&
                        Int32.TryParse(value, out int resultInt))
                    {
                        property.SetValue(this, resultInt);
                    }
                    else
                    if (property.PropertyType == typeof(float) &&
                        float.TryParse(value, out float resultFloat))
                    {
                        property.SetValue(this, resultFloat);
                    }
                    else
                    if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(this, value);
                    }
                    else
                    if (property.PropertyType == typeof(TimeSpan) &&
                        TimeSpan.TryParse(value, out TimeSpan resultTimeSpan))
                    {
                        property.SetValue(this, resultTimeSpan);
                    }
                }
            }
        }

        /// <summary>
        /// Saves all values from class' properties to settings provider.
        /// </summary>
        public void SaveProperties()
        {
            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyAttributes = property.GetCustomAttributes(typeof(ConfigurationItemAttribute), false);
                if (propertyAttributes.Length > 0)
                {
                    var providerTypeName = ((ConfigurationItemAttribute)propertyAttributes[0]).ProviderType.ToString();
                    var provider = providersList.FirstOrDefault(p => p.ProviderName == providerTypeName) as ISettingsProvider;
                    if (provider == null)
                    {
                        throw new InvalidOperationException($"No plugin {providerTypeName} found");
                    }

                    var value = property.GetValue(this).ToString();
                    provider.Save(property.Name, value);
                }
            }
        }

        private void LoadProviders()
        {
            foreach (var file in Directory.GetFiles(@".\Plugins", "*.dll"))
            {
                var pluginAssembly = Assembly.LoadFrom(Directory.GetCurrentDirectory() + file);
                foreach (var type in pluginAssembly.GetTypes())
                {
                    if (type.GetInterfaces().Contains(typeof(ISettingsProvider)))
                    {
                        var provider = Activator.CreateInstance(type) as ISettingsProvider;
                        providersList.Add(provider);
                    }
                }
            }
        }
    }
}
