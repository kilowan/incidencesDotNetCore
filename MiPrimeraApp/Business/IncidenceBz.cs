using Incidences.Models.Incidence;
using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace MiPrimeraApp.Business
{
    public class IncidenceBz : BusinessBase
    {
        private NoteBz note;
        private PieceBz piece;

        public IncidenceBz()
        {
            this.note = new NoteBz();
            this.piece = new PieceBz();
        }
        public IncidenceList GetIncidencesByStateTypeFn(int state, int userId, string type)
        {
            try
            {
                IList<Incidence> own = SelectIncidences(
                    new List<string>('*'), 
                    WhereEmployeeId(
                        WhereIncidenceState(
                            new CDictionary<string, string>(), 
                            state
                        ), 
                        userId
                    )
                );
                IncidenceList incidences = new (own);
                IList<string> list = new List<string>();
                list.Add("Technician");
                list.Add("Admin");
                list.Contains(type);
                if (list.Contains(type) && state != 4) {
                    incidences.other = SelectIncidences(
                        new List<string>('*'), 
                        WhereTechnicianId(
                            WhereIncidenceState(
                                new CDictionary<string, string>(), 
                                state
                            ),
                            userId
                        )
                    );
                }

                return incidences;
            } 
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public Incidence GetIncidenceByIdFn(int id)
        {
            try
            {
                return SelectIncidences(new List<string>('*'), WhereIncidence(new CDictionary<string, string>(), id))[0];
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        private int LastIncidence(IList<string> fields)
        {
            try
            {
                bool result = Select(new Select("parte", fields));
                if (result)
                {
                    using IDataReader reader = this.get_sql.ExecuteReader();
                    reader.Read();
                    return (int)reader.GetValue(0);
                }
                else throw new Exception("Ningún registro");
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        private int LastIncidence()
        {
            try
            {
                bool result = Select(new Select("incidence"));
                if (result)
                {
                    using IDataReader reader = this.get_sql.ExecuteReader();
                    reader.Read();
                    return (int)reader.GetValue(0);
                }
                else throw new Exception("Ningún registro");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void UpdateIncidenceFn(IncidenceDto incidenceDto, int incidenceId, int userId, bool close)
        {
            try
            {
                Incidence incidence = SelectIncidences(
                    new List<string>('*'), 
                    WhereIncidence(new CDictionary<string, string>(), 
                    incidenceId)
                )[0];
                if (incidence.solverId == userId || incidence.state == 1 || incidence.ownerId == userId)
                {
                    UpdateIncidence(incidenceDto, incidenceId, userId, close);
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
                CDictionary<string, string> columns = new CDictionary<string, string>();
                if (incidence.state != null) {
                    columns.Add("state", null, incidence.state.ToString());
                } else
                {
                    columns = new CDictionary<string, string>();
                    columns.Add("solverId", null, userId.ToString());
                    columns.Add("state", null, "2");
                }
                if (close) {
                    columns.Add("close_dateeTime", null, "CURRENT_TIMESTAMP()");
                }
                bool result = Update(
                    "incidence", 
                    columns,
                    WhereIncidenceId(
                        new CDictionary<string, string>(), 
                        incidenceId
                    )
                );
                if (!result) throw new Exception("Parte no actualizado");

                if (note != null) {

                    result = this.note.InsertNoteFn(incidence.note, userId, incidenceId);
                    if (!result) throw new Exception("Parte no actualizado");
                }

                if (incidence.piecesAdded != null && incidence.piecesAdded.Count > 0) {
                    result = this.piece.InsertPiecesSql(incidence.piecesAdded, incidenceId);
                    if (!result) throw new Exception("Parte no actualizado");
                }

                if (incidence.piecesDeleted != null && incidence.piecesDeleted.Count > 0) {
                    result = this.piece.DeletePiecesSql(incidence.piecesDeleted, incidenceId);
                    if (!result) new Exception("Parte no actualizado");
                }
                return result;
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        private IDictionary<string, object>  FillArgs(IList<string> needed, IDictionary<string, object> args)
        {
            foreach (string value in needed)
            {
                if (!args.ContainsKey(value)) args.Add(value, null);
            }

            return args;
        }
        public IList<Incidence> SelectIncidences(IList<string> fields, CDictionary<string, string> conditions = null)
        {
            try
            {
                bool result = Select(new Select("FullIncidence", fields, conditions));
                IList<Incidence> incidences = new List<Incidence>();
                if (result) 
                {
                    using (IDataReader reader = this.get_sql.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Incidence inc = new Incidence(
                                (int)reader.GetValue(0),
                                (string)reader.GetValue(2),
                                (int)reader.GetValue(1),
                                (DateTime)reader.GetValue(6),
                                (string)reader.GetValue(3)
                            );
                            incidences.Add(inc);
                        }
                    }

                    foreach (Incidence incidence in incidences)
                    {
                        IList<string> list = new List<string>
                        {
                            "*"
                        };
                        conditions = WhereIncidenceId(new CDictionary<string, string>(), incidence.id);
                        result = Select(new Select("incidence_pieces", list, conditions));
                        if (result)
                        {
                            IList<Piece> pieces = new List<Piece>();
                            using IDataReader reader = this.get_sql.ExecuteReader();
                            while (reader.Read())
                            {
                                Piece piece = new Piece(
                                    (string)reader.GetValue(3),
                                    new PieceType(
                                        (int)reader.GetValue(1),
                                        (string)reader.GetValue(2),
                                        (string)reader.GetValue(3)
                                    )
                                );
                                pieces.Add(piece);
                            }
                        }
                        conditions = WhereNoteType(conditions, "ownerNote");
                        result = Select(new Select("incidence_notes", list, conditions));
                        if (result)
                        {
                            IList<Note> notes = new List<Note>();
                            using IDataReader reader = this.get_sql.ExecuteReader();
                            while (reader.Read())
                            {
                                Note note = new Note(
                                    (string)reader.GetValue(1),
                                    (DateTime)reader.GetValue(2)
                                );
                                notes.Add(note);
                            }
                        }
                    }
                    return incidences;
                } else throw new Exception("Ningún registro");
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool InsertIncidence(IncidenceDto incidence)
        {
            try
            {
                bool result = Insert(
                    "incidence",
                    WhereOwnerId(new CDictionary<string, string>(), incidence.ownerId)
                );
                if (!result) throw new Exception("Parte no insertado");
                int id = LastIncidence();

                result = this.note.InsertNoteFn(incidence.note, incidence.ownerId, id);
                if (!result) throw new Exception("Parte no insertado");
                result = this.piece.InsertPiecesSql(incidence.piecesAdded, id);
                if (!result) throw new Exception("Parte no insertado");
                return result;
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool DeleteIncidenceFn(int incidenceId, int userId)
        {
            try
            {
                
                bool result = Update(
                    "incidence", 
                    new CDictionary<string, string> { 
                        { "state", null, "5" } 
                    },
                    WhereOwnerId(
                        WhereIncidenceId(
                            new CDictionary<string, string>(), 
                            incidenceId
                        ), 
                        userId
                    )
                );
                return result;
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
    }
}
