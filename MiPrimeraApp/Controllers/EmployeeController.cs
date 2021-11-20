using Microsoft.AspNetCore.Mvc;
using MiPrimeraApp.Business;
using MiPrimeraApp.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
