using Incidences.Business;
using Incidences.Data;
using Incidences.Models.Incidence;
using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace MiPrimeraApp.Business
{
    public class IncidenceBz : IIncidenceBz
    {
        private INoteBz note;
        private IPieceBz piece;
        private IBusinessBase bisba;
        private ISqlBase sql;

        public IncidenceBz(INoteBz note, IPieceBz piece, IBusinessBase bisba, ISqlBase sql)
        {
            this.note = note;
            this.piece = piece;
            this.bisba = bisba;
            this.sql = sql;
        }
        public IncidenceList GetIncidencesByStateTypeFn(int state, int userId, string type)
        {
            try
            {
                IList<Incidence> own = SelectIncidences(
                    new List<string> { "*" },
                    this.bisba.WhereEmployeeId(
                        this.bisba.WhereIncidenceState(
                            new CDictionary<string, string>(),
                            state
                        ),
                        userId
                    )
                );
                IncidenceList incidences = new(own);
                IList<string> list = new List<string>();
                list.Add("Technician");
                list.Add("Admin");
                list.Contains(type);
                if (list.Contains(type) && state != 4)
                {
                    string query = $"SELECT * FROM FullIncidence WHERE state = { state } AND (TechnicianId = {userId} OR TechnicianId IS NULL)";
                    incidences.Other = SelectIncidences(query);
                }

                return incidences;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Incidence GetIncidenceByIdFn(int id)
        {
            try
            {
                return SelectIncidences(new List<string> { "*" }, this.bisba.WhereIncidence(new CDictionary<string, string>(), id))[0];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private int LastIncidence(IList<string> fields)
        {
            try
            {
                bool result = this.sql.Select(new Select("incidence", fields));
                if (result)
                {
                    using IDataReader reader = this.sql.GetReader(); reader.Read();
                    int id = (int)reader.GetValue(0);
                    this.sql.Close();
                    return id;
                }
                else throw new Exception("Ningún registro");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private int LastIncidence()
        {
            try
            {
                return LastIncidence(new List<string> { "MAX(id)" });
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
                    this.bisba.WhereIncidence(new CDictionary<string, string>(),
                    incidenceId)
                )[0];
                if (incidence.SolverId == userId || incidence.State == 1 || incidence.OwnerId == userId)
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
                if (incidence.state != null)
                {
                    columns.Add("state", null, incidence.state.ToString());
                }
                else
                {
                    columns = new CDictionary<string, string>();
                    columns.Add("solverId", null, userId.ToString());
                    columns.Add("state", null, "2");
                }
                if (close)
                {
                    columns.Add("close_dateeTime", null, "CURRENT_TIMESTAMP()");
                }
                bool result = this.sql.Update(
                    "incidence",
                    columns,
                    this.bisba.WhereIncidenceId(
                        new CDictionary<string, string>(),
                        incidenceId
                    )
                );
                this.sql.Close();
                if (!result) throw new Exception("Parte no actualizado");

                if (note != null)
                {
                    result = this.note.InsertNoteFn(incidence.note, 2, userId, incidenceId);
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
        public IList<Incidence> SelectIncidences(IList<string> fields, CDictionary<string, string> conditions = null)
        {
            try
            {
                bool result = this.sql.Select(new Select("FullIncidence", fields, conditions));
                IList<Incidence> incidences = new List<Incidence>();
                if (result)
                {
                    using (IDataReader reader = this.sql.GetReader())
                    {
                        while (reader.Read())
                        {
                            Incidence inc = new(
                                (int)reader.GetValue(0),
                                (string)reader.GetValue(2),
                                (int)reader.GetValue(1),
                                (DateTime)reader.GetValue(6),
                                (string)reader.GetValue(3),
                                (int)reader.GetValue(8)
                            );
                            incidences.Add(inc);
                        }
                        this.sql.Close();
                    }

                    foreach (Incidence incidence in incidences)
                    {
                        IList<string> list = new List<string>
                        {
                            "*"
                        };
                        conditions = this.bisba.WhereIncidenceId(new CDictionary<string, string>(), incidence.Id);
                        result = this.sql.Select(new Select("incidence_pieces", list, conditions));
                        if (result)
                        {
                            IList<Piece> pieces = new List<Piece>();
                            using (IDataReader reader = this.sql.GetReader())
                            {
                                while (reader.Read())
                                {
                                    Piece piece = new(
                                        (int)reader.GetValue(1),
                                        (string)reader.GetValue(5),
                                        new PieceType(
                                            (int)reader.GetValue(2),
                                            (string)reader.GetValue(3),
                                            (string)reader.GetValue(4)
                                        ),
                                        Convert.ToBoolean(reader.GetValue(6))
                                    );
                                    pieces.Add(piece);
                                }
                                this.sql.Close();
                            }
                            incidence.Pieces = pieces;
                        }
                        conditions = this.bisba.WhereNoteType(conditions, "solverNote");
                        result = this.sql.Select(new Select("incidence_notes", list, conditions));
                        if (result)
                        {
                            IList<Note> notes = new List<Note>();
                            using IDataReader reader = this.sql.GetReader();
                            while (reader.Read())
                            {
                                Note note = new(
                                    (string)reader.GetValue(1),
                                    (DateTime)reader.GetValue(2)
                                );
                                notes.Add(note);
                            }

                            this.sql.Close();
                            incidence.Notes = notes;
                        }
                    }
                }
                return incidences;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private IList<Incidence> SelectIncidences(string query)
        {
            try
            {
                bool result = this.sql.Select(query);
                IList<Incidence> incidences = new List<Incidence>();
                if (result)
                {
                    using (IDataReader reader = this.sql.GetReader())
                    {
                        while (reader.Read())
                        {
                            Incidence inc = new(
                                (int)reader.GetValue(0),
                                (string)reader.GetValue(2),
                                (int)reader.GetValue(1),
                                (DateTime)reader.GetValue(6),
                                (string)reader.GetValue(3),
                                (int)reader.GetValue(8)
                            );
                            incidences.Add(inc);
                        }
                        this.sql.Close();
                    }
                    CDictionary<string, string> conditions;
                    foreach (Incidence incidence in incidences)
                    {
                        IList<string> list = new List<string>
                        {
                            "*"
                        };
                        conditions = this.bisba.WhereIncidenceId(new CDictionary<string, string>(), incidence.Id);
                        result = this.sql.Select(new Select("incidence_pieces", list, conditions));
                        if (result)
                        {
                            IList<Piece> pieces = new List<Piece>();
                            using (IDataReader reader = this.sql.GetReader())
                            {
                                while (reader.Read())
                                {
                                    Piece piece = new(
                                        (int)reader.GetValue(1),
                                        (string)reader.GetValue(5),
                                        new PieceType(
                                            (int)reader.GetValue(2),
                                            (string)reader.GetValue(3),
                                            (string)reader.GetValue(4)
                                        ),
                                        Convert.ToBoolean(reader.GetValue(6))
                                    );
                                    pieces.Add(piece);
                                }
                                this.sql.Close();
                            }
                            incidence.Pieces = pieces;
                        }
                        conditions = this.bisba.WhereNoteType(conditions, "solverNote");
                        result = this.sql.Select(new Select("incidence_notes", list, conditions));
                        if (result)
                        {
                            IList<Note> notes = new List<Note>();
                            using IDataReader reader = this.sql.GetReader();
                            while (reader.Read())
                            {
                                Note note = new(
                                    (string)reader.GetValue(1),
                                    (DateTime)reader.GetValue(2)
                                );
                                notes.Add(note);
                            }

                            this.sql.Close();
                            incidence.Notes = notes;
                        }
                    }
                }
                return incidences;
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
                bool result = this.sql.Insert(
                    "incidence",
                    this.bisba.WhereOwnerId(new CDictionary<string, string>(), incidence.ownerId)
                );
                if (!result) throw new Exception("Parte no insertado");
                int id = LastIncidence();

                result = this.note.InsertNoteFn(incidence.note, 1, incidence.ownerId, id);
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
        public bool DeleteIncidenceFn(int incidenceId, int userId)
        {
            try
            {

                bool result = this.sql.Update(
                    "incidence",
                    new CDictionary<string, string> {
                        { "state", null, "5" }
                    },
                    this.bisba.WhereOwnerId(
                        this.bisba.WhereIncidenceId(
                            new CDictionary<string, string>(),
                            incidenceId
                        ),
                        userId
                    )
                );
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IDictionary<string, int> GetIncidencesCounters(int userId, string type)
        {
            IDictionary<string, int> counters = new Dictionary<string, int>
            {
                { "new", 0 },
                { "old", 0 },
                { "closed", 0 },
                { "hidden", 0 },
                { "total", 0 }
            };
            IList<string> types = new List<string> { "Technician", "Admin" };
            if (types.Contains(type))
            {
                //Technician or Admin
                counters = this.sql.GetCounters(this.sql.MultiSelect(this.sql.GetStringArray(3, "solverId", userId)), counters);
            }

            counters = this.sql.GetCounters(
                this.sql.MultiSelect(
                    this.sql.GetStringArray(
                        4, 
                        "ownerId", 
                        userId
                    )
                ), counters
            );
            return counters;
        }
    }
}
