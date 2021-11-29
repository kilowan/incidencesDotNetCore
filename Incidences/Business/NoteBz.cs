using Incidences.Data;
using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Business
{
    public class NoteBz : INoteBz
    {
        #region constants
        //tables
        private const string incidence_notes = "incidence_notes";
        private const string Notes = "Notes";

        //columns
        private const string noteStr = "noteStr";
        private const string date = "date";
        private const string employeeIdC = "employeeId";
        private const string incidenceIdC = "incidenceId";
        private const string noteTypeIdC = "noteTypeId";

        #endregion

        private ISqlBase sql;
        private IBusinessBase bisba;
        public NoteBz(ISqlBase sql, IBusinessBase bisba)
        {
            this.sql = sql;
            this.bisba = bisba;
        }

        #region SELECT
        public Note SelectEmployeeNoteByIncidenceId(int incidenceId)
        {
            try
            {
                bisba.WhereNoteType(bisba.WhereIncidenceId(new CDictionary<string, string>(), incidenceId), "Employee");
                return GetNotes(
                    bisba.WhereNoteType(
                        bisba.WhereIncidenceId(
                            new CDictionary<string, string>(), 
                            incidenceId
                        ), 
                        "Employee"
                    )
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
                    bisba.WhereNoteType(
                        bisba.WhereIncidenceId(
                            new CDictionary<string, string>(), 
                            incidenceId
                        ), 
                        "Technician"
                    )
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private IList<Note> GetNotes(CDictionary<string, string> conditions)
        {
            bool result = this.sql.Select(
                new Select(
                    incidence_notes,
                    new List<string> {
                            noteStr,
                            date
                    },
                    conditions
                )
            );
            if (result)
            {
                IList<Note> notes = new List<Note>();
                using IDataReader reader = this.sql.GetReader();
                while (reader.Read())
                {
                    notes.Add(new Note((string)reader.GetValue(1), (DateTime)reader.GetValue(2)));
                }
                this.sql.Close();

                return notes;
            }
            else throw new Exception("Ningún registro");
        }

        #endregion

        #region INSERT
        public bool InsertNoteFn(string note, int noteTypeId, int? userId, int? incidenceId)
        {
            try
            {
                return InsertNote(note, noteTypeId, userId, incidenceId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private bool InsertNote(string note, int noteTypeId, int? ownerId, int? incidenceId)
        {
            try
            {
                bool result = this.sql.Insert(Notes,
                    new CDictionary<string, string>
                    {
                        { employeeIdC, null, ownerId.ToString() },
                        { incidenceIdC, null, incidenceId.ToString() },
                        { noteTypeIdC, null, $"'{ noteTypeId }'" },
                        { noteStr, null, $"'{ note }'" }
                    }
                );

                this.sql.Close();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region UPDATE
        public bool UpdateNote(string note, int incidenceId, int employeeId)
        {
            try
            {
                bool result = this.sql.Update(
                    Notes,
                    new CDictionary<string, string> { { noteStr, null, note } },
                    new CDictionary<string, string>{
                        { incidenceIdC, null, incidenceId.ToString() },
                        { employeeIdC, null, employeeId.ToString() }
                    }
                );

                this.sql.Close();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
