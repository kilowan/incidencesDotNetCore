using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Models.Incidence
{
    public class Incidence
    {
        public string owner;
		public int ownerId;
		public string solver;
		public int? solverId;
        public DateTime initDateTime;
        public string issueDesc;
        public IList<Piece> pieces;
		public IList<Note> notes;
		public int state;
		public int id;
		public DateTime finishDateTime;
        public Incidence(string owner, int ownerId, DateTime initDateTime, string issueDesc, Piece piece, Note note)
        {
            this.owner = owner;
            this.ownerId = ownerId;
            this.issueDesc = issueDesc;
            this.initDateTime = initDateTime;
            this.pieces = new List<Piece>();
            this.pieces.Add(piece);
            this.notes = new List<Note>();
            this.notes.Add(note);
        }
        public Incidence(int id, int state, string owner, int ownerId, DateTime initDateTime, string issueDesc, IList<Piece> pieces, IList<Note> notes)
        {
            this.id = id;
            this.state = state;
            this.owner = owner;
            this.ownerId = ownerId;
            this.issueDesc = issueDesc;
            this.initDateTime = initDateTime;
            this.pieces = pieces;
            this.notes = notes;
        }
        public Incidence(int id, int state, string owner, int ownerId, DateTime initDateTime, string issueDesc, IList<Piece> pieces, IList<Note> notes, DateTime finishDateTime)
        {
            this.id = id;
            this.state = state;
            this.owner = owner;
            this.ownerId = ownerId;
            this.issueDesc = issueDesc;
            this.initDateTime = initDateTime;
            this.pieces = pieces;
            this.notes = notes;
            this.finishDateTime = finishDateTime;
        }
        public void AddPiece(Piece piece)
        {
            this.pieces.Add(piece);
        }
        public void AddNote(Note note)
        {
            this.notes.Add(note);
        }
    }
}
