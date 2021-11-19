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
        public Incidences GetIncidencesByStateTypeFn(int state, int userId, string type)
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
                Incidences incidences = new Incidences(own);
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
        public int LastIncidence(IList<string> fields)
        {
            try
            {
                object con = Select(new Select("parte", fields));
                using IDataReader reader = this.get_sql.ExecuteReader();
                reader.Read();
                return (int)reader.GetValue(0);
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public int LastIncidence()
        {
            try
            {
                object con = Select(new Select("parte"));
                using IDataReader reader = this.get_sql.ExecuteReader();
                reader.Read();
                return (int)reader.GetValue(0);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void UpdateIncidenceFn(IDictionary<string, object> args)
        {
            try
            {
                IDictionary<string, object> fullArgs = FillArgs(new List<string> { "incidenceId", "userId", "note", "state", "piecesAdded", "piecesDeleted", "close" }, args);
                Incidence incidence = SelectIncidences(new List<string>('*'), WhereIncidence(new CDictionary<string, string>(), Convert.ToInt16(fullArgs["incidenceId"])))[0];
                if (incidence.solverId == Convert.ToInt16(fullArgs["userI"]) || incidence.state == 1 || incidence.ownerId == Convert.ToInt16(fullArgs["userId"]))
                {
                    UpdateIncidence(Convert.ToInt16(fullArgs["state"]), Convert.ToInt16(fullArgs["incidenceId"]), Convert.ToInt16(fullArgs["userId"]), new Note(fullArgs["note"].ToString()), (List<int>)fullArgs["piecesAdded"], (List<int>)fullArgs["piecesDeleted"], (bool)fullArgs["close"]);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void UpdateIncidence(int? state, int incidenceId, int userId, Note note, IList<int> piecesAdded, IList<int> piecesDeleted, bool close)
        {
            try
            {
                CDictionary<string, string> columns = new CDictionary<string, string>();
                if (state != null) {
                    columns.Add("state", null, state.ToString());
                } else
                {
                    columns = new CDictionary<string, string>();
                    columns.Add("tec_res", null, userId.ToString());
                    columns.Add("state", null, "2");
                }
                if (close) {
                    columns.Add("fecha_resolucion", null, "CURRENT_DATE()");
                    columns.Add("hora_resolucion", null, "CURRENT_TIME()");
                }
                bool result = Update("parte", columns, WhereId_part(new CDictionary<string, string>(), incidenceId));
                if (!result) throw new Exception("Parte no actualizado");

                if (note != null) {
                    result = this.note.InsertNote(userId, incidenceId, note.noteStr, "Technician");
                    if (!result) throw new Exception("Parte no actualizado");
                }

                if (piecesAdded != null && piecesAdded.Count > 0) {
                    result = this.piece.InsertPiecesSql(piecesAdded, incidenceId);
                    if (!result) throw new Exception("Parte no actualizado");
                }

                if (piecesDeleted != null && piecesDeleted.Count > 0) {
                    result = this.piece.DeletePiecesSql(piecesDeleted, incidenceId);
                    if (!result) new Exception("Parte no actualizado");
                }
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public IDictionary<string, object>  FillArgs(IList<string> needed, IDictionary<string, object> args)
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
                object con = Select(new Select("FullIncidence", fields, conditions));
                IList<Incidence> incidences = new List<Models.Incidence.Incidence>();
                if (con != null) {
                    using IDataReader reader = this.get_sql.ExecuteReader();
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
                    IList<string> list = new List<string>();
                    list.Add("*");
                    conditions = WhereIncidenceId(new CDictionary<string, string>(), incidence.id);
                    con = Select(new Select("incidence_pieces", list, conditions));
                    if (con != null)
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
                    con = Select(new Select("incidence_notes", list, conditions));
                    if (con != null)
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
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool InsertIncidence(int ownerId, string issueDesc, IList<int> pieces)
        {
            try
            {
                bool result = Insert("parte", new CDictionary<string, string> { { "emp_crea", null, ownerId.ToString() } });
                if (!result) throw new Exception("Parte no insertado");
                
                int id = LastIncidence();

                result = this.note.InsertNote(ownerId, id, issueDesc, "Employee");
                if (!result) throw new Exception("Parte no insertado");
                result = this.piece.InsertPiecesSql(pieces, id);
                if (!result) throw new Exception("Parte no insertado");
                return result;
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public bool DeleteIncidenceFn(int id_part, int emp_crea)
        {
            try
            {
                bool result = Update(
                    "parte", 
                    new CDictionary<string, string> { 
                        { "state", null, "5" } 
                    }, 
                    new CDictionary<string, string> { 
                        { "id_part", null, id_part.ToString()}, 
                        { "emp_crea", null, emp_crea.ToString() }
                    }
                );
                return result;
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
    }
}
