using Incidences.Data.Models;

namespace Incidences.Models.Incidence
{
    public class PieceType
    {
        private string description;
        private string name;
        private int? id;

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
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
        public PieceType(piece_type pt)
        {
            this.id = pt.id;
            this.name = pt.name;
            this.description = pt.description;
        }
    }
}
