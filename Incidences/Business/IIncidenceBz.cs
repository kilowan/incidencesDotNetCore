using Incidences.Models.Incidence;
using System.Collections.Generic;

namespace Incidences.Business
{
    public interface IIncidenceBz
    {
        public IncidenceList GetIncidencesByStateTypeFn(int state, int userId, string type);
        public Incidence GetIncidenceByIdFn(int id);
        public void UpdateIncidenceFn(IncidenceDto incidenceDto, int incidenceId, int userId, bool close);
        public bool UpdateIncidence(IncidenceDto incidence, int incidenceId, int userId, bool close = false);
        public bool NewUpdateIncidence(IncidenceDto incidence, int incidenceId, int userId, bool close = false);
        public bool InsertIncidence(IncidenceDto incidence);
        public bool DeleteIncidenceFn(int incidenceId, int userId);
        public IDictionary<string, int> GetIncidencesCounters(int userId, string type);
    }
}
