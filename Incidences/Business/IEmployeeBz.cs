using Incidences.Models.Employee;
using System.Collections.Generic;

namespace Incidences.Business
{
    public interface IEmployeeBz
    {
        #region SELECT
        public Employee GetEmployeeByUsername(string username);
        public Employee SelectEmployeeByDni(string dni);
        public Employee SelectEmployeeById(int id);
        public IList<Employee> SelectActiveEmployee();
        #endregion

        #region UPDATE
        public bool UpdateEmployee(EmployeeDto employee, int? id);

        #endregion

        #region INSERT
        public bool AddEmployee(EmployeeDto employee);

        #endregion

        #region DELETE
        public bool UpdateEmployee(int id);
        #endregion
    }
}
