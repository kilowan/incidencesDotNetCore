using Incidences.Data.Models;

namespace Incidences.Models.Incidence
{
    public class Piece
    {
        private int? id;
        private string name;
        private PieceType type;
        private byte deleted;

        public int? Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public PieceType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        public byte Deleted
        {
            get
            {
                return deleted;
            }
            set
            {
                deleted = value;
            }
        }
        public Piece()
        {

        }
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
        public Piece(int id, string name, PieceType type, byte deleted)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.deleted = deleted;
        }
        public Piece(piece_class pc)
        {
            this.id = pc.id;
            this.name = pc.name;
            this.type = new PieceType(pc.PieceType);
            this.deleted = pc.deleted;
        }
    }
}
