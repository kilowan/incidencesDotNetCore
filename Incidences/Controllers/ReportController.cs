using Incidences.Business;
using Microsoft.AspNetCore.Mvc;
using Incidences.Models;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("{id}")]
        [Authorize]
        public Report Details(int id)
        {
            return rep.GetReport(id);
        }
    }
}
