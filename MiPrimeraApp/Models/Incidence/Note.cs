using System;

namespace MiPrimeraApp.Models.Incidence
{
    public class Note
    {
        public string noteStr;
        public DateTime date;
        public NoteType type;
        public Note(string noteStr)
        {
            this.noteStr = noteStr;
            this.type = new NoteType();
            this.date = new DateTime();
        }
        public Note(string noteStr, DateTime date)
        {
            this.noteStr = noteStr;
            this.type = new NoteType();
            this.date = date;
        }
        public Note(string noteStr, NoteType type, DateTime date)
        {
            this.noteStr = noteStr;
            this.type = type;
            this.date = date;
        }
    }
}
