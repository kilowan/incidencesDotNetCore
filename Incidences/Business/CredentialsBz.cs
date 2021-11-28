using Incidences.Data;
using Incidences.Data.Models;
using Incidences.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Business
{
    public class CredentialsBz : ICredentialsBz
    {
        private IBusinessBase bisba;
        private ISqlBase sql;
        public CredentialsBz(IBusinessBase businessBase, ISqlBase sqlBase)
        {
            this.bisba = businessBase;
            this.sql = sqlBase;
        }
        #region SELECT
        public Credentials SelectCredentialsByUsername(string username)
        {
            try
            {
                return SelectCredentials(
                    this.bisba.WhereUsername(
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
        public Credentials SelectCredentialsById(int id)
        {
            try
            {
                return SelectCredentials(this.bisba.WhereEmployee(new CDictionary<string, string>(), id));
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
                bool result = this.sql.Select(new Select("credentials", new List<string> { "*" }, conditions));
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
        #endregion

        #region UPDATE
        public bool UpdatePassword(string password, int employeeId)
        {
            try
            {
                return this.sql.Update("credentials", GetCredentialsColumns(null, password), new CDictionary<string, string> { { "employee", null, employeeId.ToString() } });
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
                return this.sql.Update("credentials", GetCredentialsColumns(username), new CDictionary<string, string> { { "employee", null, employeeId.ToString() } });
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
                return this.sql.Update(
                    "credentials",
                    GetCredentialsColumns(
                        credentials.username,
                        credentials.password),
                    new CDictionary<string, string> {
                        { "employee", null, employeeId.ToString() }
                    }
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region OTHER
        private CDictionary<string, string> GetCredentialsColumns(string username = null, string password = null, int? employee = null)
        {
            CDictionary<string, string> tmpColumns = new CDictionary<string, string>();
            if (username != null) tmpColumns.Add("username", null, username);
            if (password != null) tmpColumns.Add("password", null, password);
            if (employee != null) tmpColumns.Add("employee", null, employee.ToString());
            return tmpColumns;
        }
        public bool CheckCredentialsFn(string username, string password)
        {
            try
            {
                return CheckCredentials(
                    this.bisba.WherePassword(
                        this.bisba.WhereUsername(
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
        public bool CheckCredentialsFn(string username)
        {
            try
            {
                return CheckCredentials(
                    this.bisba.WhereUsername(
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
            return this.sql.Select(
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