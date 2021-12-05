using Incidences.Models.Employee;
using System;
using System.Data;
using System.Linq;
using Credentials = Incidences.Data.Models.Credentials;

namespace Incidences.Data
{
    public class CredentialsData : ICredentialsData
    {
        private readonly IncidenceContext _context;

        public CredentialsData(IncidenceContext context)
        {
            this._context = context;
        }

        public Incidences.Models.Employee.Credentials SelectCredentialsById(int id)
        {
            try
            {
                Credentials old = _context.Credentialss
                    .Where(emp => emp.id == id)
                    .FirstOrDefault();

                return new Incidences.Models.Employee.Credentials()
                {
                    Username = old.username,
                    Password = old.password,
                    EmployeeId = old.employeeId
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Incidences.Models.Employee.Credentials SelectCredentialsByUsername(string username)
        {
            try
            {
                Credentials cred = _context.Credentialss
                    .Where(a => a.username == username)
                    .FirstOrDefault();
                return new()
                {
                    EmployeeId = cred.employeeId,
                    Username = cred.username,
                    Password = cred.password
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdatePassword(string password, int employeeId)
        {
            try
            {
                bool result = false;
                Credentials old = _context.Credentialss
                    .Where(emp => emp.employeeId == employeeId)
                    .FirstOrDefault();
                if (old != null)
                {
                    old.password = password;
                    _context.Credentialss.Update(old);
                    _context.SaveChanges();
                    result = true;
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool CheckCredentials(string username)
        {
            try
            {
                Credentials old = _context.Credentialss
                    .Where(emp => emp.username == username)
                    .FirstOrDefault();
                if (old != null) return true;
                else return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool CheckCredentials(string username, string password)
        {
            try
            {
                Credentials old = _context.Credentialss
                    .Where(emp => emp.username == username && emp.password == password)
                    .FirstOrDefault();
                if (old != null) return true;
                else return false;
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
                bool result = false;
                Credentials old = _context.Credentialss
                    .Where(emp => emp.employeeId == employeeId)
                    .FirstOrDefault();
                if (old != null)
                {
                    old.password = credentials.password;
                    old.username = credentials.username;
                    _context.Credentialss.Update(old);
                    _context.SaveChanges();
                    result = true;
                }

                return result;
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
                bool result = false;
                Credentials old = _context.Credentialss
                    .Where(emp => emp.employeeId == employeeId)
                    .FirstOrDefault();
                if (old != null)
                {
                    old.username = username;
                    _context.Credentialss.Update(old);
                    _context.SaveChanges();
                    result = true;
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool InsertCredentials(CredentialsDto credentials, int? employeeId) 
        {
            Credentials cred = new()
            {
                username = credentials.username,
                password = credentials.password
            };

            _context.Credentialss.Add(cred);
            if(_context.SaveChanges() != 1) throw new Exception("Empleado no insertado");
            return true;
        }
    }
}
