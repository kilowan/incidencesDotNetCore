using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Models.Incidence
{
    public class PieceType
    {
        public string description;
		public string name;
		public int? id;
        public PieceType()
        {
            this.name = string.Empty;
            this.description = string.Empty;
        }
        public PieceType(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
        public PieceType(int id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }
    }
}
