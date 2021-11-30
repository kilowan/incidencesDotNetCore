﻿using Incidences.Data.Models;
using Incidences.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Data
{
    public class EmployeeRangeData : IEmployeeRangeData
    {
        private const string employee_range = "employee_range";
        private const string ALL = "*";

        private readonly ISqlBase sql;

        public EmployeeRangeData(ISqlBase sql)
        {
            this.sql = sql;
        }

        public IList<TypeRange> SelectRangeList(CDictionary<string, string> conditions = null)
        {
            try
            {
                bool result = this.sql.Select(new Select(employee_range, new List<string> { ALL }, conditions));
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
        public TypeRange SelectRangeById(int id)
        {
            try
            {

                return SelectRangeList(
                    this.sql.WhereId(new CDictionary<string, string>(), id)
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
                sql.WhereEmployeeTypeName(
                    new CDictionary<string, string>(),
                    typeName
                )
            )[0].Id;
        }
    }
}
