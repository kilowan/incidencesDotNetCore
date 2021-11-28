using Incidences.Data;
using Incidences.Data.Models;
using Incidences.Models;
using Incidences.Models.Employee;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Business
{
    public class ReportBz : IReportBz
    {
        #region CONST
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
        #endregion

        #region private properties
        /// <summary>
        /// Employee dependency
        /// </summary>
        private IEmployeeBz emp;
        /// <summary>
        /// SqlBase dependency
        /// </summary>
        private ISqlBase sql;
        #endregion

        #region constructors
        /// <summary>
        /// Report constructor
        /// </summary>
        /// <param name="employee">employee class</param>
        /// <param name="sqlbase">sqlBase class</param>
        public ReportBz(IEmployeeBz employee, ISqlBase sqlbase)
        {
            this.emp = employee;
            this.sql = sqlbase;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Gets the report
        /// </summary>
        /// <param name="userId">Id param</param>
        /// <returns>Returns report</returns>
        public Report GetReportFn(int userId)
        {
            Employee user = this.emp.SelectEmployeeById(userId)[0];
            Report report = new(SelectReportedPieces(), GetStatisticsFn(userId));
            if (user.Type.Id == 3)
            {
                report.Global = GetGlobalStatistics();
            }
            return report;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Gets the reported pieces
        /// </summary>
        /// <returns>Returns pieces reported</returns>
        private IList<ReportedPiece> SelectReportedPieces()
        {
            try
            {
                IList<ReportedPiece> reportedPieces = new List<ReportedPiece>();
                bool result = this.sql.Select(
                    new Select(
                        "reportedpieces",
                        new List<string> { "*" }
                    )
                );
                if (result)
                {
                    using IDataReader reader = this.sql.GetReader();
                    while (reader.Read())
                    {
                        reportedPieces.Add(
                            new ReportedPiece(
                                (string)reader.GetValue(0),
                                (int)reader.GetValue(1)
                            )
                        );
                    }
                    this.sql.Close();
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
        private IList<Statistics> GetGlobalStatistics()
        {
            try
            {
                IList<Statistics> globalData = new List<Statistics>();
                bool result = this.sql.Select(
                    new Select(
                        "Tiempo_resolucion",
                        new List<string> {
                            "ROUND(AVG(Tiempo),0) AS 'tiempo_medio'",
                            "employeeName"
                        },
                        new List<string> { "employeeName, solverId" },
                        new Order("solverId")
                    )
                );
                if (result)
                {
                    using IDataReader reader = this.sql.GetReader();

                    while (reader.Read())
                    {
                        Statistics globalStatistics = new();
                        globalStatistics.Average = SecondsToTimeFn((int)reader.GetValue(0));
                        globalStatistics.EmployeeName = (string)reader.GetValue(3);
                        globalData.Add(globalStatistics);
                    }
                }

                this.sql.Close();
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
        private Statistics GetStatisticsFn(int id)
        {
            try
            {
                bool result = this.sql.Select(
                    new Select(
                        "Tiempo_resolucion",
                        new List<string> {
                            "ROUND(AVG(Tiempo),0) AS 'tiempo_medio'",
                            "count(solverId) AS 'cantidad_partes'",
                            "employeeName"
                        },
                        new CDictionary<string, string> {
                            { "solverId", null, id.ToString() }
                        },
                        new List<string> {
                            "employeeName"
                        },
                        new Order(
                            "ROUND(AVG(Tiempo),0)",
                            "DESC"
                        )
                    )
                );
                Statistics statistics = new();
                if (result)
                {
                    using IDataReader reader = this.sql.GetReader();
                    reader.Read();
                    statistics.Average = SecondsToTimeFn((int)reader.GetValue(0));
                    statistics.SolvedIncidences = (int)reader.GetValue(1);
                }

                this.sql.Close();
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
        private static int SetNumUnitsFn(int seconds)
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
        private static string SecondsToTimeFn(int seconds)
        {
            int num_units = SetNumUnitsFn(seconds);
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
        #endregion
    }
}
