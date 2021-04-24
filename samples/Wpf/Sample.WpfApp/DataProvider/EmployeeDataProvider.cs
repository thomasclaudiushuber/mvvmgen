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
        new Employee{FirstName="Julia",LastName="Developer"},
        new Employee{FirstName="Anna",LastName="Programmer"},
        new Employee{FirstName="Thomas",LastName="Huber"},
      };
    }
  }
}
