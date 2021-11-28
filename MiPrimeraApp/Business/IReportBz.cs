using Incidences.Models;

namespace Incidences.Business
{
    public interface IReportBz
    {
        /// <summary>
        /// Gets the report
        /// </summary>
        /// <param name="userId">Id param</param>
        /// <returns>Returns report</returns>
        public Report GetReportFn(int userId);
    }
}
