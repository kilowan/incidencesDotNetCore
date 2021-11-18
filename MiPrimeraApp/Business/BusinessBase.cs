using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MiPrimeraApp.Business
{
    public class BusinessBase : SqlBase
    {
        public IList<ColumnKeyValue<string, string>> Pushcolumns(string field, string value, IList<ColumnKeyValue<string, string>> columns, string key = null)
        {
            if (!string.IsNullOrEmpty(value)) columns.Add(new ColumnKeyValue<string, string>(field, key, value));
            return columns;
        }

        public CDictionary<string, string> WhereIncidenceId(CDictionary<string, string> conditions, int incidenceId)
        {
            return WhereCommon(conditions, "incidenceId", null, $"{ incidenceId }");
        }
        public CDictionary<string, string> WhereNoteType(CDictionary<string, string> conditions, string noteType)
        {
            return WhereCommon(conditions, "noteType", null, $"{ noteType }");
        }
        public CDictionary<string, string> WhereId_part(CDictionary<string, string> conditions, int incidenceId)
        {
            return WhereCommon(conditions, "id_part", null, $"{ incidenceId }");
        }
        public CDictionary<string, string> WhereIncidence(CDictionary<string, string> conditions, int incidenceId)
        {
            return WhereCommon(conditions, "id", null, $"{ incidenceId }");
        }
        public CDictionary<string, string> WhereIncidenceState(CDictionary<string, string> conditions, int state)
        {
            return WhereCommon(conditions, "state", null, $"{ state }");
        }
        public CDictionary<string, string> WhereNotDeleted(CDictionary<string, string> conditions)
        {
            return WhereCommon(conditions, "deleted", "<>", $"{ 1 }");
        }
        public CDictionary<string, string> WherePieceId(CDictionary<string, string> conditions, IList<int> ids)
        {
            return WhereCommon(conditions, "id", "IN", string.Join(", ", ids));
        }
        public CDictionary<string, string> WhereUsername(CDictionary<string, string> conditions, string username)
        {
            return WhereCommon(conditions, "username", null, $"{ username }");
        }
        public CDictionary<string, string> WhereEmployee(CDictionary<string, string> conditions, int employee)
        {
            return WhereCommon(conditions, "employee", null, $"{ employee }");
        }
        public CDictionary<string, string> WhereEmployeeId(CDictionary<string, string> conditions, int employeeId)
        {
            return WhereCommon(conditions, "employeeId", null, $"{ employeeId }");
        }
        public CDictionary<string, string> WhereTechnicianId(CDictionary<string, string> conditions, int technicianId)
        {
            return WhereCommon(conditions, "technicianId", null, $"{ technicianId }");
        }
        public CDictionary<string, string> WherePassword(CDictionary<string, string> conditions, string password)
        {
            return WhereCommon(conditions, "password", null, GetMD5(password));
        }
        private static CDictionary<string, string> WhereCommon(CDictionary<string, string> conditions, string column, string key, string value)
        {
            conditions.Add(column, key, value);
            return conditions;
        }
        private static string GetMD5(string str)
        {
            MD5 md5 = MD5.Create();
            ASCIIEncoding encoding = new();
            byte[] stream = null;
            StringBuilder sb = new();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
