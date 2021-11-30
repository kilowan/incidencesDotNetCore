using Incidences.Data;
using Incidences.Models.Employee;
using System;

namespace Incidences.Business
{
    public class CredentialsBz : ICredentialsBz
    {
        private readonly ICredentialsData credentialsData;
        public CredentialsBz(ICredentialsData credentialsData)
        {
            this.credentialsData = credentialsData;
        }

        #region SELECT
        public Credentials SelectCredentialsByUsername(string username)
        {
            try
            {
                return credentialsData.SelectCredentialsByUsername(username);
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
                return credentialsData.SelectCredentialsById(id);
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
                return credentialsData.UpdatePassword(password, employeeId);
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
                return credentialsData.UpdateUsername(username, employeeId);
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
                return credentialsData.UpdateCredentials(credentials, employeeId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region OTHER
        public bool CheckCredentialsFn(string username, string password)
        {
            try
            {
                return credentialsData.CheckCredentialsFn(username, password);
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
                return credentialsData.CheckCredentialsFn(username);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}