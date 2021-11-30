using Incidences.Data.Models;
using Incidences.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Data
{
    public class EmployeeData : IEmployeeData
    {
        #region constants
        //tables
        private const string completeEmployee = "completeEmployee";
        private const string employee = "employee";

        //columns
        private const string ALL = "*";
        private const string employeeC = "employee";
        private const string dni = "dni";
        private const string name = "name";
        private const string surname1 = "surname1";
        private const string surname2 = "surname2";
        private const string typeId = "typeId";
        private const string state = "state";
        private const string deleted = "deleted";

        #endregion

        private readonly ISqlBase sql;
        private readonly ICredentialsData credentialsData;

        public EmployeeData(ISqlBase sql, ICredentialsData credentialsData)
        {
            this.sql = sql;
            this.credentialsData = credentialsData;
        }

        public Employee SelectEmployeeByDni(string dni)
        {
            try
            {
                return SelectEmployees(
                    new List<string> { ALL },
                    sql.WhereDni(dni)
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
                    new List<string> { ALL },
                    sql.WhereId(id)
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
                    new List<string> { ALL },
                    sql.WhereDeleted(0)
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
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
                    CDictionary<string, string> columns = new();
                    foreach (string field in fields)
                    {
                        if (CheckField(employeeC, field)) columns.Add(field, null, values[position]);
                        position++;
                    }
                    if (columns.Count > 0)
                    {
                        bool result = sql.Update(
                            employeeC, 
                            columns,
                            sql.WhereDni(dni)
                        );
                        sql.Close();
                        return result;
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
                bool result = sql.Update(
                    employeeC,
                    GetUserColumns(employee),
                    sql.WhereId(id)
                );
                sql.Close();
                if (!result) throw new Exception("Empleado no actualizado");
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
                bool result = sql.Insert(employeeC, GetUserColumns(employee));
                if (!result) throw new Exception("Empleado no insertado");
                IList<Employee> employees = SelectEmployees(
                    new List<string> { ALL }, 
                    sql.WhereDni(employee.dni)
                );
                Employee user = employees[0];

                return credentialsData.InsertCredentials(employee.credentials, user.Id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdateEmployeeFn(int id)
        {
            EmployeeDto employee = new();
            employee.deleted = true;
            return sql.Update(employeeC,
                GetUserColumns(employee),
                sql.WhereId(id)
            );
        }
        private IList<Employee> SelectEmployees(IList<string> fields, CDictionary<string, string> conditions = null)
        {
            try
            {
                bool result = sql.Select(new Select(completeEmployee, fields, conditions));
                if (result)
                {
                    IList<Employee> employees = new List<Employee>();
                    using IDataReader reader = sql.GetReader();
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
                    sql.Close();
                    return employees;
                }
                else throw new Exception("Ningún registro");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region OTHER
        private CDictionary<string, string> GetUserColumns(EmployeeDto employee)
        {
            CDictionary<string, string> tmpColumns = new();
            if (employee.dni != null) tmpColumns.Add(dni, null, $"{ employee.dni }");
            if (employee.name != null) tmpColumns.Add(name, null, $"{ employee.name }");
            if (employee.surname1 != null) tmpColumns.Add(surname1, null, $"{ employee.surname1 }");
            if (employee.surname2 != null) tmpColumns.Add(surname2, null, $"{ employee.surname2 }");
            if (employee.type != null) tmpColumns.Add(typeId, null, employee.typeId.ToString());
            if (employee.deleted != null) tmpColumns.Add(state, null, Convert.ToInt16(employee.deleted).ToString());
            return tmpColumns;
        }
        private bool CheckField(string table, string field)
        {
            IList<string> fields;
            switch (table)
            {
                case employee:
                    fields = new List<string> { name, surname1, surname2, typeId, deleted };
                    return fields.Contains(field);
                default:
                    return false;
            }
        }
        #endregion
    }
}
