using Incidences.Business;
using Incidences.Models.Incidence;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraApp.Models.Incidence;
using System.Collections.Generic;

namespace MiPrimeraApp.Controllers
{
    [Route("api/Incidence")]
    [ApiController]
    public class IncidenceController : ControllerBase
    {
        private readonly IIncidenceBz inc;
        public IncidenceController(IIncidenceBz incidence)
        {
            this.inc = incidence;
        }

        [HttpGet("{id}")]
        public Incidence Details(int id)
        {
            return inc.GetIncidenceByIdFn(id);
        }

        [HttpGet("{userId}/{type}")]
        public IDictionary<string, int> Counters(int userId, string type)
        {
            return inc.GetIncidencesCounters(userId, type);
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
