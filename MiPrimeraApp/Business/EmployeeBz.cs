using Incidences.Business;
using Incidences.Data;
using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;

namespace MiPrimeraApp.Business
{
    public class EmployeeBz : IEmployeeBz
    {
        private ICredentialsBz cred;
        private IEmployeeRangeBz range;
        private ISqlBase sql;
        private IBusinessBase bisba;
        public EmployeeBz(IBusinessBase bisba, ISqlBase sqlBase, ICredentialsBz credentials, IEmployeeRangeBz employeeRange)
        {
            this.cred = credentials;
            this.range = employeeRange;
            this.sql = sqlBase;
            this.bisba = bisba;
        }

        #region SELECT
        public IList<Employee> GetEmployeeByUsernameFn(string username)
        {
            try
            {
                Credentials credentials = cred.SelectCredentialsByUsername(username);
                if (credentials.employeeId != null) return SelectEmployeeById((int)credentials.employeeId);
                else return new List<Employee>();
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
                return SelectEmployees(
                    new List<string> { "*" },
                    new CDictionary<string, string> {
                        { "dni", null, dni }
                    }
                )[0];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IList<Employee> SelectEmployeeById(int id)
        {
            try
            {
                return SelectEmployees(
                    new List<string> { "*" },
                    new CDictionary<string, string> {
                        { "id", null, id.ToString() }
                    }
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
                return SelectEmployees(
                    new List<string> { "*" },
                    new CDictionary<string, string> {
                        { "deleted", null, "0" }
                    }
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IList<Employee> SelectEmployees(IList<string> fields, CDictionary<string, string> conditions = null)
        {
            try
            {
                bool result = this.sql.Select(new Select("completeEmployee", fields, conditions));
                if (result)
                {
                    IList<Employee> employees = new List<Employee>();
                    using IDataReader reader = this.sql.get_sql.ExecuteReader();
                    while (reader.Read())
                    {
                        employees.Add(
                            new Employee(
                                (string)reader.GetValue(1),
                                (string)reader.GetValue(2),
                                (string)reader.GetValue(3),
                                reader.GetValue(4) != DBNull.Value ? (string)reader.GetValue(4) : null,
                                new TypeRange(
                                    (int)reader.GetValue(7),
                                    (string)reader.GetValue(8)
                                ),
                                (int)reader.GetValue(0),
                                (int)reader.GetValue(9)
                            )
                        );
                    }
                    return employees;
                }
                else throw new Exception("Ningún registro");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region UPDATE
        public bool UpdateWorker(IList<string> fields, IList<string> values, string dni)
        {
            try
            {
                Employee oldUser = SelectEmployeeByDni(dni);
                if (oldUser == null)
                {
                    return false;
                }
                else
                {
                    int position = 0;
                    CDictionary<string, string> columns = new CDictionary<string, string>();
                    foreach (string field in fields)
                    {
                        if (CheckField("employee", field)) columns.Add(field, null, values[position]);
                        position++;
                    }
                    if (columns.Count > 0)
                    {
                        return this.sql.Update("employee", columns, new CDictionary<string, string> { { "dni", null, dni } });
                    }
                    else return false;
                }
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
                int? rangeId = employee.type != null ? this.range.GetEmployeeRangeIdByName(employee.type) : null;

                bool result = this.sql.Update(
                    "employee",
                    GetUserColumns(employee),
                    new CDictionary<string, string> {
                        { "id", null, id.ToString() }
                    }
                );
                if (!result) throw new Exception("Empleado no actualizado");
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region INSERT
        public bool AddEmployeeFn(EmployeeDto employee)
        {
            try
            {
                bool result = cred.CheckCredentialsFn(employee.credentials.username);
                if (result)
                {
                    Credentials credentials = cred.SelectCredentialsByUsername(employee.credentials.username);
                    return UpdateEmployee(employee, credentials.employeeId);
                }
                else return InsertEmployee(employee);
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
                int rangeId = this.range.GetEmployeeRangeIdByName(employee.type);
                bool result = this.sql.Insert("employee", GetUserColumns(employee));
                if (!result) throw new Exception("Empleado no insertado");
                IList<Employee> employees = SelectEmployees(new List<string> { "*" }, new CDictionary<string, string> { { "dni", null, employee.dni } });
                Employee user = employees[0];

                CDictionary<string, string> columns = new();
                if (employee.credentials.username != null) columns.Add("username", null, $"'{ employee.credentials.username }'");
                if (employee.credentials.password != null) columns.Add("password", null, $"'{ this.bisba.GetMD5(employee.credentials.password) }'");
                if (user.id != null) columns.Add("employeeId", null, user.id.ToString());
                result = this.sql.Insert("credentials", columns);
                if (!result) throw new Exception("Empleado no insertado");
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region DELETE
        public bool UpdateEmployeeFn(int id)
        {
            EmployeeDto employee = new();
            employee.deleted = true;
            return this.sql.Update("employee",
                GetUserColumns(employee),
                new CDictionary<string, string> {
                    { "id", null, id.ToString() }
                }
            );
        }
        #endregion

        #region OTHER
        public CDictionary<string, string> GetUserColumns(EmployeeDto employee)
        {
            CDictionary<string, string> tmpColumns = new();
            if (employee.dni != null) tmpColumns.Add("dni", null, $"{ employee.dni }");
            if (employee.name != null) tmpColumns.Add("name", null, $"{ employee.name }");
            if (employee.surname1 != null) tmpColumns.Add("surname1", null, $"{ employee.surname1 }");
            if (employee.surname2 != null) tmpColumns.Add("surname2", null, $"{ employee.surname2 }");
            if (employee.type != null) tmpColumns.Add("typeId", null, employee.typeId.ToString());
            if (employee.deleted != null) tmpColumns.Add("state", null, Convert.ToInt16(employee.deleted).ToString());
            return tmpColumns;
        }
        public bool CheckField(string table, string field)
        {
            IList<string> fields;
            switch (table)
            {
                case "employee":
                    fields = new List<string> { "name", "surname1", "surname2", "typeId", "deleted" };
                    return fields.Contains(field);
                default:
                    return false;
            }
        }
        #endregion
    }
}
