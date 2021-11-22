using Incidences.Models.Incidence;
using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace MiPrimeraApp.Business
{
    public class NoteBz : BusinessBase
    {
        #region SELECT
        public Note SelectEmployeeNoteByIncidenceId(int incidenceId)
        {
            try
            {
                return GetNotes(
                    new CDictionary<string, string> {
                        { "incidence", null, incidenceId.ToString() },
                        { "noteType", null, "Employee" }
                    }
                )[0];
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
                return GetNotes(
                    new CDictionary<string, string> {
                        { "incidence", null, incidenceId.ToString() },
                        { "noteType", null, "Technician" }
                    }
                );
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        private IList<Note> GetNotes(CDictionary<string, string> conditions) 
        {
            bool result = Select(
                new Select(
                    "incidence_notes",
                    new List<string> {
                            "noteStr",
                            "date"
                    },
                    conditions
                )
            );
            if (result)
            {
                IList<Note> notes = new List<Note>();
                using IDataReader reader = this.get_sql.ExecuteReader();
                while (reader.Read())
                {
                    notes.Add(new Note((string)reader.GetValue(1), (DateTime)reader.GetValue(2)));
                }

                return notes;
            }
            else throw new Exception("Ningún registro");
        }
        #endregion
        #region INSERT
        public bool InsertNoteFn(NoteDto note, int? userId, int? incidenceId)
        {
            try
            {
                return InsertNote(note, userId, incidenceId);
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        private bool InsertNote(NoteDto note, int? ownerId, int? incidenceId)
        {
            try
            {
                
                return Insert("notes",
                    new CDictionary<string, string> 
                    {
                        { "employee", null, ownerId.ToString() },
                        { "incidence", null, incidenceId.ToString() },
                        { "noteTypeId", null, $"'{ note.typeId }'" },
                        { "noteStr", null, $"'{ note.noteStr }'" }
                    }
                );
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        #endregion
        #region UPDATE
        public bool UpdateNote(string note, int incidenceId, int employeeId)
        {
            try
            {
                return Update(
                    "notes",
                    new CDictionary<string, string> { { "noteStr", null, note } }, 
                    new CDictionary<string, string>{
                        { "incidence", null, incidenceId.ToString() },
                        { "employee", null, employeeId.ToString() } 
                    }
                );
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
            }
        #endregion
    }
}
