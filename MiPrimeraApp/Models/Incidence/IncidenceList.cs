using System.Collections.Generic;

namespace MiPrimeraApp.Models.Incidence
{
    public class IncidenceList
    {
        public IList<Incidence> own { get; set; }
        public IList<Incidence> other { get; set; }
        public IncidenceList()
        {
            this.own = new List<Incidence>();
        }
        public IncidenceList(IList<Incidence> own)
        {
            this.own = own;
        }
        public IncidenceList(IList<Incidence> own, IList<Incidence> other)
        {
            this.own = own;
            this.other = other;
        }
    }
}
