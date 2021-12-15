using Incidences.Data.Models;
using Incidences.Models.Employee;

namespace Incidences.Data
{
    public interface ICredentialsData
    {
        public Incidences.Models.Employee.Credentials SelectCredentialsById(int id);
        public Incidences.Models.Employee.Credentials SelectCredentialsByUsername(string username);
        public bool CheckCredentials(string username);
        public employee CheckCredentials(string username, string password);
        public bool UpdateCredentials(CredentialsDto credentials, int employeeId);
        public bool UpdateUsername(string username, int employeeId);
        public bool UpdatePassword(string password, int employeeId);
        public bool InsertCredentials(CredentialsDto credentials, int? employeeId);
    }
}
