﻿using Microsoft.AspNetCore.Mvc;
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
            return emp.SelectActiveEmployee();
        }

        [HttpGet("{username}")]
        public Employee Details(string username)
        {
            return emp.GetEmployeeByUsernameFn(username)[0];
        }

        [HttpPost]
        public bool Add(EmployeeDto employee)
        {
            return emp.AddEmployeeFn(employee);
        }
        [HttpPut("{id}")]
        public bool Update(EmployeeDto employee, int id)
        {
            return emp.UpdateEmployee(employee, id);
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return emp.UpdateEmployeeFn(id);
        }
    }
}
