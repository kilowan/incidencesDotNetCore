using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data
{
    public interface IReportData
    {
        /// <summary>
        /// Gets the reported pieces
        /// </summary>
        /// <returns>Returns pieces reported</returns>
        public IList<ReportedPiece> SelectReportedPieces();
        /// <summary>
        /// Gets global statistics
        /// </summary>
        /// <returns>Returns global statistics</returns>
        public IList<Statistics> GetGlobalStatistics();
        /// <summary>
        /// Gets the own statistics
        /// </summary>
        /// <param name="id">own employeeId</param>
        /// <returns>Returns own statistics</returns>
        public Statistics GetStatistics(int id);
    }
}
