using Incidences.Data.Models;
using Incidences.Models.Employee;
using System.Collections.Generic;

namespace Incidences.Data
{
    public interface IEmployeeData
    {
        public IList<Employee> SelectEmployees(IList<string> fields, CDictionary<string, string> conditions = null);
        public Employee SelectEmployeeByDni(string dni);
        public IList<Employee> SelectEmployeeById(int id);
        public IList<Employee> SelectActiveEmployee();
        public bool UpdateEmployee(EmployeeDto employee, int? id);
        public bool InsertEmployee(EmployeeDto employee);
        public bool UpdateEmployeeFn(int id);
    }
}
