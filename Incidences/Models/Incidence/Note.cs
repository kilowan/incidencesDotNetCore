using Incidences.Data.Models;
using System;

namespace Incidences.Models.Incidence
{
    public class Note
    {
        private string noteStr;
        private DateTime date;
        private NoteType type;

        public string NoteStr
        {
            get
            {
                return noteStr;
            }
            set
            {
                noteStr = value;
            }
        }
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }
        public NoteType Type
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

        public Note()
        {

        }
        public Note(Notes notes)
        {
            this.noteStr = notes.noteStr;
            this.type = new NoteType(notes.NoteType);
            this.date = notes.date;
        }
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
