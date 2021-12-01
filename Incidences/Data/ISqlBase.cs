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
        public CDictionary<string, string> WhereDni(string Dni, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereDeleted(int deleted, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereEmployeeId(int? employeeId, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereIncidenceState(int state, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereIncidence(int incidenceId, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereOwnerId(int ownerId, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereUsername(string username, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WherePassword(string password, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereEmployee(int employee, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereEmployeeTypeName(string typeName, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereIncidenceId(int incidenceId, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereIncidenceId(int? incidenceId, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereNoteType(string noteType, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereNoteTypeId(int noteType, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereNotDeleted(CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WhereId(int? id, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WherePieceId(int? id, CDictionary<string, string> conditions = null);
        public CDictionary<string, string> WherePieceId(IList<int> ids, CDictionary<string, string> conditions = null);
        public string GetMD5(string str);
        /// <summary>
        /// Sets the IDBCommand Connection
        /// </summary>
        public void Connection();
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
