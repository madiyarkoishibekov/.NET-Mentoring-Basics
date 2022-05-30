using System.Runtime.Serialization.Formatters.Binary;

namespace DeepCloning
{
    [Serializable]
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public Department(int id, string name)
        {
            DepartmentId = id;
            DepartmentName = name;
        }
    }
    [Serializable]
    public class Employee : ICloneable
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public Department Department { get; set; }

        public Employee(int id, string name, int departmentId, string departmentName)
        {
            EmployeeId = id;
            EmployeeName = name;
            Department = new Department(departmentId, departmentName);
        }
        public object Clone()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if (this.GetType().IsSerializable)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                    stream.Position = 0;
                    return formatter.Deserialize(stream);
                }
                return null;
            }
        }

        public override string ToString()
        {
            return $"Employee {EmployeeName} with {EmployeeId} works in {Department.DepartmentName}.";
        }
    }
}
