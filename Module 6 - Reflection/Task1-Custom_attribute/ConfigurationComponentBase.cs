using System;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Collections.Specialized;

namespace Task1_Custom_attribute
{
    class ConfigurationComponentBase
    {
        protected IConfiguration configurationFromFile;
        protected Configuration configurationFromConfigurationManager;
        public ConfigurationComponentBase()
        {
            // Initialize Configuration from custom file.
            configurationFromFile = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Initialize configuration with ConfigurationManager
            configurationFromConfigurationManager = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
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
                    var providerType = ((ConfigurationItemAttribute)propertyAttributes[0]).ProviderType;
                    string value = string.Empty;
                    switch (providerType)
                    {
                        case ProviderType.FileConfiguration:
                            value = configurationFromFile[property.Name];
                            break;
                        case ProviderType.ConfigurationManagerConfiguration:
                            value = System.Configuration.ConfigurationManager.AppSettings.Get(property.Name);
                            break;
                    }

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
                    var value = property.GetValue(this).ToString();

                    var providerType = ((ConfigurationItemAttribute)propertyAttributes[0]).ProviderType;
                    switch (providerType)
                    {
                        case ProviderType.FileConfiguration:
                            configurationFromFile[property.Name] = value;
                            break;
                        case ProviderType.ConfigurationManagerConfiguration:
                            System.Configuration.ConfigurationManager.AppSettings[property.Name] = value;
                            break;
                    }
                }
                configurationFromConfigurationManager.Save(ConfigurationSaveMode.Modified); // this line does not work
            }
        }


    }
}
