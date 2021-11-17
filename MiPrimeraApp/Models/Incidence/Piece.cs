using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Models.Incidence
{
    public class Piece
    {
		public int? id;
		public string name;
		public PieceType type;
		public int? deleted;
        public Piece(string name, PieceType type)
        {
            this.name = name;
            this.type = type;
        }
        public Piece(int id, string name, PieceType type, int deleted)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.deleted = deleted;
        }
    }
}
