using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Models.Incidence
{
    public class Note
    {
        public string noteStr;
		public DateTime date;
        public Note(string noteStr)
        {
            this.noteStr = noteStr;
            this.date = new DateTime();
        }
        public Note(string noteStr, DateTime date)
        {
            this.noteStr = noteStr;
            this.date = date;
        }
    }
}
