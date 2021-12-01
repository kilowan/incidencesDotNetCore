using Incidences.Data;
using Incidences.Models.Employee;
using System;
using System.Collections.Generic;

namespace Incidences.Business
{
    public class EmployeeBz : IEmployeeBz
    {
        private readonly ICredentialsBz cred;
        private readonly IEmployeeData employeeData;
        public EmployeeBz(IEmployeeData employeeData, ICredentialsBz credentials)
        {
            this.cred = credentials;
            this.employeeData = employeeData;
        }

        #region SELECT
        public Employee GetEmployeeByUsername(string username)
        {
            try
            {
                Credentials credentials = cred.SelectCredentialsByUsername(username);
                if (credentials.EmployeeId != null) return SelectEmployeeById((int)credentials.EmployeeId);
                else return new Employee();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Employee SelectEmployeeByDni(string dni)
        {
            try
            {
                return employeeData.SelectEmployeeByDni(dni);
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
                return employeeData.SelectEmployeeById(id);
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
                return employeeData.SelectActiveEmployee();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region UPDATE
        public bool UpdateEmployee(EmployeeDto employee, int? id)
        {
            try
            {
                return employeeData.UpdateEmployee(employee, id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region INSERT
        public bool AddEmployee(EmployeeDto employee)
        {
            try
            {
                bool result = cred.CheckCredentials(employee.credentials.username);
                if (result)
                {
                    Credentials credentials = cred.SelectCredentialsByUsername(employee.credentials.username);
                    result = UpdateEmployee(employee, credentials.EmployeeId);
                }
                else result = employeeData.InsertEmployee(employee);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region DELETE
        public bool UpdateEmployee(int id)
        {
            return employeeData.UpdateEmployee(id);
        }
        #endregion

    }
}
