using Incidences.Business;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraApp.Models.Employee;

namespace MiPrimeraApp.Controllers
{
    [Route("api/Credentials")]
    [ApiController]
    public class CredentialsController : ControllerBase
    {
        private ICredentialsBz cred;
        public CredentialsController(ICredentialsBz credentials)
        {
            this.cred = credentials;
        }

        [HttpGet("{username}/{password}")]
        public bool Details(string username, string password)
        {
            return this.cred.CheckCredentialsFn(username, password);
        }

        [HttpPut("{employeeId}")]
        public bool Update(CredentialsDto credentials, int employeeId)
        {
            return this.cred.UpdateCredentials(credentials, employeeId);
        }
    }
}
