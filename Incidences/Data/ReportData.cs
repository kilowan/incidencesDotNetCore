using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Incidences.Data
{
    public class ReportData : IReportData
    {
        /// <summary>
        /// One mounth in seconds
        /// </summary>
        public const int OneMonth = 2592000;
        /// <summary>
        /// One week in seconds
        /// </summary>
        public const int OneWeek = 604800;
        /// <summary>
        /// One day in seconds
        /// </summary>
        public const int OneDay = 86400;
        /// <summary>
        /// One hour in seconds
        /// </summary>
        public const int OneHour = 3600;
        /// <summary>
        /// One minute in seconds
        /// </summary>
        public const int OneMinute = 60;

        #region private properties
        /// <summary>
        /// Employee dependency
        /// </summary>
        //private IEmployeeBz emp;
        /// <summary>
        /// SqlBase dependency
        /// </summary>
        private readonly IncidenceContext _context;
        #endregion
        public ReportData(IncidenceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the reported pieces
        /// </summary>
        /// <returns>Returns pieces reported</returns>
        public IList<ReportedPiece> SelectReportedPieces()
        {
            try
            {
                IList<ReportedPiece> reportedPieces = new List<ReportedPiece>();
                IList<int> states = new List<int> { 3, 4 };
                IList<int> incidenceIds = _context.Incidences
                    .Where(inc => states.Contains(inc.state))
                    .Select(inc => inc.id)
                    .ToList();
                if (incidenceIds.Count >0)
                {
                    IList<int> pieceIds = _context.IncidencePieceLogs
                        .Where(ipl => incidenceIds.Contains(ipl.incidenceId))
                        .Select(inc => inc.pieceId)
                        .ToList();
                    if (pieceIds.Count > 0)
                    {
                        piece_class[] pieces = _context.PieceClasss
                            .Where(pie => pieceIds.Contains(pie.id))
                            .ToArray();

                        foreach (piece_class piece in pieces)
                        {
                            reportedPieces.Add(new(piece.name, piece.id));
                        }
                    }
                }

                return reportedPieces;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Gets global statistics
        /// </summary>
        /// <returns>Returns global statistics</returns>
        public IList<Statistics> GetGlobalStatistics()
        {
            try
            {
                int incidences = _context.Incidences
                    .Where(inc => inc.solverId != null)
                    .Select(inc => inc.id)
                    .Count();

                IList <Statistics> globalData = new List<Statistics>();
                if (incidences > 0)
                {
                    IList<int> employeeId = _context.Incidences
                        .Select(inc => (int)inc.solverId)
                        .ToList();

                    employee[] employees = _context.Employees
                        .Where(emp => employeeId.Contains(emp.id))
                        .ToArray();
                    foreach (employee emp in employees)
                    {
                        Statistics globalStatistics = new();
                        incidence[] emp_inc = _context.Incidences
                            .Where(inc => inc.solverId == emp.id)
                            .ToArray();
                        double dtime = 0;

                        foreach (incidence inc in emp_inc)
                        {
                            dtime += CalculateTime(inc.open_dateTime, (DateTime)inc.close_dateTime);
                        }

                        int time = (int)Math.Truncate(Math.Round(dtime / emp_inc.Length));
                        if (time > 0)
                        {
                            globalStatistics.Average = SecondsToTime(time);
                            globalStatistics.EmployeeName = $"{emp.name} {emp.surname1} {emp.surname2}";
                        }

                        globalData.Add(globalStatistics);
                    }
                }

                return globalData;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Gets the own statistics
        /// </summary>
        /// <param name="id">own employeeId</param>
        /// <returns>Returns own statistics</returns>
        public Statistics GetStatistics(int id)
        {
            try
            {
                incidence[] incidences = _context.Incidences
                    .Where(inc => inc.solverId == id)
                    .ToArray();
                Statistics statistics = new();
                if (incidences.Length > 0)
                {
                    employee emp = _context.Employees
                        .Where(emp => emp.id == id)
                        .FirstOrDefault();
                    double dtime = 0;
                    foreach (incidence inc in incidences)
                    {
                        dtime += CalculateTime(inc.open_dateTime, (DateTime)inc.close_dateTime);
                    }
                    int time = (int)Math.Truncate(Math.Round(dtime / incidences.Length));
                    if (time > 0)
                    {
                        statistics.Average = SecondsToTime(time);
                        statistics.SolvedIncidences = incidences.Length;
                    }
                }

                return statistics;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Gets unit's number
        /// </summary>
        /// <param name="seconds">seconds param</param>
        /// <returns>Returns number</returns>
        private static int SetNumUnits(int seconds)
        {
            return
                seconds >= OneMonth ? 6 :
                seconds >= OneWeek ? 5 :
                seconds >= OneDay ? 4 :
                seconds >= OneHour ? 3 :
                seconds >= OneMinute ? 2 : 1;
        }
        /// <summary>
        /// Gets the time in string
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns>Returns string time</returns>
        private static string SecondsToTime(int seconds)
        {
            int num_units = SetNumUnits(seconds);
            decimal mounths = seconds / OneMonth;
            decimal weeks = seconds % OneMonth / OneWeek;
            decimal days = seconds % OneMonth / OneDay;
            decimal hours = seconds % OneDay / OneHour;
            decimal minutes = seconds % OneHour / OneMinute;
            decimal rest = seconds % OneMinute;
            IDictionary<string, decimal> time_descr = new Dictionary<string, decimal> {
            { "meses", Math.Floor(mounths) },
            { "semanas", Math.Floor(weeks) },
            { "días", Math.Floor(days) },
            { "horas", Math.Floor(hours) },
            { "minutos", Math.Floor(minutes) },
            { "segundos", Math.Floor(rest) },
        };
            string res = string.Empty;
            int counter = 0;
            foreach (var k in time_descr)
            {
                res += k.Value + " " + k.Key;
                counter++;
            }

            return res;
        }
        private static double CalculateTime(DateTime before, DateTime after)
        {
            return after.Subtract(DateTime.MinValue).TotalSeconds - before.Subtract(DateTime.MinValue).TotalSeconds;
        }
    }
}
