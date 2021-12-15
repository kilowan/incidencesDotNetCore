﻿using Incidences.Business;
using Incidences.Models.Incidence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Incidences.Controllers
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
        [Authorize]
        public Incidence Details(int id)
        {
            return inc.GetIncidenceById(id);
        }

        [HttpGet("{userId}/{type}")]
        [Authorize]
        public IDictionary<string, int> Counters(int userId, string type)
        {
            return inc.GetIncidencesCounters(userId, type);
        }

        [HttpGet("{state}/{userId}/{Type}")]
        [Authorize]
        public IncidenceList List(int state, int userId, string Type)
        {
            return inc.GetIncidencesByStateType(state, userId, Type);
        }

        [HttpPost]
        [Authorize]
        public bool Create(IncidenceDto incidence)
        {
            return inc.InsertIncidence(incidence);
        }

        [HttpPut("{incidenceId}/{userId}/{close}")]
        [Authorize]
        public bool Update(IncidenceDto incidence, int incidenceId, int userId, bool close)
        {
            return inc.UpdateIncidence(incidence, incidenceId, userId, close);
        }

        [HttpPut("{incidenceId}/{userId}")]
        [Authorize]
        public bool Update(IncidenceDto incidence, int incidenceId, int userId)
        {
            return inc.NewUpdateIncidence(incidence, incidenceId, userId);
        }

        [HttpDelete("{incidenceId}/{userId}")]
        [Authorize]
        public bool Delete(int incidenceId, int userId)
        {
            return inc.DeleteIncidence(incidenceId, userId);
        }
    }
}
