using BinarySerialization;
using System.Runtime.Serialization.Formatters.Binary;

Employee employee = new Employee("Madiyar", ".NET development");
Stream stream = File.Open("Employee.dat", FileMode.Create);
BinaryFormatter binaryFormatter = new BinaryFormatter();
binaryFormatter.Serialize(stream, employee);
employee = null;
stream.Close();

stream = File.Open("Employee.dat", FileMode.Open);
binaryFormatter = new BinaryFormatter();
employee = (Employee)binaryFormatter.Deserialize(stream);
stream.Close();
Console.WriteLine(employee.ToString());
