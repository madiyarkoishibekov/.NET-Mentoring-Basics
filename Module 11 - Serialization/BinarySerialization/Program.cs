using BinarySerialization;
using System.Runtime.Serialization.Formatters.Binary;

Animal bowser = new Animal("Bowser", 45, 25);

Stream stream = File.Open("AnimalData.dat", FileMode.Create);
BinaryFormatter bf = new BinaryFormatter();
bf.Serialize(stream, bowser);

bowser = null;
stream.Close();

stream = File.Open("AnimalData.dat", FileMode.Open);
bf = new BinaryFormatter();
bowser = (Animal)bf.Deserialize(stream);
stream.Close();
Console.WriteLine(bowser.ToString());