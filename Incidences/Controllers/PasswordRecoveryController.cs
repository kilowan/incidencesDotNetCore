using Incidences.Business;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Incidences.Controllers
{
    [Route("api/PasswordRecovery")]
    [ApiController]
    public class PasswordRecoveryController : ControllerBase
    {
        IPasswordRecoveryBz passwordRecoveryBz;
        public PasswordRecoveryController(IPasswordRecoveryBz passwordRecoveryBz)
        {
            this.passwordRecoveryBz = passwordRecoveryBz;
        }

        [HttpGet("{code}/{username}")]
        public bool CheckRecoverCode(string code, string username)
        {
            try
            {
                return passwordRecoveryBz.CheckRecoverCode(code, username);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("{username}")]
        public bool RecoverPassword(string username)
        {
            try
            {
                return passwordRecoveryBz.RecoverPassword(username);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
