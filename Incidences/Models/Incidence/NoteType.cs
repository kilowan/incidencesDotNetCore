using Incidences.Data.Models;

namespace Incidences.Models.Incidence
{
    public class NoteType
    {
        private int? id;
        private string name;

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

        public NoteType(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public NoteType(note_type nt)
        {
            this.id = nt.id;
            this.name = nt.name;
        }
        public NoteType()
        {
            this.id = null;
            this.name = null;
        }
    }
}
