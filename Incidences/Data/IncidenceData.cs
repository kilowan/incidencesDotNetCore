using Incidences.Data.Models;
using Incidences.Models.Incidence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Incidences.Data
{
    public class IncidenceData : IIncidenceData
    {
        private readonly IncidenceContext _context;

        public IncidenceData(IncidenceContext context)
        {
            _context = context;
        }
        public int Count(int state, int userId) 
        {
            return _context.Incidences
                .Where(inc => inc.state == state && inc.solverId == userId)
                .Count();
        }
        public int LastIncidence()
        {
            try
            {
                return _context.Incidences
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
                    counters[names[i]] += Count(i, userId);
                }
            }

            for (int i = 1; i <= 4; i++)
            {
                counters[names[i]] += Count(i, userId);
            }

            return counters;
        }
        public bool DeleteIncidence(int incidenceId, int userId)
        {
            try
            {

                incidence inc = _context.Incidences
                    .Where(inc => inc.id == incidenceId && inc.ownerId == userId)
                    .FirstOrDefault();
                inc.state = 5;

                _context.Incidences.Update(inc);
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
                _context.Incidences.Add(new()
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
        public bool AddIncidence(IncidenceDto incidence) 
        {
            try
            {
                _context.Database.BeginTransaction();
                incidence inc = new()
                {
                    ownerId = incidence.ownerId,
                    open_dateTime = DateTime.Now
                };
                _context.Incidences.Add(inc);

                if (_context.SaveChanges() != 1) throw new Exception("Incidencia no insertada");
                _context.Notess.Add(new Notes()
                {
                    noteStr = incidence.note,
                    noteTypeId = 1,
                    employeeId = incidence.ownerId,
                    incidenceId = inc.id
                });

                if (_context.SaveChanges() != 1) throw new Exception("Incidencia no insertada");
                IList<incidence_piece_log> pieces = new List<incidence_piece_log>();
                foreach (int piece in incidence.piecesAdded)
                {
                    pieces.Add(new incidence_piece_log()
                    {
                        pieceId = piece,
                        incidenceId = inc.id
                    });
                }

                _context.IncidencePieceLogs.AddRange(pieces);
                if (_context.SaveChanges() != 1) throw new Exception("Incidencia no insertada");

                _context.Database.CommitTransaction();

                return true;
            }
            catch (Exception e)
            {
                _context.Database.RollbackTransaction();
                throw new Exception(e.Message);
            }
        }
        //changeState 1-2
        public bool AttendIncidence(IncidenceDto incidence, int incidenceId, int solverID)
        {
            try
            {
                incidence inc = _context.Incidences
                    .Where(inc => inc.id == incidenceId)
                    .FirstOrDefault();
                inc.solverId = solverID;
                inc.state = 2;
                _context.Incidences.Update(inc);
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
                incidence inc = _context.Incidences
                    .Where(inc => inc.id == incidenceId)
                    .FirstOrDefault();
                inc.state = 3;
                _context.Incidences.Update(inc);
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
                incidence inc = _context.Incidences
                    .Where(inc => inc.id == incidenceId)
                    .FirstOrDefault();

                if (incidence.state != null) inc.state = (int)incidence.state;
                else
                {
                    inc.solverId = userId;
                    inc.state = 2;
                }

                if (close) inc.close_dateTime = DateTime.Now;

                _context.Incidences.Update(inc);
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
                incidence inc = _context.Incidences
                    .Include(inc => inc.notes)
                    .ThenInclude(noty => noty.NoteType)
                    .Include(inc => inc.solver)
                    .ThenInclude(solty => solty.EmployeeRange)
                    .Include(inc => inc.owner)
                    .ThenInclude(ownty => ownty.EmployeeRange)
                    .Include(inc => inc.pieces)
                    .ThenInclude(ipl => ipl.Piece)
                    .ThenInclude(pie => pie.PieceType)
                    .Where(inc => inc.id == id)
                    .FirstOrDefault();

                if (inc != null)
                {
                    return new Incidence(inc);
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
                incidence inc = _context.Incidences
                    .Where(inc => inc.id == incidenceId)
                    .FirstOrDefault();

                if (incidence.state != null) inc.state = (int)incidence.state;

                _context.Incidences.Update(inc);
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
                IList<incidence> incidencesOwn = _context.Incidences
                    .Include(inc => inc.notes)
                    .ThenInclude(noty => noty.NoteType)
                    .Include(inc => inc.solver)
                    .ThenInclude(solty => solty.EmployeeRange)
                    .Include(inc => inc.owner)
                    .ThenInclude(ownty => ownty.EmployeeRange)
                    .Include(inc => inc.pieces)
                    .ThenInclude(ipl => ipl.Piece)
                    .ThenInclude(pie => pie.PieceType)
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
                    IList<incidence> incidencesOther = _context.Incidences
                        .Include(inc => inc.notes)
                        .ThenInclude(noty => noty.NoteType)
                        .Include(inc => inc.solver)
                        .ThenInclude(solty => solty.EmployeeRange)
                        .Include(inc => inc.owner)
                        .ThenInclude(ownty => ownty.EmployeeRange)
                        .Include(inc => inc.pieces)
                        .ThenInclude(ipl => ipl.Piece)
                        .ThenInclude(pie => pie.PieceType)
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

        private static IList<Incidence> ConvertIncidences(IList<incidence> incidences) 
        {
            IList<Incidence> incidencesOut = new List<Incidence>();
            foreach (incidence inc in incidences)
            {
                incidencesOut.Add(new Incidence(inc));
            }
            return incidencesOut;
        }
    }
}
