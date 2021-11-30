using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System.Collections.Generic;

namespace Incidences.Data
{
    public interface IIncidenceData
    {
        public int LastIncidence(IList<string> fields);
        public int LastIncidence();
        public IDictionary<string, int> GetCounters(int state, string column, int userId, IDictionary<string, int> counters);
        public IDictionary<string, int> GetIncidencesCounters(int userId, string type);
        public bool DeleteIncidenceFn(int incidenceId, int userId);
        public int InsertIncidence(int ownerId);
        public bool CloseIncidence(int incidenceId);
        public bool AttendIncidence(IncidenceDto incidence, int incidenceId, int solverID);
        public bool UpdateIncidence(IncidenceDto incidence, int incidenceId, int userId, bool close = false);
        public void UpdateIncidenceFn(IncidenceDto incidenceDto, int incidenceId, int userId, bool close);
        public Incidence GetIncidenceByIdFn(int id);
        public bool NewUpdateIncidence(IncidenceDto incidence, int incidenceId);
        public IncidenceList GetIncidencesByStateTypeFn(int state, int userId, string type);
    }
}
