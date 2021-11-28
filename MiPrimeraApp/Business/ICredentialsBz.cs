using Incidences.Models.Employee;

namespace Incidences.Business
{
    public interface ICredentialsBz
    {
        public Credentials SelectCredentialsByUsername(string username);
        public Credentials SelectCredentialsById(int id);
        public bool UpdateCredentials(CredentialsDto credentials, int employeeId);
        public bool CheckCredentialsFn(string username, string password);
        public bool CheckCredentialsFn(string username);
    }
}
