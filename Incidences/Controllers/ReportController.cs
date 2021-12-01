using Incidences.Business;
using Microsoft.AspNetCore.Mvc;
using Incidences.Models;

namespace Incidences.Controllers
{
    [Route("api/Report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportBz rep;
        public ReportController(IReportBz report)
        {
            this.rep = report;
        }
        // GET: ReportController/Details/5
        [HttpGet("{id}")]
        public Report Details(int id)
        {
            return rep.GetReport(id);
        }
    }
}
