using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Models.Employee
{
    public class CredentialsDto
    {
        public string username { get; set; }
        public string password { get; set; }
        public int? employeeId { get; set; }
    }
}
