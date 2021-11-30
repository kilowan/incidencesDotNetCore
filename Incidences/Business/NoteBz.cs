using Incidences.Data;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;

namespace Incidences.Business
{
    public class NoteBz : INoteBz
    {
        private readonly INoteData noteData;
        public NoteBz(INoteData noteData)
        {
            this.noteData = noteData;
        }

        #region SELECT
        public Note SelectEmployeeNoteByIncidenceId(int incidenceId)
        {
            try
            {
                return noteData.SelectEmployeeNoteByIncidenceId(incidenceId);
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
                return noteData.SelectNotesByIncidenceId(incidenceId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region INSERT
        public bool InsertNoteFn(string note, int noteTypeId, int? userId, int? incidenceId)
        {
            try
            {
                return noteData.InsertNote(note, noteTypeId, userId, incidenceId);
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
                return noteData.UpdateNote(note, incidenceId, employeeId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
