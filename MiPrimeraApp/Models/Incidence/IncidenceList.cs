using System.Collections.Generic;

namespace Incidences.Models.Incidence
{
    public class IncidenceList
    {
        private IList<Incidence> own;
        private IList<Incidence> other;

        public IList<Incidence> Own
        {
            get
            {
                return own;
            }
            set
            {
                own = value;
            }
        }
        public IList<Incidence> Other
        {
            get
            {
                return other;
            }
            set
            {
                other = value;
            }
        }
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
