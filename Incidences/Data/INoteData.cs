using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data
{
    public interface INoteData
    {
        public Note SelectEmployeeNoteByIncidenceId(int incidenceId);
        public IList<Note> SelectNotesByIncidenceId(int incidenceId);
        public bool InsertNote(string note, int noteTypeId, int? ownerId, int? incidenceId);
        public bool UpdateNote(string note, int incidenceId, int employeeId);
    }
}
