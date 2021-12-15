using System.Security.Cryptography;
using System.Text;

namespace Incidences.Models.Employee
{
    public class Credentials
    {
        private string username;
        private string password;
        private int? employeeId;

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = GetSHA1(value);
            }
        }
        public int? EmployeeId
        {
            get
            {
                return employeeId;
            }
            set
            {
                employeeId = value;
            }
        }
        public Credentials()
        {

        }
        public Credentials(string username, string password, int employeeId)
        {
            this.username = username;
            this.password = GetSHA1(password);
            this.employeeId = employeeId;
        }
        public Credentials(string username, string password)
        {
            this.username = username;
            this.password = GetSHA1(password);
        }

        /*private static string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }*/
        private static string GetSHA1(string str)
        {
            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new();
            StringBuilder sb = new();
            byte[] stream = sha1.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
