using JsonSerialization;
using Newtonsoft.Json;

Employee employee = new Employee("Madiyar", ".NET Development.");

Serialize(employee);

employee = null;
employee = Deserialize<Employee>();
Console.WriteLine(employee.ToString());

static void Serialize(Employee employee)
{
    using (StreamWriter sw = new StreamWriter(File.Open($@"{AppDomain.CurrentDomain.BaseDirectory}\employee.json", FileMode.Create)))
    {
        using (JsonTextWriter jtw = new JsonTextWriter(sw))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(jtw, employee);
            jtw.Flush();
        }
    }
}

static T Deserialize<T>()
{
    using (StreamReader sr = new StreamReader(File.Open($@"{AppDomain.CurrentDomain.BaseDirectory}\employee.json", FileMode.Open)))
    {
        using (JsonTextReader jtr = new JsonTextReader(sr))
        {
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize<T>(jtr);
        }
    }
}