using System.Security.Cryptography;
using System.Text;

namespace MiPrimeraApp.Models.Employee
{
    public class Credentials
    {
        public string username;
        public string password;
        public int? employeeId;
        public Credentials()
        {

        }
        public Credentials(string username, string password, int employeeId)
        {
            this.username = username;
            this.password = GetMD5(password);
            this.employeeId = employeeId;
        }

        private static string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
