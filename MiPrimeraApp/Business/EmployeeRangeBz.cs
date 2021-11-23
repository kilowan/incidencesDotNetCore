using Incidences.Business;
using Incidences.Data;
using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;

namespace MiPrimeraApp.Business
{
    public class EmployeeRangeBz : IEmployeeRangeBz
    {
        private IBusinessBase bisba;
        private ISqlBase sql;
        public EmployeeRangeBz(IBusinessBase bisba, ISqlBase sql)
        {
            this.bisba = bisba;
            this.sql = sql;
        }
        #region SELECT
        //new
        public TypeRange SelectRangeById(int id)
        {
            try
            {
                
                return SelectRangeList(
                    this.bisba.WhereId(new CDictionary<string, string>(), id)
                )[0];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int GetEmployeeRangeIdByName(string typeName)
        {
            return (int)SelectRangeList(
                this.bisba.WhereEmployeeTypeName(
                    new CDictionary<string, string>(),
                    typeName
                )
            )[0].id;
        }
        public IList<TypeRange> SelectRangeList(CDictionary<string, string> conditions = null)
        {
            try
            {
                bool result = this.sql.Select(new Select("employee_range", new List<string> { "*" }, conditions));
                if (result)
                {
                    IList<TypeRange> ranges = new List<TypeRange>();
                    using IDataReader reader = this.sql.GetReader();
                    while (reader.Read())
                    {
                        ranges.Add(
                            new TypeRange(
                                (int)reader.GetValue(0),
                                (string)reader.GetValue(1)
                            )
                        );
                    }
                    this.sql.Close();
                    return ranges;
                }
                else throw new Exception("Ningún registro");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
