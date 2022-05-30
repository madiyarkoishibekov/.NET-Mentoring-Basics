using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XmlSerialization
{
    [Serializable]
    public class Employee
    {
        public string Name { get; set; }
        public Department Department { get; set; }
        public Employee() { }
        public Employee(string name, string departmentName)
        {
            Name = name;
            Department = new Department(departmentName);
        }

        public override string ToString()
        {
            return $"Employee {Name} works in {Department.DepartmentName} department.";
        }
    }
    [Serializable]
    public class Department 
    {
        public string DepartmentName { get; set; }
        public List<Employee> Employees { get; set; }
        public Department() { }
        public Department(string name)
        {
            DepartmentName = name;
        }
    }
}
