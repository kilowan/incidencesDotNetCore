using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraApp.Business;
using MiPrimeraApp.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Controllers
{
    [Route("api/Credentials")]
    [ApiController]
    public class CredentialsController : ControllerBase
    {
        private CredentialsBz cred;
        public CredentialsController()
        {
            this.cred = new();
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
