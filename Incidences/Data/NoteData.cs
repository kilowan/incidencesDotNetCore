using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Incidences.Data
{
    public class NoteData : INoteData
    {
        private readonly IncidenceContext _context;
        private readonly INoteTypeData noteTypeData;
        public NoteData(IncidenceContext context, INoteTypeData noteTypeData)
        {
            _context = context;
            this.noteTypeData = noteTypeData;
        }

        public Note SelectEmployeeNoteByIncidenceId(int incidenceId)
        {
            try
            {
                NoteType noteType = noteTypeData.GetNoteTypeByName("ownerNote");
                Notes note = _context.Notes
                    .Where(note => note.incidenceId == incidenceId && note.noteTypeId == noteType.Id)
                    .FirstOrDefault();
                if (note != null)
                {
                    return new(note.noteStr, noteType, note.date);
                }
                else return new Note();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IList<Note> SelectNotesByIncidenceId(int incidenceId)
        {
            try
            {
                NoteType noteType = noteTypeData.GetNoteTypeByName("solverNote");
                IList<Notes> notes = _context.Notes
                    .Where(note => note.incidenceId == incidenceId && note.noteTypeId == noteType.Id)
                    .ToList();

                IList<Note> result = new List<Note>();
                foreach (Notes note in notes)
                {
                    result.Add(new(note.noteStr, noteType, note.date));
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdateNote(string note, int incidenceId, int employeeId)
        {
            try
            {
                Notes noteData = _context.Notes
                    .Where(note => note.incidenceId == incidenceId && note.employeeId == employeeId)
                    .FirstOrDefault();
                noteData.noteStr = note;
                _context.Notes.Update(noteData);
                if (_context.SaveChanges() != 1) throw new Exception("Nota no actualizada");

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool InsertNote(string note, int noteTypeId, int? ownerId, int? incidenceId)
        {
            try
            {
                _context.Notes.Add(new Notes()
                {
                    noteStr = note,
                    noteTypeId = noteTypeId,
                    employeeId = (int)ownerId,
                    incidenceId = (int)incidenceId
                });
                if (_context.SaveChanges() != 1) throw new Exception("Nota no insertada");

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
