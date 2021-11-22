using MiPrimeraApp.Models.Employee;

namespace Incidences.Business
{
    public interface IEmployeeRangeBz
    {
        #region SELECT
        public TypeRange SelectRangeById(int id);
        public int GetEmployeeRangeIdByName(string typeName);
        #endregion
    }
}
