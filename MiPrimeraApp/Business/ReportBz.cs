using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models;
using MiPrimeraApp.Models.Employee;
using MiPrimeraApp.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;

namespace MiPrimeraApp.Business
{
    public class ReportBz : BusinessBase
    {
        #region CONST
        public const int OneMonth = 2592000;
        public const int OneWeek = 604800;
        public const int OneDay = 86400;
        public const int OneHour = 3600;
        public const int OneMinute = 60;
        #endregion
        private EmployeeBz emp;
        public ReportBz()
        {
            this.emp = new EmployeeBz();
        }
        #region SELECT
        //new
        public Report GetReportFn(int userId)
        {
            Employee user = this.emp.SelectEmployeeById(userId)[0];
            Report report = new(SelectReportedPieces(), GetStatisticsFn(userId));
            if (user.type.id == 3) {
                report.global = GetGlobalStatistics();
            }
            return report;
        }
        //new
        private IList<ReportedPiece> SelectReportedPieces()
        {
            try
            {
                IList<ReportedPiece> reportedPieces = new List<ReportedPiece>();
                bool result = Select(new Select("reportedpieces", new List<string> { "*" }));
                if (result)
                {
                    using IDataReader reader = this.get_sql.ExecuteReader();
                    while (reader.Read())
                    {
                        reportedPieces.Add(
                            new ReportedPiece(
                                (string)reader.GetValue(0),
                                (int)reader.GetValue(1)
                            )
                        );
                    }

                }

                return reportedPieces;
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        private IList<Statistics> GetGlobalStatistics()
            {
            try
            {
                int number = 0;
                IList<Statistics> globalData = new List<Statistics>();
                bool result = Select(new Select("Tiempo_resolucion", new List<string> { "ROUND(AVG(Tiempo),0) AS 'tiempo_medio'", "employeeName" }, new Order("solverId")));
                if (result)
                {
                    using IDataReader reader = this.get_sql.ExecuteReader();

                    while (reader.Read())
                    {

                        Statistics globalStatistics = new();
                        globalStatistics.average = SecondsToTimeFn((int)reader.GetValue(0));
                        globalStatistics.employeeName = (string)reader.GetValue(3);
                        globalData[number] = globalStatistics;
                        number++;
                    }
                }

                return globalData;

                } catch (Exception e) {
                throw new Exception(e.Message);
            }
}
        private Statistics GetStatisticsFn(int id)
        {
            try
            {
                Order orderBy = new Order("ROUND(AVG(Tiempo),0)", "DESC");
                bool result = Select(
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
                        orderBy
                    )
                );
                Statistics statistics = new Statistics();
                if (result)
                {
                    using IDataReader reader = this.get_sql.ExecuteReader();
                    reader.Read();
                    statistics.average = SecondsToTimeFn((int)reader.GetValue(0));
                    statistics.solvedIncidences = (int)reader.GetValue(1);
                }
                return statistics;
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        private int SetNumUnitsFn(int seconds)
        {
            return 
                seconds >= OneMonth ? 6 : 
                seconds >= OneWeek ? 5 : 
                seconds >= OneDay ? 4 : 
                seconds >= OneHour ? 3 : 
                seconds >= OneMinute ? 2 : 1;
        }
        private string SecondsToTimeFn(int seconds)
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
