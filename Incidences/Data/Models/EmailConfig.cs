using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data.Models
{
    public class EmailConfig : baseClass
    {
        public string username { get; set; }
        public string password { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public bool ssl { get; set; }
        public bool defaultCredentials { get; set; }
    }
}
