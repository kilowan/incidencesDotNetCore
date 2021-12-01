using Incidences.Data;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;

namespace Incidences.Business
{
    public class IncidenceBz : IIncidenceBz
    {

        private readonly INoteBz note;
        private readonly IPieceBz piece;
        private readonly IIncidenceData incidenceData;

        public IncidenceBz(IIncidenceData incidenceData, INoteBz note, IPieceBz piece)
        {
            this.note = note;
            this.piece = piece;
            this.incidenceData = incidenceData;
        }
        public IncidenceList GetIncidencesByStateType(int state, int userId, string type)
        {
            try
            {
                return incidenceData.GetIncidencesByStateType(state, userId, type);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Incidence GetIncidenceById(int id)
        {
            try
            {
                return incidenceData.GetIncidenceById(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool NewUpdateIncidence(IncidenceDto incidence, int incidenceId, int userId, bool close = false)
        {
            try
            {
                if (close)
                {
                    return CloseIncidence(incidence, incidenceId, userId);
                }
                else if (incidence.state == 2)
                {
                    return incidenceData.AttendIncidence(incidence, incidenceId, userId);
                }
                else if (incidence.state == 3)
                {
                    return CloseIncidence(incidence, incidenceId, userId);
                }
                else
                {
                    bool result = incidenceData.NewUpdateIncidence(incidence, incidenceId);
                    if (!result) throw new Exception("Parte no actualizado");

                    if (note != null)
                    {
                        result = this.note.InsertNote(incidence.note, 2, userId, incidenceId);
                        if (!result) throw new Exception("Parte no actualizado");
                    }

                    if (incidence.piecesAdded != null && incidence.piecesAdded.Count > 0)
                    {
                        result = this.piece.InsertPiecesSql(incidence.piecesAdded, incidenceId);
                        if (!result) throw new Exception("Parte no actualizado");
                    }

                    if (incidence.piecesDeleted != null && incidence.piecesDeleted.Count > 0)
                    {
                        result = this.piece.DeletePiecesSql(incidence.piecesDeleted, incidenceId);
                        if (!result) _ = new Exception("Parte no actualizado");
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdateIncidence(IncidenceDto incidence, int incidenceId, int userId, bool close = false)
        {
            try
            {
                bool result = incidenceData.UpdateIncidence(incidence, incidenceId, userId, close);
                if (!result) throw new Exception("Parte no actualizado");

                if (note != null)
                {
                    result = this.note.InsertNote(incidence.note, 2, userId, incidenceId);
                    if (!result) throw new Exception("Parte no actualizado");
                }

                if (incidence.piecesAdded != null && incidence.piecesAdded.Count > 0)
                {
                    result = this.piece.InsertPiecesSql(incidence.piecesAdded, incidenceId);
                    if (!result) throw new Exception("Parte no actualizado");
                }

                if (incidence.piecesDeleted != null && incidence.piecesDeleted.Count > 0)
                {
                    result = this.piece.DeletePiecesSql(incidence.piecesDeleted, incidenceId);
                    if (!result) _ = new Exception("Parte no actualizado");
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool InsertIncidence(IncidenceDto incidence)
        {
            try
            {
                int id = incidenceData.InsertIncidence(incidence.ownerId);
                bool result = this.note.InsertNote(incidence.note, 1, incidence.ownerId, id);
                if (!result) throw new Exception("Parte no insertado");
                result = this.piece.InsertPiecesSql(incidence.piecesAdded, id);
                if (!result) throw new Exception("Parte no insertado");
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool DeleteIncidence(int incidenceId, int userId)
        {
            try
            {
                return incidenceData.DeleteIncidence(incidenceId, userId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IDictionary<string, int> GetIncidencesCounters(int userId, string type)
        {
            try
            {
                return incidenceData.GetIncidencesCounters(userId, type);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //changeState 2-3
        private bool CloseIncidence(IncidenceDto incidence, int incidenceId, int userId)
        {
            try
            {
                bool result = incidenceData.CloseIncidence(incidenceId);
                if (!result) throw new Exception("Parte no actualizado");
                result = this.note.InsertNote(incidence.note, 2, userId, incidenceId);
                if (!result) throw new Exception("Parte no actualizado");

                if (incidence.piecesAdded != null && incidence.piecesAdded.Count > 0)
                {
                    result = this.piece.InsertPiecesSql(incidence.piecesAdded, incidenceId);
                    if (!result) throw new Exception("Parte no actualizado");
                }

                if (incidence.piecesDeleted != null && incidence.piecesDeleted.Count > 0)
                {
                    result = this.piece.DeletePiecesSql(incidence.piecesDeleted, incidenceId);
                    if (!result) _ = new Exception("Parte no actualizado");
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
