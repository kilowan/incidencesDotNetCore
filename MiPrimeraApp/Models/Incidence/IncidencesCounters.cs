namespace Incidences.Models.Incidence
{
    public class IncidencesCounters
    {
        private int newIncidences;
        private int oldIncidences;
        private int closedIncidences;
        private int hiddenIncidences;
        private int totalIncidences;

        public int NewIncidences
        {
            get
            {
                return newIncidences;
            }
            set
            {
                newIncidences = value;
            }
        }
        public int OldIncidences
        {
            get
            {
                return oldIncidences;
            }
            set
            {
                oldIncidences = value;
            }
        }
        public int ClosedIncidences
        {
            get
            {
                return closedIncidences;
            }
            set
            {
                closedIncidences = value;
            }
        }
        public int HiddenIncidences
        {
            get
            {
                return hiddenIncidences;
            }
            set
            {
                hiddenIncidences = value;
            }
        }
        public int TotalIncidences
        {
            get
            {
                return totalIncidences;
            }
            set
            {
                totalIncidences = value;
            }
        }
        public IncidencesCounters()
        {
            this.newIncidences = 0;
            this.oldIncidences = 0;
            this.closedIncidences = 0;
            this.hiddenIncidences = 0;
            this.totalIncidences = 0;
        }
    }
}
