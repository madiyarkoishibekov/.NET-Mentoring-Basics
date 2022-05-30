using DeepCloning;

var employee1 = new Employee(1, "Madiyar", 101302, ".NET development Team");

var employee2 = (Employee)employee1.Clone();
employee2.Department = new Department(1000000, ".NET coordinating team");
Console.WriteLine(employee1.ToString());
Console.WriteLine(employee2.ToString());