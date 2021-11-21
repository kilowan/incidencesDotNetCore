using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace MiPrimeraApp.Business
{
    public class EmployeeBz : BusinessBase
    {
        private CredentialsBz cred;
        private EmployeeRangeBz range;
        public EmployeeBz()
        {
            this.cred = new CredentialsBz();
            this.range = new();
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
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public Employee SelectEmployeeByDni(string dni)
        {
            try
            {
                return SelectEmployees(new List<string> { "*" }, new CDictionary<string, string> { { "dni", null, dni } })[0];
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public IList<Employee> GetEmployee(string username = null)
        {
            if (username == null) {
                return SelectActiveEmployee();
            }
            else
            {
                return GetEmployeeByUsernameFn(username);
            }
        }
        public IList<Employee> SelectEmployeeById(int id)
        {
            try
            {
                return SelectEmployees(new List<string> { "*" }, new CDictionary<string, string> { { "id", null, id.ToString() } });
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public IList<Employee> SelectActiveEmployee()
        {
            try
            {
                return SelectEmployees(new List<string> { "*" }, new CDictionary<string, string> { { "deleted", null, "0" } });
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public IList<Employee> SelectEmployees(IList<string> fields, CDictionary<string, string> conditions = null)
        {
            try
            {
                bool result = Select(new Select("completeEmployee", fields, conditions));
                if (result)
                {
                    IList<Employee> employees = new List<Employee>();
                    using IDataReader reader = this.get_sql.ExecuteReader();
                    while (reader.Read())
                    {
                        employees.Add(
                            new Employee(
                                (string)reader.GetValue(1),
                                (string)reader.GetValue(2),
                                (string)reader.GetValue(3),
                                reader.GetValue(4) != DBNull.Value ? (string)reader.GetValue(4): null,
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
                } else throw new Exception("Ningún registro");
            } catch (Exception e) {
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
                if (oldUser == null) {
                return false;
                } else
                {
                    int position = 0;
                    CDictionary<string, string> columns = new CDictionary<string, string>();
                    foreach (string field in fields)
                    {
                        if (CheckField("Empleados", field)) columns.Add(field, null, values[position]);
                        position++;
                    }
                    if (columns.Count > 0) {
                        return Update("employee", columns, new CDictionary<string, string> { { "dni", null, dni} });
                    }
                    else return false;
                }
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool UpdateEmployee(bool? deleted = null, string dni = null, string name = null, string surname1 = null, string surname2 = null, string typeName = null, int? employeeId = null, string username = null, string password = null)
        {
            try
            {
                int? rangeId = typeName!= null?this.range.GetEmployeeRangeIdByName(typeName): null;

                bool result = Update(
                    "employee", 
                    GetUserColumns(
                        dni, 
                        name, 
                        surname1, 
                        surname2, 
                        rangeId, 
                        deleted
                    ), 
                    new CDictionary<string, string> { 
                        { "id", null, employeeId.ToString() } 
                    }
                );
                if (!result) throw new Exception("Empleado no actualizado");
                return result;
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool UpdateEmployeeFn(bool? deleted = null, string dni = null, string name = null, string surname1 = null, string surname2 = null, string typeName = null, int? employeeId = null, string username = null, string password = null)
        {
            int rangeId = this.range.GetEmployeeRangeIdByName(typeName);
            bool result = Update("employee", GetUserColumns(null, name, surname1, surname2, rangeId, deleted), new CDictionary<string, string> { { "id", null, employeeId.ToString() } });
            if (!result) throw new Exception("Empleado no actualizado");

            if (username != null || password != null)
            {
                result = cred.UpdateCredentials(username, password, (int)employeeId);
            }

            return result;
        }

        #endregion

        #region INSERT
        public bool AddEmployeeFn(string username, string password, string dni, string name, string surname1, string surname2, string type)
        {
            try
            {
                bool result = cred.CheckCredentialsFn(username);
                if (result) 
                {
                    Credentials credentials = cred.SelectCredentialsByUsername(username);
                    return UpdateEmployee(null, dni, name, surname1, surname2, type, (int)credentials.employeeId, username, password);
                }
                else return InsertEmployee(username, password, dni, name, surname1, type, surname2);
                } catch (Exception e) {
            throw new Exception(e.Message);
            }
        }

        public bool InsertEmployee(string username, string password, string dni, string name, string surname1, string typeName, string surname2 = null)
        {
            try
            {
                int rangeId = this.range.GetEmployeeRangeIdByName(typeName);
                bool result = Insert("employee", GetUserColumns(dni, name, surname1, surname2, rangeId));
                if (!result) throw new Exception("Empleado no insertado");
                IList<Employee> employees = SelectEmployees(new List<string> { "*" }, new CDictionary<string, string> { { "dni", null, dni } });
                Employee user = employees[0];

                CDictionary<string, string> columns = new();
                if (username != null) columns.Add("username", null, $"'{ username }'");
                if (password != null) columns.Add("password", null, $"'{ GetMD5(password) }'");
                if (user.id != null) columns.Add("employeeId", null, user.id.ToString());
                result = Insert("credentials", columns);
                if (!result) throw new Exception("Empleado no insertado");
                return result;
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region DELETE
        public bool UpdateEmployeeFn(int id)
        {
            return Update("employee",
                GetUserColumns(null, null, null, null, null, true),
                new CDictionary<string, string> {
                    { "id", null, id.ToString() }
                }
            );
        }
        #endregion

        #region OTHER
        public CDictionary<string, string> GetUserColumns(string dni = null, string name = null, string surname1 = null, string surname2 = null, int? type = null, bool? deleted = null) 
        {
            CDictionary<string, string> tmpColumns = new();
            if (dni != null) tmpColumns.Add("dni", null, $"{ dni }");
            if(name != null) tmpColumns.Add("name", null, $"{ name }");
            if (surname1 != null) tmpColumns.Add("surname1", null, $"{ surname1 }");
            if (surname2 != null) tmpColumns.Add("surname2", null, $"{ surname2 }");
            if (type != null) tmpColumns.Add("typeId", null, type.ToString());
            if(deleted != null) tmpColumns.Add("state", null, Convert.ToInt16(deleted).ToString());
            return tmpColumns;
        }
        public bool CheckField(string table, string field) 
        {
            IList<string>fields;
            switch (table) {
                case "Empleados":
                    fields = new List<string> { "name", "surname1", "surname2", "typeId", "deleted" };
                    return fields.Contains(field);
                default:
                    return false;
            }
        }
        private static string GetMD5(string str)
        {
            MD5 md5 = MD5.Create();
            ASCIIEncoding encoding = new();
            byte[] stream = null;
            StringBuilder sb = new();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        #endregion
    }
}
