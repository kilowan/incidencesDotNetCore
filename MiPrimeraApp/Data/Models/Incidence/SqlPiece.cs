using MiPrimeraApp.Data.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
