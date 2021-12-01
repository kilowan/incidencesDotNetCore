using Incidences.Business;
using Microsoft.AspNetCore.Mvc;
using Incidences.Models.Employee;

namespace Incidences.Controllers
{
    [Route("api/Credentials")]
    [ApiController]
    public class CredentialsController : ControllerBase
    {
        private readonly ICredentialsBz cred;
        public CredentialsController(ICredentialsBz credentials)
        {
            this.cred = credentials;
        }

        [HttpGet("{username}/{password}")]
        public bool Details(string username, string password)
        {
            return this.cred.CheckCredentials(username, password);
        }

        [HttpGet("{username}")]
        public Credentials Get(string username)
        {
            return this.cred.SelectCredentialsByUsername(username);
        }

        [HttpPut("{employeeId}")]
        public bool Update(CredentialsDto credentials, int employeeId)
        {
            return this.cred.UpdateCredentials(credentials, employeeId);
        }
    }
}
