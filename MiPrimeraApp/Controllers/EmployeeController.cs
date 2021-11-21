using Microsoft.AspNetCore.Mvc;
using MiPrimeraApp.Business;
using MiPrimeraApp.Models.Employee;
using System.Collections.Generic;

namespace MiPrimeraApp.Controllers
{
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        EmployeeBz emp;
        public EmployeeController()
        {
            this.emp = new();
        }
        [HttpGet]
        public IList<Employee> Index()
        {
            return emp.GetEmployee();
        }
        [HttpGet("{username}")]
        public Employee Details(string username)
        {
            return emp.GetEmployeeByUsernameFn(username)[0];
        }
        [HttpPost]
        public bool Add(EmployeeDto employee)
        {
            return emp.AddEmployeeFn(
                employee.username,
                employee.password,
                employee.dni,
                employee.name,
                employee.surname1,
                employee.surname2,
                employee.type
            );
        }
    }
}
