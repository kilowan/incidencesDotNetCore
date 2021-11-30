using Incidences.Data.Models;
using System.Collections.Generic;
using System.Data;

namespace Incidences.Data
{
    public interface ISqlBase
    {
        public IDbCommand command { get; set; }

        /// <summary>
        /// Makes Insert string
        /// </summary>
        /// <param name="table">The table param</param>
        /// <param name="data">Where params</param>
        /// <returns>Returns Insert string</returns>
        public bool Insert(string table, CDictionary<string, string> data);
        /// <summary>
        /// Makes Select string
        /// </summary>
        /// <param name="select">The Select params</param>
        /// <returns>Returns Select string</returns>
        public bool Select(Select select);
        /// <summary>
        /// Makes MultiSelect string
        /// </summary>
        /// <param name="queries">The queries</param>
        /// <returns>Returns queries joined</returns>
        //public bool MultiSelectSQL(IList<Select> queries);
        /// <summary>
        /// Makes Where string
        /// </summary>
        /// <param name="conditions">where params</param>
        /// <returns>Returns Where string</returns>
        public string Where(CDictionary<string, string> conditions);
        /// <summary>
        /// Launch Update call
        /// </summary>
        /// <param name="table">The table param</param>
        /// <param name="columns">Columns affected</param>
        /// <param name="conditions">Where params</param>
        /// <returns>Returns true or false</returns>
        public bool Update(string table, CDictionary<string, string> columns, CDictionary<string, string> conditions);
        /// <summary>
        /// Launch Delete call
        /// </summary>
        public void Delete();
        /// <summary>
        /// Makes the GroupBy string
        /// </summary>
        /// <param name="fields">GroupBy params</param>
        /// <returns>Return the GroupBy string</returns>
        public string GroupBySQL(IList<string> fields);
        /// <summary>
        /// Makes the InnerJoin string
        /// </summary>
        /// <param name="innerJoin">InnerJoin params</param>
        /// <returns>Returns the InnerJoin string</returns>
        public string InnerJoinSQL(IList<InnerJoin> innerJoin);
        /// <summary>
        /// Makes the OrderBy string
        /// </summary>
        /// <param name="orderBy">OrderBy params</param>
        /// <returns>Returns the OrderBy string</returns>
        public string OrderBySQL(Order orderBy);

        public CDictionary<string, string> WhereEmployeeId(CDictionary<string, string> conditions, int? employeeId);
        public CDictionary<string, string> WhereIncidenceState(CDictionary<string, string> conditions, int state);
        public CDictionary<string, string> WhereIncidence(CDictionary<string, string> conditions, int incidenceId);
        public CDictionary<string, string> WhereOwnerId(CDictionary<string, string> conditions, int ownerId);
        public CDictionary<string, string> WhereUsername(CDictionary<string, string> conditions, string username);
        public CDictionary<string, string> WherePassword(CDictionary<string, string> conditions, string password);
        public CDictionary<string, string> WhereEmployee(CDictionary<string, string> conditions, int employee);
        public CDictionary<string, string> WhereEmployeeTypeName(CDictionary<string, string> conditions, string typeName);
        public CDictionary<string, string> WhereIncidenceId(CDictionary<string, string> conditions, int incidenceId);
        public CDictionary<string, string> WhereIncidenceId(CDictionary<string, string> conditions, int? incidenceId);
        public CDictionary<string, string> WhereNoteType(CDictionary<string, string> conditions, string noteType);
        public CDictionary<string, string> WhereNoteTypeId(CDictionary<string, string> conditions, int noteType);
        public CDictionary<string, string> WhereNotDeleted(CDictionary<string, string> conditions);
        public CDictionary<string, string> WhereId(CDictionary<string, string> conditions, int? id);
        public CDictionary<string, string> WherePieceId(CDictionary<string, string> conditions, int? id);
        public CDictionary<string, string> WherePieceId(CDictionary<string, string> conditions, IList<int> ids);
        public string GetMD5(string str);
        /// <summary>
        /// Sets the IDBCommand Connection
        /// </summary>
        public void ConnectionFn();
        /// <summary>
        /// Launch Call to the SQLServer
        /// </summary>
        /// <param name="text">The query string</param>
        /// <param name="type">The call's type</param>
        /// <returns>Returns true or false</returns>
        public bool Call(string text, string type = null);
        /// <summary>
        /// Launch Call to the SQLServer
        /// </summary>
        /// <param name="text">The query string</param>
        /// <returns>Returns true or false</returns>
        public bool Call(string text);
        /// <summary>
        /// Gets the reader
        /// </summary>
        /// <returns>Returns Reader</returns>
        public IDataReader GetReader();
        /// <summary>
        /// Opens the connection
        /// </summary>
        public void Open();
        /// <summary>
        /// Closes the connection
        /// </summary>
        public void Close();
    }
}
