using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Data
{
    public class NoteData : INoteData
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

        private readonly ISqlBase sql;
        public NoteData(ISqlBase sql)
        {
            this.sql = sql;
        }

        public Note SelectEmployeeNoteByIncidenceId(int incidenceId)
        {
            try
            {
                return GetNotes(
                    sql.WhereNoteType("Employee", 
                        sql.WhereIncidenceId(incidenceId)
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
                    sql.WhereNoteType(
                        "Technician", 
                        sql.WhereIncidenceId(incidenceId)
                    )
                );
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
                bool result = sql.Update(
                    Notes,
                    new CDictionary<string, string> { { noteStr, null, note } },
                    new CDictionary<string, string>{
                        { incidenceIdC, null, incidenceId.ToString() },
                        { employeeIdC, null, employeeId.ToString() }
                    }
                );

                sql.Close();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private IList<Note> GetNotes(CDictionary<string, string> conditions)
        {
            bool result = sql.Select(
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
                using IDataReader reader = sql.GetReader();
                while (reader.Read())
                {
                    notes.Add(new Note((string)reader.GetValue(1), (DateTime)reader.GetValue(2)));
                }
                sql.Close();

                return notes;
            }
            else throw new Exception("Ningún registro");
        }
        public bool InsertNote(string note, int noteTypeId, int? ownerId, int? incidenceId)
        {
            try
            {
                bool result = sql.Insert(Notes,
                    new CDictionary<string, string>
                    {
                        { employeeIdC, null, ownerId.ToString() },
                        { incidenceIdC, null, incidenceId.ToString() },
                        { noteTypeIdC, null, $"{ noteTypeId }" },
                        { noteStr, null, $"'{ note }'" }
                    }
                );

                sql.Close();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
