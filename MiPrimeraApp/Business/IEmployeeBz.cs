using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Business
{
    public interface IEmployeeBz
    {
        #region SELECT
        public IList<Employee> GetEmployeeByUsernameFn(string username);
        public Employee SelectEmployeeByDni(string dni);
        public IList<Employee> SelectEmployeeById(int id);
        public IList<Employee> SelectActiveEmployee();
        #endregion

        #region UPDATE
        public bool UpdateEmployee(EmployeeDto employee, int? id);

        #endregion

        #region INSERT
        public bool AddEmployeeFn(EmployeeDto employee);

        public bool InsertEmployee(EmployeeDto employee);
        #endregion

        #region DELETE
        public bool UpdateEmployeeFn(int id);
        #endregion
    }
}
