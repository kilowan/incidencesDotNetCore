using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MiPrimeraApp.Business
{
    public class BusinessBase : SqlBase
    {
        protected IList<ColumnKeyValue<string, string>> Pushcolumns(string field, string value, IList<ColumnKeyValue<string, string>> columns, string key = null)
        {
            if (!string.IsNullOrEmpty(value)) columns.Add(new ColumnKeyValue<string, string>(field, key, value));
            return columns;
        }

        protected CDictionary<string, string> WhereIncidenceId(CDictionary<string, string> conditions, int incidenceId)
        {
            return WhereCommon(conditions, "incidenceId", null, $"{ incidenceId }");
        }
        protected CDictionary<string, string> WhereNoteType(CDictionary<string, string> conditions, string noteType)
        {
            return WhereCommon(conditions, "noteType", null, $"{ noteType }");
        }
        protected CDictionary<string, string> WhereOwnerId(CDictionary<string, string> conditions, int ownerId)
        {
            return WhereCommon(conditions, "ownerId", null, $"{ ownerId }");
        }
        protected CDictionary<string, string> WhereSolverId(CDictionary<string, string> conditions, int solverId)
        {
            return WhereCommon(conditions, "solverId", null, $"{ solverId }");
        }
        protected CDictionary<string, string> WhereEmployeeTypeName(CDictionary<string, string> conditions, string typeName) 
        {
            return WhereCommon(conditions, "name", null, $"{ typeName }");
        }
        protected CDictionary<string, string> WhereIncidence(CDictionary<string, string> conditions, int incidenceId)
        {
            return WhereCommon(conditions, "id", null, $"{ incidenceId }");
        }
        protected CDictionary<string, string> WhereIncidenceState(CDictionary<string, string> conditions, int state)
        {
            return WhereCommon(conditions, "state", null, $"{ state }");
        }
        protected CDictionary<string, string> WhereNotDeleted(CDictionary<string, string> conditions)
        {
            return WhereCommon(conditions, "deleted", "<>", $"{ 1 }");
        }
        protected CDictionary<string, string> WherePieceId(CDictionary<string, string> conditions, int? id)
        {
            return WhereCommon(conditions, "id", "=", id.ToString());
        }
        protected CDictionary<string, string> WherePieceId(CDictionary<string, string> conditions, IList<int> ids)
        {
            return WhereCommon(conditions, "id", "IN", string.Join(", ", ids));
        }
        protected CDictionary<string, string> WhereUsername(CDictionary<string, string> conditions, string username)
        {
            return WhereCommon(conditions, "username", null, $"{ username }");
        }
        protected CDictionary<string, string> WhereEmployee(CDictionary<string, string> conditions, int employee)
        {
            return WhereCommon(conditions, "employee", null, $"{ employee }");
        }
        protected CDictionary<string, string> WhereEmployeeId(CDictionary<string, string> conditions, int employeeId)
        {
            return WhereCommon(conditions, "employeeId", null, $"{ employeeId }");
        }
        protected CDictionary<string, string> WhereTechnicianId(CDictionary<string, string> conditions, int technicianId)
        {
            return WhereCommon(conditions, "technicianId", null, $"{ technicianId }");
        }
        protected CDictionary<string, string> WherePassword(CDictionary<string, string> conditions, string password)
        {
            return WhereCommon(conditions, "password", null, GetMD5(password));
        }
        protected static CDictionary<string, string> WhereCommon(CDictionary<string, string> conditions, string column, string key, string value)
        {
            conditions.Add(column, key, value);
            return conditions;
        }
        protected static string GetMD5(string str)
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
