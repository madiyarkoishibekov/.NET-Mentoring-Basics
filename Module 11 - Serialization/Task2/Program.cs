using System.Xml.Serialization;
using XmlSerialization;

Employee employee = new Employee("Madiyar", ".NET development");
XmlSerializer xmlSerializer = new XmlSerializer(typeof(Employee));
using (TextWriter tw = new StreamWriter($@"{System.AppDomain.CurrentDomain.BaseDirectory}\employee.xml"))
{
    xmlSerializer.Serialize(tw, employee);
}

employee = null;
XmlSerializer deserializer = new XmlSerializer(typeof(Employee));
TextReader reader = new StreamReader($@"{System.AppDomain.CurrentDomain.BaseDirectory}\employee.xml");
employee = (Employee)deserializer.Deserialize(reader);
reader.Close();
Console.WriteLine(employee.ToString());