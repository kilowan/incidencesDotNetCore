using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using MiPrimeraApp.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;

namespace MiPrimeraApp.Business
{
    public class EmployeeRangeBz : BusinessBase
    {
        #region SELECT
        //new
        public TypeRange SelectRangeById(int id)
        {
            try
            {
                return SelectRangeList(
                    new CDictionary<string, string> { 
                        { "id", null, id.ToString() } 
                    }
                )[0];
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        public IList<TypeRange> SelectRangeList(CDictionary<string, string> conditions = null, IDbCommand conexion = null)
            {
                try
                {
                bool result = Select(new Select("employee_range", new List<string> { "*" }, conditions), conexion);
                if (result)
                {
                    IList<TypeRange> ranges = new List<TypeRange>();
                    using IDataReader reader = this.get_sql.ExecuteReader();
                    while (reader.Read())
                    {
                        ranges.Add(
                            new TypeRange(
                                (int)reader.GetValue(0), 
                                (string)reader.GetValue(1)
                            )
                        );
                    }

                    return ranges;
                } else throw new Exception("Ningún registro");
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
