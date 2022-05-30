using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerialization
{
    [Serializable]
    public class Animal : ISerializable
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int AnimaID { get; set; }

        public Animal() { }

        public Animal(string name = "No name",
            double weight = 0,
            double height = 0)
        {
            Name = name;
            Weight = weight;
            Height = height;
        }

        public override string ToString()
        {
            return $"{Name} weighs {Weight} lbs and is {Height} inches tall";
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("weight", Weight);
            info.AddValue("height", Height);
        }

        public Animal(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("name", typeof(string));
            Weight = (double)info.GetValue("weight", typeof(double));
            Height = (double)info.GetValue("height", typeof(double));
        }
    }
}
