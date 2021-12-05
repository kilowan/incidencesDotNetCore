using Incidences.Data.Models;
using Incidences.Models.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Incidences.Data
{
    public class EmployeeData : IEmployeeData
    {
        private readonly ICredentialsData credentialsData;
        private readonly IncidenceContext _context;

        public EmployeeData(ICredentialsData credentialsData, IncidenceContext context)
        {
            this._context = context;
            this.credentialsData = credentialsData;
        }

        public Employee SelectEmployeeByDni(string dni)
        {
            try
            {
                employee emp = _context.Employees
                    .Include(em => em.EmployeeRange)
                    .Where(em => em.dni == dni)
                    .FirstOrDefault();

                return new Employee(
                    emp.dni,
                    emp.name,
                    emp.surname1,
                    emp.surname2,
                    new TypeRange(emp.EmployeeRange.id, emp.EmployeeRange.name),
                    emp.id, emp.state
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Employee SelectEmployeeById(int id)
        {
            try
            {
                employee emp = _context.Employees
                    .Include(em => em.EmployeeRange)
                    .Where(em => em.id == id)
                    .FirstOrDefault();
                return new Employee(
                    emp.dni, 
                    emp.name, 
                    emp.surname1, 
                    emp.surname2,
                    new TypeRange(emp.EmployeeRange.id, emp.EmployeeRange.name),
                    emp.id, emp.state
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IList<Employee> SelectActiveEmployee()
        {
            try
            {
                IList<employee> emps = _context.Employees
                    .Where(em => em.state != 1)
                    .ToList();
                IList<Employee> result = new List<Employee>(); 
                foreach (employee employee in emps)
                {
                    employee_range emra = _context.EmployeeRanges
                        .Find(employee.typeId);
                    result.Add(
                        new Employee(
                            employee.dni, 
                            employee.name, 
                            employee.surname1, 
                            employee.surname2,
                            new TypeRange(emra),
                            employee.id, employee.state)
                        );
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdateEmployee(EmployeeDto employee, int? id)
        {
            try
            {
                bool result = false;
                employee emp = _context.Employees
                    .Where(em => em.id == id)
                    .FirstOrDefault();
                if (emp != null)
                {
                    if(employee.name != null) emp.name = employee.name;
                    if (employee.surname1 != null) emp.surname1 = employee.surname1;
                    if (employee.surname2 != null) emp.surname2 = employee.surname2;
                    if (employee.typeId != null) emp.typeId = (int)employee.typeId;
                    _context.Update(emp);
                    if (_context.SaveChanges() != 1) throw new Exception("Empleado no actualizado");
                    result = true;
                } else throw new Exception("Empleado no actualizado");

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool InsertEmployee(EmployeeDto employee)
        {
            try
            {
                int? old = _context.Employees
                    .Select(incidence => incidence.id)
                    .Max();
                if (old == null) old = 1;
                else old += 1;
                employee emp = new()
                {
                    dni = employee.dni,
                    name = employee.name,
                    surname1 = employee.surname1,
                    surname2 = employee.surname2,
                    state = 0,
                    typeId = (int)employee.typeId,
                    id = (int)old
                };
                _context.Employees.Add(emp);
                if (_context.SaveChanges() != 1) throw new Exception("Empleado no insertado");
                bool result = credentialsData.InsertCredentials(
                    new CredentialsDto()
                    { 
                        username = employee.credentials.username,
                        password = employee.credentials.password,
                    },
                    emp.id
                );

                if (result) throw new Exception("Empleado no insertado");

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdateEmployee(int id)
        {
            bool result = false;
            employee emp = _context.Employees
                .Where(em => em.id == id)
                .FirstOrDefault();
            if (emp != null)
            {
                 emp.state = 1;
                _context.Update(emp);
                result = true;
            }
            else throw new Exception("Empleado no actualizado");

            return result;
        }
    }
}
