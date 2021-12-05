using Incidences.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Incidences.Models.Incidence
{
    public class Incidence
    {
        private string owner;
        private int? ownerId;
        private string solver;
        private int? solverId;
        private DateTime? initDateTime;
        private string issueDesc;
        private IList<Piece> pieces;
        private IList<Note> notes;
        private int? state;
        private int id;
        private DateTime? finishDateTime;

        public string Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }
        public int? OwnerId
        {
            get
            {
                return ownerId;
            }
            set
            {
                ownerId = value;
            }
        }
        public string Solver
        {
            get
            {
                return solver;
            }
            set
            {
                solver = value;
            }
        }
        public int? SolverId
        {
            get
            {
                return solverId;
            }
            set
            {
                solverId = value;
            }
        }
        public DateTime? InitDateTime
        {
            get
            {
                return initDateTime;
            }
            set
            {
                initDateTime = value;
            }
        }
        public string IssueDesc
        {
            get
            {
                return issueDesc;
            }
            set
            {
                issueDesc = value;
            }
        }
        public IList<Piece> Pieces
        {
            get
            {
                return pieces;
            }
            set
            {
                pieces = value;
            }
        }
        public IList<Note> Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
            }
        }
        public int? State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
        public int Id
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
        public DateTime? FinishDateTime
        {
            get
            {
                return finishDateTime;
            }
            set
            {
                finishDateTime = value;
            }
        }

        public Incidence()
        {

        }

        public Incidence(
            int id, 
            string owner, 
            int ownerId, 
            DateTime initDateTime, 
            string issueDesc, 
            int state, 
            string solver, 
            int? solverId, 
            DateTime? FinishDateTime
            )
        {
            this.id = id;
            this.owner = owner;
            this.ownerId = ownerId;
            this.issueDesc = issueDesc;
            this.initDateTime = initDateTime;
            this.pieces = new List<Piece>();
            //this.pieces.Add(piece);
            this.notes = new List<Note>();
            //this.notes.Add(note);
            this.finishDateTime = null;
            this.state = state;
            this.solver = solver;
            this.solverId = solverId;
            this.finishDateTime = FinishDateTime;
        }
        public Incidence(
            int id, 
            int state, 
            string owner, 
            int ownerId, 
            DateTime initDateTime, 
            string issueDesc, 
            IList<Piece> pieces
            )
        {
            this.id = id;
            this.state = state;
            this.owner = owner;
            this.ownerId = ownerId;
            this.issueDesc = issueDesc;
            this.initDateTime = initDateTime;
            this.pieces = pieces;
        }
        public Incidence(incidence inc)
        {
            if (inc.state != 1)
            {
                this.id = inc.id;
                this.state = inc.state;
                if (inc.owner.surname2 != null) this.owner = $"{inc.owner.name} {inc.owner.surname1} {inc.owner.surname2}";
                else this.owner = $"{inc.owner.name} {inc.owner.surname1}";
                this.ownerId = inc.ownerId;
                this.solverId = inc.solverId;
                if (inc.solver.surname2 != null) this.solver = $"{inc.solver.name} {inc.solver.surname1} {inc.solver.surname2}";
                else this.solver = $"{inc.solver.name} {inc.solver.surname1}";
                this.issueDesc = inc.notes
                    .Where(inc => inc.noteTypeId == 1)
                    .Select(inc => inc.noteStr)
                    .FirstOrDefault();
                this.initDateTime = inc.open_dateTime;
                this.pieces = new List<Piece>();
                foreach (incidence_piece_log ipl in inc.pieces)
                {
                    this.pieces.Add(new Piece(ipl.Piece));
                }
                Notes[] notes = inc.notes
                    .Where(inc => inc.noteTypeId == 2)
                    .ToArray();
                this.notes = new List<Note>();
                foreach (Notes note in notes)
                {
                    this.notes.Add(new Note(note));
                }
            }
            else
            {
                this.id = inc.id;
                this.state = inc.state;
                if(inc.owner.surname2 != null) this.owner = $"{inc.owner.name} {inc.owner.surname1} {inc.owner.surname2}";
                else this.owner = $"{inc.owner.name} {inc.owner.surname1}";
                this.ownerId = inc.ownerId;
                this.issueDesc = inc.notes
                    .Where(inc => inc.noteTypeId == 1)
                    .Select(inc => inc.noteStr)
                    .FirstOrDefault();
                this.initDateTime = inc.open_dateTime;
                this.pieces = new List<Piece>();
                foreach (incidence_piece_log ipl in inc.pieces)
                {
                    this.pieces.Add(new Piece(ipl.Piece));
                }
            }
        }
        public Incidence(
            int id,
            int state,
            string owner,
            int ownerId,
            DateTime initDateTime,
            string issueDesc,
            IList<piece_class> pieces
            )
        {
            this.id = id;
            this.state = state;
            this.owner = owner;
            this.ownerId = ownerId;
            this.issueDesc = issueDesc;
            this.initDateTime = initDateTime;
            this.pieces = new List<Piece>();
            foreach (piece_class piece in pieces)
            {
                this.pieces.Add(new Piece(piece));
            }
        }
        public Incidence(incidence inc, string issueDesc, IList<Note> notes)
        {
            this.id = inc.id;
            this.state = inc.state;
            this.owner = $"{inc.owner.name} {inc.owner.surname1} {inc.owner.surname2}";
            this.ownerId = inc.ownerId;
            this.solverId = inc.solverId;
            this.solver = $"{inc.solver.name} {inc.solver.surname1} {inc.solver.surname2}";
            this.issueDesc = issueDesc;
            this.initDateTime = inc.open_dateTime;
            this.pieces = new List<Piece>();
            foreach (incidence_piece_log ipl in inc.pieces)
            {
                this.pieces.Add(new Piece(ipl.Piece));
            }
            this.notes = notes;
        }
        public Incidence(int id, int state, string owner, int ownerId, DateTime initDateTime, string issueDesc, IList<Piece> pieces, IList<Note> notes, DateTime? finishDateTime)
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
        public Incidence(int id, int state, string owner, int ownerId, DateTime initDateTime, string issueDesc, IList<Piece> pieces, IList<Note> notes, string solver, int? solverId, DateTime? finishDateTime)
        {
            this.id = id;
            this.state = state;
            this.owner = owner;
            this.ownerId = ownerId;
            this.solver = solver;
            this.solverId = solverId;
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
