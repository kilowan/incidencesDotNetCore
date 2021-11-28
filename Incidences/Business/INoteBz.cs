using Incidences.Models.Incidence;
using System.Collections.Generic;

namespace Incidences.Business
{
    public interface INoteBz
    {
        public Note SelectEmployeeNoteByIncidenceId(int incidenceId);
        public IList<Note> SelectNotesByIncidenceId(int incidenceId);
        public bool InsertNoteFn(string note, int noteTypeId, int? userId, int? incidenceId);
        public bool UpdateNote(string note, int incidenceId, int employeeId);
    }
}
