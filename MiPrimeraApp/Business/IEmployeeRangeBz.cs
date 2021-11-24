using MiPrimeraApp.Models.Employee;
using System.Collections.Generic;

namespace Incidences.Business
{
    public interface IEmployeeRangeBz
    {
        public TypeRange SelectRangeById(int id);
        public int GetEmployeeRangeIdByName(string typeName);
        public IList<TypeRange> GetEmployeeTypes();
    }
}
