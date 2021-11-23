using MiPrimeraApp.Models.Incidence;
using System.Collections.Generic;

namespace MiPrimeraApp.Models
{
    public class Report
    {
        private IList<ReportedPiece> reported;
        private IList<Statistics> global;
        private Statistics statistics;

        public IList<ReportedPiece> Reported 
        {
            get 
            { 
                return reported; 
            }
            set 
            { 
                reported = value; 
            }
        }
        public IList<Statistics> Global
        {
            get
            {
                return global;
            }
            set
            {
                global = value;
            }
        }
        public Statistics Statistics
        {
            get
            {
                return statistics;
            }
            set
            {
                statistics = value;
            }
        }

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
