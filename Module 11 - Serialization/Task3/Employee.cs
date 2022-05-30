using System.Runtime.Serialization;

namespace JsonSerialization
{
    public class Employee
    {
        public string Name { get; set; }
        public Department Department { get; set; }
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
    public class Department 
    {
        public string DepartmentName { get; set; }
        public List<Employee> Employees { get; set; }
        public Department(string name)
        {
            DepartmentName = name;
        }
    }
}
