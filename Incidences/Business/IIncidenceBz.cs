using Incidences.Models.Incidence;
using System.Collections.Generic;

namespace Incidences.Business
{
    public interface IIncidenceBz
    {
        public IncidenceList GetIncidencesByStateType(int state, int userId, string type);
        public Incidence GetIncidenceById(int id);
        public bool UpdateIncidence(IncidenceDto incidence, int incidenceId, int userId, bool close = false);
        public bool NewUpdateIncidence(IncidenceDto incidence, int incidenceId, int userId, bool close = false);
        public bool InsertIncidence(IncidenceDto incidence);
        public bool DeleteIncidence(int incidenceId, int userId);
        public IDictionary<string, int> GetIncidencesCounters(int userId, string type);
    }
}
