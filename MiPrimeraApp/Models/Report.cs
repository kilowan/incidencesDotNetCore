using MiPrimeraApp.Models.Incidence;
using System.Collections.Generic;

namespace MiPrimeraApp.Models
{
    public class Report
    {
        public IList<ReportedPiece> reported;
        public IList<Statistics> global;
        public Statistics statistics;
        public Report(IList<ReportedPiece> reported, Statistics statistics)
        {
            this.reported = reported;
            this.statistics = statistics;
        }
        public Report(IList<ReportedPiece> reported, Statistics statistics, IList<Statistics> global)
        {
            this.reported = reported;
            this.statistics = statistics;
            this.global = global;
        }
    }
}
