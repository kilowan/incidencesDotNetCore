using MiPrimeraApp.Data;
using MiPrimeraApp.Data.Models;
using System.Collections.Generic;

namespace Incidences.Business
{
    public interface IBusinessBase
    {
        public IList<ColumnKeyValue<string, string>> Pushcolumns(string field, string value, IList<ColumnKeyValue<string, string>> columns, string key = null);

        public CDictionary<string, string> WhereIncidenceId(CDictionary<string, string> conditions, int incidenceId);
        public CDictionary<string, string> WhereNoteType(CDictionary<string, string> conditions, string noteType);
        public CDictionary<string, string> WhereOwnerId(CDictionary<string, string> conditions, int ownerId);
        public CDictionary<string, string> WhereSolverId(CDictionary<string, string> conditions, int solverId);
        public CDictionary<string, string> WhereEmployeeTypeName(CDictionary<string, string> conditions, string typeName);
        public CDictionary<string, string> WhereIncidence(CDictionary<string, string> conditions, int incidenceId);
        public CDictionary<string, string> WhereIncidenceState(CDictionary<string, string> conditions, int state);
        public CDictionary<string, string> WhereNotDeleted(CDictionary<string, string> conditions);
        public CDictionary<string, string> WherePieceId(CDictionary<string, string> conditions, int? id);
        public CDictionary<string, string> WherePieceId(CDictionary<string, string> conditions, IList<int> ids);
        public CDictionary<string, string> WhereUsername(CDictionary<string, string> conditions, string username);
        public CDictionary<string, string> WhereEmployee(CDictionary<string, string> conditions, int employee);
        public CDictionary<string, string> WhereEmployeeId(CDictionary<string, string> conditions, int? employeeId);
        public CDictionary<string, string> WhereTechnicianId(CDictionary<string, string> conditions, int technicianId);
        public CDictionary<string, string> WherePassword(CDictionary<string, string> conditions, string password);
        public CDictionary<string, string> WhereId(CDictionary<string, string> conditions, int? id);
        public string GetMD5(string str);
    }
}
