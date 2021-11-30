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
                return SelectCredentials(this.sql.WhereEmployee(new CDictionary<string, string>(), id));
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
                return SelectCredentials(
                    this.sql.WhereUsername(
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
        public bool UpdatePassword(string password, int employeeId)
        {
            try
            {
                return this.sql.Update(credentialsC, GetCredentialsColumns(null, password), new CDictionary<string, string> { { employeeC, null, employeeId.ToString() } });
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
                return CheckCredentials(
                    this.sql.WhereUsername(
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
        public bool CheckCredentialsFn(string username, string password)
        {
            try
            {
                return CheckCredentials(
                    this.sql.WherePassword(
                        this.sql.WhereUsername(
                            new CDictionary<string, string>(),
                            username
                        ),
                        password
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
        public bool UpdateUsername(string username, int employeeId)
        {
            try
            {
                return this.sql.Update(credentialsC, GetCredentialsColumns(username), new CDictionary<string, string> { { employeeC, null, employeeId.ToString() } });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private Credentials SelectCredentials(CDictionary<string, string> conditions)
        {
            try
            {
                bool result = this.sql.Select(new Select(credentialsC, new List<string> { ALL }, conditions));
                if (result)
                {
                    using IDataReader reader = this.sql.GetReader();
                    reader.Read();
                    Credentials cred = new(
                        (string)reader.GetValue(1),
                        (string)reader.GetValue(2),
                        (int)reader.GetValue(3)
                    );
                    this.sql.Close();
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
            bool result = this.sql.Select(
                new Select(
                    credentialsmatch,
                    new List<string> { ALL },
                    conditions
                )
            );

            this.sql.Close();
            return result;
        }


    }
}
