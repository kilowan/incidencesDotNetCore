using Microsoft.AspNetCore.Mvc;
using MiPrimeraApp.Business;
using MiPrimeraApp.Models;

namespace MiPrimeraApp.Controllers
{
    [Route("api/Report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private ReportBz rep;
        // GET: ReportController/Details/5
        [HttpGet("{id}")]
        public Report Details(int id)
        {
            rep = new ReportBz();
            return rep.GetReportFn(id);
        }
    }
}
