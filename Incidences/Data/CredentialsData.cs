using Incidences.Data.Models;
using Incidences.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Data
{
    public class CredentialsData : ICredentialsData
    {
        #region constants
        //tables
        private const string credentialsC = "credentials";
        private const string credentialsmatch = "credentialsmatch";

        //columns
        private const string ALL = "*";
        private const string employeeC = "employee";
        private const string usernameC = "username";
        private const string passwordC = "password";
        private const string employeeIdC = "employeeId";
        #endregion

        private readonly ISqlBase sql;

        public CredentialsData(ISqlBase sql)
        {
            this.sql = sql;
        }

        public Credentials SelectCredentialsById(int id)
        {
            try
            {
                return SelectCredentials(sql.WhereEmployee(id));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Credentials SelectCredentialsByUsername(string username)
        {
            try
            {
                return SelectCredentials(sql.WhereUsername(username));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdatePassword(string password, int employeeId)
        {
            try
            {
                return sql.Update(
                    credentialsC, 
                    GetCredentialsColumns(
                        null, 
                        password
                    ), 
                    new CDictionary<string, string> { 
                        { employeeC, null, employeeId.ToString() } 
                    }
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool CheckCredentialsFn(string username)
        {
            try
            {
                return CheckCredentials(sql.WhereUsername(username));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool CheckCredentialsFn(string username, string password)
        {
            try
            {
                return CheckCredentials(
                    sql.WherePassword(
                        password, 
                        sql.WhereUsername(username)
                    )
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdateCredentials(CredentialsDto credentials, int employeeId)
        {
            try
            {
                return sql.Update(
                    credentialsC,
                    GetCredentialsColumns(
                        credentials.username,
                        credentials.password),
                    sql.WhereEmployeeId(employeeId)
                );
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
                return sql.Update(
                    credentialsC, 
                    GetCredentialsColumns(username), 
                    new CDictionary<string, string> { 
                        { employeeC, null, employeeId.ToString() } 
                    }
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool InsertCredentials(CredentialsDto credentials, int? employeeId) 
        {
            bool result = sql.Insert(
                credentialsC, 
                new CDictionary<string, string>
                {
                    { usernameC, null, $"'{ credentials.username }'" },
                    { passwordC, null, $"'{ sql.GetMD5(credentials.password) }'" },
                    { employeeIdC, null, employeeId.ToString() }
                }
            );
            sql.Close();
            if (!result) throw new Exception("Empleado no insertado");
            return result;
        }

        private Credentials SelectCredentials(CDictionary<string, string> conditions)
        {
            try
            {
                bool result = sql.Select(
                    new Select(
                        credentialsC, new List<string> { ALL }, 
                        conditions
                    )
                );

                if (result)
                {
                    using IDataReader reader = sql.GetReader();
                    reader.Read();
                    Credentials cred = new(
                        (string)reader.GetValue(1),
                        (string)reader.GetValue(2),
                        (int)reader.GetValue(3)
                    );
                    sql.Close();
                    return cred;
                }
                else throw new Exception("Ningún registro");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private static CDictionary<string, string> GetCredentialsColumns(string username = null, string password = null, int? employee = null)
        {
            CDictionary<string, string> tmpColumns = new CDictionary<string, string>();
            if (username != null) tmpColumns.Add(usernameC, null, username);
            if (password != null) tmpColumns.Add(passwordC, null, password);
            if (employee != null) tmpColumns.Add(employeeC, null, employee.ToString());
            return tmpColumns;
        }
        private bool CheckCredentials(CDictionary<string, string> conditions)
        {
            bool result = sql.Select(
                new Select(
                    credentialsmatch,
                    new List<string> { ALL },
                    conditions
                )
            );

            sql.Close();
            return result;
        }
    }
}
