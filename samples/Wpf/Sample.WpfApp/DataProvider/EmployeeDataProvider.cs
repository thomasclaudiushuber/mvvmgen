using Sample.WpfApp.Model;
using System.Collections.Generic;

namespace Sample.WpfApp.DataProvider
{
  public interface IEmployeeDataProvider
  {
    IEnumerable<Employee> GetAll();
  }

  public class EmployeeDataProvider : IEmployeeDataProvider
  {
    public IEnumerable<Employee> GetAll()
    {
      return new List<Employee>
      {
        new Employee{Id= 1, FirstName="Julia",LastName="Developer"},
        new Employee{Id= 2, FirstName="Anna",LastName="Programmer"},
        new Employee{Id= 3, FirstName="Thomas",LastName="Huber"},
      };
    }
  }
}
