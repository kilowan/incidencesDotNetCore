using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;

namespace MiPrimeraApp.Business
{
    public class CredentialsBz : BusinessBase
    {
        #region SELECT
        public Credentials SelectCredentialsByUsername(string username)
        {
            try
            {
                return SelectCredentials(WhereUsername(new CDictionary<string, string>(), username));
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public Credentials SelectCredentialsById(int id)
        {
            try
            {
                return SelectCredentials(WhereEmployee(new CDictionary<string, string>(), id));
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        private Credentials SelectCredentials(CDictionary<string, string> conditions)
        {
            try
            {
                bool result = Select(new Select("credentials", new List<string> { "*" }, conditions));
                if (result)
                {
                    using IDataReader reader = this.get_sql.ExecuteReader();
                    reader.Read();
                    return new Credentials((string)reader.GetValue(1), (string)reader.GetValue(2), (int)reader.GetValue(3));
                } else throw new Exception("Ningún registro");
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region UPDATE
        public bool UpdatePassword(string password, int employeeId)
        {
            try
            {
                return Update("credentials", GetCredentialsColumns(null, password), new CDictionary<string, string> { { "employee", null, employeeId.ToString() } });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdateUsername(string username, int employeeId)
        {
            try
            {
                return Update("credentials", GetCredentialsColumns(username), new CDictionary<string, string> { { "employee", null, employeeId.ToString() } });
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool UpdateCredentials(string username, string password, int? employeeId, IDbCommand conexion = null)
        {
            try
            {
                return Update("credentials", GetCredentialsColumns(username, password), new CDictionary<string, string> { { "employee", null, employeeId.ToString() } });
            } catch (Exception e) {
                    throw new Exception(e.Message);
            }
        }
        #endregion

        #region OTHER
        public CDictionary<string, string> GetCredentialsColumns(string username = null, string password = null, int? employee = null)
        {
            CDictionary<string, string> tmpColumns = new CDictionary<string, string>();
            if(username != null) tmpColumns.Add("username", null, username);
            if (password != null) tmpColumns.Add("password", null, password);
            if (employee != null) tmpColumns.Add("employee", null, employee.ToString());
            return tmpColumns;
        }
        public bool CheckCredentialsFn(string username, string password)
        {
            try
            {
                return CheckCredentials(
                    WherePassword(
                        WhereUsername(
                            new CDictionary<string, string>(), 
                            username
                        ), 
                        password
                    )
                );
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool CheckCredentialsFn(string username)
        {
            try
            {
                return CheckCredentials(
                    WhereUsername(
                        new CDictionary<string, string>(), 
                        username
                    )
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private bool CheckCredentials(CDictionary<string, string> conditions)
        {
            return Select(
                new Select(
                    "credentialsmatch",
                    new List<string> { "*" },
                    conditions
                )
            );
        }
        #endregion
    }
}