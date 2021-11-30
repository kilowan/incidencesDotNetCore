using Incidences.Data;
using Incidences.Models;
using Incidences.Models.Employee;

namespace Incidences.Business
{
    public class ReportBz : IReportBz
    {
        #region private properties
        /// <summary>
        /// Employee dependency
        /// </summary>
        private readonly IEmployeeBz emp;
        private readonly IReportData reportData;
        #endregion

        #region constructors
        /// <summary>
        /// Report constructor
        /// </summary>
        /// <param name="employee">employee class</param>
        public ReportBz(IEmployeeBz employee, IReportData reportData)
        {
            this.emp = employee;
            this.reportData = reportData;
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
            Report report = new(reportData.SelectReportedPieces(), reportData.GetStatisticsFn(userId));
            if (user.Type.Id == 3)
            {
                report.Global = reportData.GetGlobalStatistics();
            }
            return report;
        }
        #endregion
    }
}
