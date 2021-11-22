using MiPrimeraApp.Models.Employee;

namespace Incidences.Business
{
    public interface ICredentialsBz
    {
        #region SELECT
        public Credentials SelectCredentialsByUsername(string username);
        public Credentials SelectCredentialsById(int id);
        #endregion

        #region UPDATE
        public bool UpdateCredentials(CredentialsDto credentials, int employeeId);
        #endregion

        #region OTHER
        public bool CheckCredentialsFn(string username, string password);
        public bool CheckCredentialsFn(string username);
        #endregion
    }
}
