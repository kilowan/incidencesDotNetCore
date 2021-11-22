using MiPrimeraApp.Data.Models.Employee;

namespace MiPrimeraApp.Data.Models.Incidence
{
    public class SqlPiece : SqlEmployeeRange
    {
        public int? employee;
        public int? incidence;
        public int? noteType;
        public string noteStr;
        public string date;
    }
}
