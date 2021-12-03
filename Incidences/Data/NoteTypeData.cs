using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data
{
    public class NoteTypeData : INoteTypeData
    {
        private readonly IncidenceContext _context;
        public NoteTypeData(IncidenceContext context)
        {
            _context = context;
        }

        public NoteType GetNoteTypeByName(string noteTypeName)
        {
            note_type noteType = _context.NoteType
                .Where(note => note.name == noteTypeName)
                .FirstOrDefault();

            return new NoteType(noteType.id, noteType.name);
        }
    }
}
