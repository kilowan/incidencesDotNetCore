using Incidences.Models.Incidence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraApp.Business;
using MiPrimeraApp.Models.Incidence;

namespace MiPrimeraApp.Controllers
{
    [Route("api/Incidence")]
    [ApiController]
    public class IncidenceController : ControllerBase
    {
        private IncidenceBz inc;
        public IncidenceController()
        {
            this.inc = new();
        }

        [HttpGet("{id}")]
        public Incidence Details(int id)
        {
            return inc.GetIncidenceByIdFn(id);
        }

        [HttpGet("{state}/{userId}/{Type}")]
        public IncidenceList List(int state, int userId, string Type)
        {
            return inc.GetIncidencesByStateTypeFn(state, userId, Type);
        }

        [HttpPost]
        public bool Create(IncidenceDto incidence)
        {
            return inc.InsertIncidence(incidence);
        }

        [HttpPut("{incidenceId}/{userId}/{close}")]
        public bool Update(IncidenceDto incidence, int incidenceId, int userId, bool close)
        {
            return inc.UpdateIncidence(incidence, incidenceId, userId, close);
        }

        [HttpPut("{incidenceId}/{userId}")]
        public bool Update(IncidenceDto incidence, int incidenceId, int userId)
        {
            return inc.UpdateIncidence(incidence, incidenceId, userId);
        }

        [HttpDelete("{incidenceId}/{userId}")]
        public bool Delete(int incidenceId, int userId)
        {
            return inc.DeleteIncidenceFn(incidenceId, userId);
        }
    }
}
