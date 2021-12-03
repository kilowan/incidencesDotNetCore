using Incidences.Data.Models;
using Incidences.Models.Employee;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Incidences.Data
{
    public class IncidenceData : IIncidenceData
    {
        private readonly IncidenceContext _context;
        private readonly INoteData noteData;
        private readonly IPieceData pieceData;
        private readonly IEmployeeData employeeData;

        public IncidenceData(
            IncidenceContext context, 
            INoteData noteData, 
            IPieceData pieceData,
            IEmployeeData employeeData
            )
        {
            _context = context;
            this.noteData = noteData;
            this.pieceData = pieceData;
            this.employeeData = employeeData;
        }

        public int LastIncidence()
        {
            try
            {
                return _context.Incidence
                    .Select(inc => inc.id)
                    .Max();
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
            IList<string> names = new List<string>()
            {
                "new",
                "old",
                "closed",
                "hidden",
                "total"
            };
            IList<string> types = new List<string> { "Technician", "Admin" };
            if (types.Contains(type))
            {
                //Technician or Admin
                for (int i = 1; i <= 3; i++)
                {
                    counters[names[i]] += _context.Incidence
                        .Where(inc => inc.state == i && inc.solverId == userId)
                        .Count();
                }
            }

            for (int i = 1; i <= 4; i++)
            {
                counters[names[i]] += _context.Incidence
                    .Where(inc => inc.state == i && inc.solverId == userId)
                    .Count();
            }

            return counters;
        }
        public bool DeleteIncidence(int incidenceId, int userId)
        {
            try
            {

                incidence inc = _context.Incidence
                    .Where(inc => inc.id == incidenceId && inc.ownerId == userId)
                    .FirstOrDefault();
                inc.state = 5;

                _context.Incidence.Update(inc);
                if (_context.SaveChanges() != 1) throw new Exception("Empleado no insertado");
                return true;
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
                _context.Incidence.Add(new()
                {
                    ownerId = ownerId,
                });

                if (_context.SaveChanges() != 1) throw new Exception("Incidencia no insertada");

                return LastIncidence();

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
                incidence inc = _context.Incidence
                    .Where(inc => inc.id == incidenceId)
                    .FirstOrDefault();
                inc.solverId = solverID;
                inc.state = 2;
                _context.Incidence.Update(inc);
                if (_context.SaveChanges() != 1) throw new Exception("Incidencia no atendida");

                return true;
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
                incidence inc = _context.Incidence
                    .Where(inc => inc.id == incidenceId)
                    .FirstOrDefault();
                inc.state = 3;
                _context.Incidence.Update(inc);
                if (_context.SaveChanges() != 1) throw new Exception("Incidencia no atendida");

                return true;
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
                incidence inc = _context.Incidence
                    .Where(inc => inc.id == incidenceId)
                    .FirstOrDefault();

                if (incidence.state != null) inc.state = (int)incidence.state;
                else
                {
                    inc.solverId = userId;
                    inc.state = 2;
                }

                if (close) inc.close_dateTime = DateTime.Now;

                _context.Incidence.Update(inc);
                if (_context.SaveChanges() != 1) throw new Exception("Incidencia no actualizada");
                return true;
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
                incidence inc = _context.Incidence
                    .Where(inc => inc.id == id)
                    .FirstOrDefault();
                if (inc != null)
                {
                    return ConvertIncidence(inc);
                }
                else return new Incidence();
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
                incidence inc = _context.Incidence
                    .Where(inc => inc.id == incidenceId)
                    .FirstOrDefault();

                if (incidence.state != null) inc.state = (int)incidence.state;

                _context.Incidence.Update(inc);
                if (_context.SaveChanges() != 1) throw new Exception("Incidencia no actualizada");
                return true;
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
                IList<incidence> incidencesOwn = _context.Incidence
                    .Where(inc => inc.state == state && inc.ownerId == userId)
                    .ToList();

                IList<Incidence> own = ConvertIncidences(incidencesOwn);
                IncidenceList incidences = new(own);
                IList<string> list = new List<string>{
                    "Technician",
                    "Admin"
                };

                if (list.Contains(type) && state != 4)
                {
                    IList<incidence> incidencesOther = _context.Incidence
                        .Where(inc => inc.state == state && inc.solverId == userId)
                        .ToList();

                    incidences.Other = ConvertIncidences(incidencesOther);
                }

                return incidences;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private IList<Incidence> ConvertIncidences(IList<incidence> incidences) 
        {
            IList<Incidence> incidencesOut = new List<Incidence>();
            foreach (incidence inc in incidences)
            {
                incidencesOut.Add(ConvertIncidence(inc));
            }
            return incidencesOut;
        }
        private Incidence ConvertIncidence(incidence incidence) 
        {
            Note employeeNote = noteData.SelectEmployeeNoteByIncidenceId(incidence.id);
            IList<Piece> pieces = pieceData.GetPiecesByIncidenceId(incidence.id);
            Employee owner = employeeData.SelectEmployeeById(incidence.ownerId);
            if (incidence.state == 2)
            {
                IList<Note> notes = noteData.SelectNotesByIncidenceId(incidence.id);
                Employee solver = employeeData.SelectEmployeeById((int)incidence.solverId);
                return new Incidence(
                    incidence.id,
                    incidence.state,
                    owner.FullName,
                    incidence.ownerId,
                    incidence.open_dateTime,
                    employeeNote.NoteStr,
                    pieces,
                    notes,
                    solver.FullName,
                    incidence.solverId,
                    (DateTime)incidence.close_dateTime
                );
            }
            
            return new Incidence(
                incidence.id,
                incidence.state,
                owner.FullName,
                incidence.ownerId,
                incidence.open_dateTime,
                employeeNote.NoteStr,
                pieces
            );
        }
    }
}
