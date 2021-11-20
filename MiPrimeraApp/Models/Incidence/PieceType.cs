using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Models.Incidence
{
    public class PieceType
    {
        public string description { get; set; }
        public string name { get; set; }
        public int? id { get; set; }
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
