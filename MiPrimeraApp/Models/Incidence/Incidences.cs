using System.Collections.Generic;

namespace MiPrimeraApp.Models.Incidence
{
    public class Incidences
    {
        public IList<Incidence> own;
		public IList<Incidence> other;
        public Incidences()
        {
            this.own = new List<Incidence>();
        }
        public Incidences(IList<Incidence> own)
        {
            this.own = own;
        }
        public Incidences(IList<Incidence> own, IList<Incidence> other)
        {
            this.own = own;
            this.other = other;
        }
    }
}
