using Incidences.Data;
using Incidences.Data.Models;
using Incidences.Models.Employee;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Credentials = Incidences.Models.Employee.Credentials;

namespace Incidences.Business
{
    public class CredentialsBz : ICredentialsBz
    {
        private readonly ICredentialsData credentialsData;
        private readonly IConfiguration configuration;
        public CredentialsBz(ICredentialsData credentialsData, IConfiguration configuration)
        {
            this.credentialsData = credentialsData;
            this.configuration = configuration;
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
        public bool CheckCredentials(string username)
        {
            try
            {
                return credentialsData.CheckCredentials(username);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public string Login(string username, string password)
        {
            try
            {
                string result = "False";
                employee userExist = credentialsData.CheckCredentials(username, password);
                if (userExist.name != null)
                {
                    Employee user = new Employee(userExist);
                    result = MakeTokenJWT(user);
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdatePassword(CredentialsDto creds, string code)
        {
            try
            {
                return credentialsData.UpdatePassword(creds, code);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private string MakeTokenJWT(Employee usuarioInfo)
        {
            // CREAMOS EL HEADER //
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JWT:ClaveSecreta"])
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            // CREAMOS LOS CLAIMS //
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.Id.ToString()),
                new Claim("nombre", usuarioInfo.Name),
                new Claim("apellidos", $"{usuarioInfo.Surname1} {usuarioInfo.Surname2}"),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.Email.fullEmail)
            };

            // CREAMOS EL PAYLOAD //
            var _Payload = new JwtPayload(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    // Exipra a la 24 horas.
                    expires: DateTime.UtcNow.AddHours(24)
                );

            // GENERAMOS EL TOKEN //
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }
        #endregion
    }
}