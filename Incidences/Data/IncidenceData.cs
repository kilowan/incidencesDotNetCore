﻿using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Data
{
    public class IncidenceData : IIncidenceData
    {
        #region constants
        //tables
        private const string incidenceC = "incidence";
        private const string FullIncidence = "FullIncidence";
        private const string incidence_pieces = "incidence_pieces";
        private const string incidence_notes = "incidence_notes";

        //columns
        private const string counter = "COUNT(*) AS counter";
        private const string ALL = "*";
        private const string ownerId = "ownerId";
        private const string stateC = "state";
        private const string technicianId = "technicianId";
        private const string max = "MAX(id)";
        private const string solverId = "solverId";
        private const string close_dateeTime = "close_dateeTime";
        private const string TIMESTAMP = "CURRENT_TIMESTAMP()";
        private const string solverNote = "solverNote";

        #endregion

        private readonly ISqlBase sql;

        public IncidenceData(ISqlBase sql)
        {
            this.sql = sql;
        }

        public int LastIncidence(IList<string> fields)
        {
            try
            {
                bool result = sql.Select(new Select(incidenceC, fields));
                if (result)
                {
                    using IDataReader reader = sql.GetReader(); reader.Read();
                    int id = (int)reader.GetValue(0);
                    sql.Close();
                    return id;
                }
                else throw new Exception("Ningún registro");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int LastIncidence()
        {
            try
            {
                return LastIncidence(new List<string> { max });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IDictionary<string, int> GetCounters(int state, string column, int userId, IDictionary<string, int> counters)
        {
            IList<string> sentences = new List<string>();
            for (int i = 1; i <= state; i++)
            {
                string field = incidenceC;
                Select select;
                IList<string> columns = new List<string> { counter };
                if (column == ownerId)
                    select = new Select(
                    field,
                    columns,
                    new CDictionary<string, string> {
                        { stateC, null, state.ToString() },
                        { column, null, userId.ToString() }
                    }
                    );
                else
                    select = new Select(
                    field,
                    columns,
                    new ColumnsKeysValues<string, string>
                    {
                        KeyValue = new List<ColumnKeyValue<string, string>>
                        {
                            new ColumnKeyValue<string, string>
                            (
                                stateC, "=", state.ToString()
                            )
                        },
                        Connector = "AND",
                        Children = new ColumnsKeysValues<string, string>
                        {
                            KeyValue = new List<ColumnKeyValue<string, string>>
                            {
                                new ColumnKeyValue<string, string>
                                (
                                    column, "=", userId.ToString()
                                ),
                                new ColumnKeyValue<string, string>
                                (
                                    column, "IS", "NULL"
                                )
                            },
                            Connector = "OR",
                        }
                    }
                );

                sentences.Add(select.GetSentence());
            }

            string text = string.Join($" UNION ALL ", sentences);
            bool result = sql.Call(text, "");

            if (result)
            {
                string[] names = new string[] { "new", "old", "closed", "hidden" };
                int counter = 0;
                using IDataReader reader = sql.GetReader();
                while (reader.Read())
                {
                    counters[names[counter]] += (int)reader.GetValue(0);
                    counters["total"] += (int)reader.GetValue(0);
                    counter++;
                }

                sql.Close();
            }

            return counters;
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
                counters = GetCounters(3, solverId, userId, counters);
            }

            return GetCounters(4, ownerId, userId, counters);
        }
        public bool DeleteIncidence(int incidenceId, int userId)
        {
            try
            {

                bool result = sql.Update(
                    incidenceC,
                    new CDictionary<string, string> {
                        { stateC, null, "5" }
                    },
                    sql.WhereOwnerId(userId, sql.WhereIncidenceId(incidenceId))
                );
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int InsertIncidence(int ownerId)
        {
            try
            {
                bool result = sql.Insert(
                    incidenceC,
                    sql.WhereOwnerId(ownerId)
                );
                if (!result) throw new Exception("Parte no insertado");
                return LastIncidence();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IList<Incidence> SelectIncidences(IList<string> fields, ColumnsKeysValues<string, string> conditions = null)
        {
            try
            {
                bool result = sql.Select(new Select(FullIncidence, fields, conditions));
                IList<Incidence> incidences = new List<Incidence>();
                if (result)
                {
                    using (IDataReader reader = sql.GetReader())
                    {
                        while (reader.Read())
                        {
                            Incidence inc = new(
                                (int)reader.GetValue(0),
                                (string)reader.GetValue(2),
                                (int)reader.GetValue(1),
                                (DateTime)reader.GetValue(6),
                                (string)reader.GetValue(3),
                                (int)reader.GetValue(8),
                                reader.GetValue(5) != DBNull.Value ? (string)reader.GetValue(5) : null,
                                reader.GetValue(4) != DBNull.Value ? (int?)reader.GetValue(4) : null,
                                reader.GetValue(7) != DBNull.Value ? (DateTime?)reader.GetValue(7) : null
                            );
                            incidences.Add(inc);
                        }
                        sql.Close();
                    }

                    foreach (Incidence incidence in incidences)
                    {
                        IList<string> list = new List<string>
                        {
                            ALL
                        };
                        CDictionary<string, string> oldConditions = sql.WhereIncidenceId(incidence.Id);
                        result = sql.Select(new Select(incidence_pieces, list, oldConditions));
                        if (result)
                        {
                            IList<Piece> pieces = new List<Piece>();
                            using (IDataReader reader = sql.GetReader())
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
                                sql.Close();
                            }
                            incidence.Pieces = pieces;
                        }
                        oldConditions = sql.WhereNoteType(solverNote, oldConditions);
                        result = sql.Select(new Select(incidence_notes, list, oldConditions));
                        if (result)
                        {
                            IList<Note> notes = new List<Note>();
                            using IDataReader reader = sql.GetReader();
                            while (reader.Read())
                            {
                                Note note = new(
                                    (string)reader.GetValue(1),
                                    (DateTime)reader.GetValue(2)
                                );
                                notes.Add(note);
                            }

                            sql.Close();
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
        public IList<Incidence> SelectIncidences(IList<string> fields, CDictionary<string, string> conditions = null)
        {
            try
            {
                bool result = sql.Select(new Select(FullIncidence, fields, conditions));
                IList<Incidence> incidences = new List<Incidence>();
                if (result)
                {
                    using (IDataReader reader = sql.GetReader())
                    {
                        while (reader.Read())
                        {
                            Incidence inc = new(
                                (int)reader.GetValue(0),
                                (string)reader.GetValue(2),
                                (int)reader.GetValue(1),
                                (DateTime)reader.GetValue(6),
                                (string)reader.GetValue(3),
                                (int)reader.GetValue(8),
                                reader.GetValue(5) != DBNull.Value ? (string)reader.GetValue(5) : null,
                                reader.GetValue(4) != DBNull.Value ? (int?)reader.GetValue(4) : null,
                                reader.GetValue(7) != DBNull.Value ? (DateTime?)reader.GetValue(7) : null
                            );
                            incidences.Add(inc);
                        }
                        sql.Close();
                    }

                    foreach (Incidence incidence in incidences)
                    {
                        IList<string> list = new List<string>
                        {
                            ALL
                        };
                        conditions = sql.WhereIncidenceId(incidence.Id);
                        result = sql.Select(new Select(incidence_pieces, list, conditions));
                        if (result)
                        {
                            IList<Piece> pieces = new List<Piece>();
                            using (IDataReader reader = sql.GetReader())
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
                                sql.Close();
                            }
                            incidence.Pieces = pieces;
                        }
                        conditions = sql.WhereNoteType(solverNote, conditions);
                        result = sql.Select(new Select(incidence_notes, list, conditions));
                        if (result)
                        {
                            IList<Note> notes = new List<Note>();
                            using IDataReader reader = sql.GetReader();
                            while (reader.Read())
                            {
                                Note note = new(
                                    (string)reader.GetValue(1),
                                    (DateTime)reader.GetValue(2)
                                );
                                notes.Add(note);
                            }

                            sql.Close();
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
        //changeState 1-2
        public bool AttendIncidence(IncidenceDto incidence, int incidenceId, int solverID)
        {
            try
            {
                bool result = sql.Update(
                    incidenceC,
                    new CDictionary<string, string>
                    {
                        { solverId, null, solverID.ToString() },
                        { stateC, null, "2" }
                    },
                    sql.WhereId(incidenceId)
                );
                sql.Close();
                if (!result) throw new Exception("Parte no actualizado");
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //changeState 2-3
        public bool CloseIncidence(int incidenceId)
        {
            try
            {
                bool result = sql.Update(
                    incidenceC,
                    new()
                    {
                        { stateC, null, "3" },
                        { close_dateeTime, null, TIMESTAMP }
                    },
                    sql.WhereIncidenceId(incidenceId)
                );
                sql.Close();

                return result;
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
                    columns.Add(stateC, null, incidence.state.ToString());
                }
                else
                {
                    columns = new CDictionary<string, string>();
                    columns.Add(solverId, null, userId.ToString());
                    columns.Add(stateC, null, "2");
                }
                if (close)
                {
                    columns.Add(close_dateeTime, null, TIMESTAMP);
                }
                bool result = sql.Update(
                    incidenceC,
                    columns,
                    sql.WhereIncidenceId(incidenceId)
                );
                sql.Close();
                if (!result) throw new Exception("Parte no actualizado");
                return result;
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
                return SelectIncidences(
                    new List<string> { ALL }, 
                    sql.WhereIncidence(id)
                )[0];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool NewUpdateIncidence(IncidenceDto incidence, int incidenceId)
        {
            try
            {
                    CDictionary<string, string> columns = new();
                    if (incidence.state != null)
                    {
                        columns.Add(stateC, null, incidence.state.ToString());
                    }
                    bool result = sql.Update(
                        incidenceC,
                        columns,
                        sql.WhereIncidenceId(incidenceId)
                    );
                    sql.Close();
                    if (!result) throw new Exception("Parte no actualizado");
                    return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IncidenceList GetIncidencesByStateType(int state, int userId, string type)
        {
            try
            {
                IList<Incidence> own = SelectIncidences(
                    new List<string> { ALL },
                    sql.WhereEmployeeId(
                        userId, 
                        sql.WhereIncidenceState(state)
                    )
                );
                IncidenceList incidences = new(own);
                IList<string> list = new List<string>{
                    "Technician",
                    "Admin"
                };

                if (list.Contains(type) && state != 4)
                {
                    incidences.Other = SelectIncidences(
                        new List<string> { ALL },
                        new ColumnsKeysValues<string, string>
                        {
                            KeyValue = new List<ColumnKeyValue<string, string>>
                            {
                                new ColumnKeyValue<string, string>( stateC, "=", state.ToString() )
                            },
                            Connector = "AND",
                            Children = new ColumnsKeysValues<string, string>
                            {
                                KeyValue = new List<ColumnKeyValue<string, string>>
                                {
                                    new ColumnKeyValue<string, string>(technicianId, "=", userId.ToString()),
                                    new ColumnKeyValue<string, string>( technicianId, "IS", "NULL" )
                                },
                                Connector = "OR"
                            }
                        }
                    );
                }

                return incidences;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
