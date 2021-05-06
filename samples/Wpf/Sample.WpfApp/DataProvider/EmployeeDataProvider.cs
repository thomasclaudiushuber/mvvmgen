// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using Sample.WpfApp.Model;
using System.Collections.Generic;
using System.Linq;

namespace Sample.WpfApp.DataProvider
{
    public interface IEmployeeDataProvider
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int employeeId);
    }

    public class EmployeeDataProvider : IEmployeeDataProvider
    {
        private List<Employee> _list;

        public EmployeeDataProvider()
        {
            _list = new List<Employee>
                  {
                    new Employee{Id= 1, FirstName="Julia",LastName="Developer"},
                    new Employee{Id= 2, FirstName="Anna",LastName="Programmer"},
                    new Employee{Id= 3, FirstName="Thomas Claudius",LastName="Huber"},
                  };
        }

        public IEnumerable<Employee> GetAll()
        {
            return _list.ToArray();
        }

        public Employee GetById(int employeeId)
        {
            return _list.Single(x => x.Id == employeeId);
        }
    }
}
