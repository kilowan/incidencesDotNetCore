using Incidences.Business;
using Microsoft.AspNetCore.Mvc;
using Incidences.Models.Employee;
using System.Collections.Generic;

namespace Incidences.Controllers
{
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeBz emp;
        public EmployeeController(IEmployeeBz employee)
        {
            this.emp = employee;
        }

        [HttpGet]
        public IList<Employee> Index()
        {
            return emp.SelectActiveEmployee();
        }

        [HttpGet("{username}")]
        public Employee Details(string username)
        {
            return emp.GetEmployeeByUsername(username);
        }

        [HttpPost]
        public bool Add(EmployeeDto employee)
        {
            return emp.AddEmployee(employee);
        }
        [HttpPut("{id}")]
        public bool Update(EmployeeDto employee, int id)
        {
            return emp.UpdateEmployee(employee, id);
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return emp.UpdateEmployee(id);
        }
    }
}
