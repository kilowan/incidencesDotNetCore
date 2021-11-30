using Incidences.Data;
using Incidences.Data.Models;
using Incidences.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Business
{
    public class EmployeeRangeBz : IEmployeeRangeBz
    {
        private readonly IEmployeeRangeData employeeRangeData;
        public EmployeeRangeBz(IEmployeeRangeData employeeRangeData)
        {
            this.employeeRangeData = employeeRangeData;
        }
        #region SELECT
        public IList<TypeRange> GetEmployeeTypes()
        {
            try
            {
                return employeeRangeData.SelectRangeList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public TypeRange SelectRangeById(int id)
        {
            try
            {

                return employeeRangeData.SelectRangeById(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int GetEmployeeRangeIdByName(string typeName)
        {
            return employeeRangeData.GetEmployeeRangeIdByName(typeName);
        }
        #endregion
    }
}
