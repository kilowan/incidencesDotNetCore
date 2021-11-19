namespace MiPrimeraApp.Models.Incidence
{
    public class NoteType
    {
        public int? id;
        public string name;
        public NoteType(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public NoteType()
        {
            this.id = null;
            this.name = null;
        }
    }
}
