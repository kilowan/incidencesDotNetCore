using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Models.Incidence
{
    public class Piece
    {
		public int? id { get; set; }
		public string name { get; set; }
        public PieceType type { get; set; }
        public bool deleted { get; set; }
        public Piece(string name)
        {
            this.name = name;
            this.type = new PieceType();
        }
        public Piece(string name, PieceType type)
        {
            this.name = name;
            this.type = type;
        }
        public Piece(int id, string name, PieceType type, bool deleted)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.deleted = deleted;
        }
    }
}
