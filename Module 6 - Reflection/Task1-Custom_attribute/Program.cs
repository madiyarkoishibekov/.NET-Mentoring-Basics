using System;

namespace Task1_Custom_attribute
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load initial values, print them
            var configurationTest = new ConfigurationTest();
            configurationTest.LoadProperties();
            configurationTest.PrintProperties();

            // Change values and save them
            configurationTest.IntProperty += 20;
            configurationTest.FloatProperty += 20.5F;
            configurationTest.StringProperty += "small_change";
            configurationTest.TimeSpanProperty += new TimeSpan(3,0,0);
            configurationTest.SaveProperties();

            // Load again, print them
            configurationTest.LoadProperties();
            configurationTest.PrintProperties();
        }
    }
}
