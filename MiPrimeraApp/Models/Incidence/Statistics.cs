namespace Incidences.Models.Incidence
{
    public class Statistics
    {
        private string employeeName;
        private string average;
        private int solvedIncidences;

        public string EmployeeName
        {
            get
            {
                return employeeName;
            }
            set
            {
                employeeName = value;
            }
        }
        public string Average
        {
            get
            {
                return average;
            }
            set
            {
                average = value;
            }
        }
        public int SolvedIncidences
        {
            get
            {
                return solvedIncidences;
            }
            set
            {
                solvedIncidences = value;
            }
        }
    }
}
