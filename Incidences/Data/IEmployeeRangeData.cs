using Incidences.Models.Employee;
using System.Collections.Generic;

namespace Incidences.Data
{
    public interface IEmployeeRangeData
    {
        public IList<TypeRange> SelectRangeList();
        public int GetEmployeeRangeIdByName(string typeName);
        public TypeRange SelectRangeById(int id);
    }
}
