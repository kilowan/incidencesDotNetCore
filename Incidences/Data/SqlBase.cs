using Incidences.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Incidences.Data
{
    public class SqlBase : ISqlBase
    {
        #region constants
        //SQL
        private const string AND = "AND";
        private const string IS = "IS";
        private const string equal = "=";
        private const string INSERT = "INSERT INTO";
        private const string VALUES = "VALUES";
        private const string WHERE = "WHERE";
        private const string SET = "SET";
        private const string UPDATE = "UPDATE";
        private const string GROUP = "GROUP BY";
        private const string INNER = "INNER JOIN";
        private const string ON = "ON";
        private const string ORDER = "ORDER BY";
        #endregion
        private string stringconnection;
        private IDbConnection connection;
        private IDbCommand get_sql;

        #region string generation

        public bool Insert(string table, CDictionary<string, string> data)
        {
            string text = $"{INSERT} { table } ({ string.Join(", ", data.Keys) }) {VALUES} ({ string.Join(", ", data.Values) })";
            return Call(text);
        }
        public bool Select(Select select)
        {
            return Call(select.GetSentence(), "");
        }
        public string Where(CDictionary<string, string> conditions)
        {
            IList<string> results = new List<string>();
            foreach (ColumnKeyValue<string, string> item in conditions.Get())
            {
                string result;
                if (string.IsNullOrEmpty(item.key)) result = $"{ item.column } = '{ item.value }'";
                else result = $"{ item.column } { item.key } { item.value }";

                results.Add(result);
            }

            return $"{WHERE} { string.Join($" {AND} ", results) }";
        }
        public string Where(ColumnsKeysValues<string, string> data)
        {
            IList<string> strings = new List<string>();
            foreach (ColumnKeyValue<string, string> value in data.KeyValue)
            {
                if (value.key == IS) strings.Add($"{ value.column } { value.key } { $"{ value.value }" }");
                else strings.Add($"{ value.column } { value.key } { $"'{ value.value }'" }");
            }

            if (data.Children != null) strings.Add($"({ Where(data.Children) })");

            return string.Join($" {data.Connector} ", strings);
        }
        public bool Update(string table, CDictionary<string, string> columns, CDictionary<string, string> conditions)
        {
            IList<string> conditionsValues = new List<string>();
            foreach (ColumnKeyValue<string, string> item in columns.Get())
            {
                conditionsValues.Add($"{ item.column } = '{ item.value }'");
            }

            string text = $"{UPDATE} { table } {SET} { string.Join(", ", conditionsValues) } { Where(conditions) }";
            return Call(text);
        }
        public void Delete()
        {

        }
        public string GroupBySQL(IList<string> fields)
        {
            return $" {GROUP} {string.Join(", ", fields)}";
        }
        public string InnerJoinSQL(IList<InnerJoin> innerJoin)
        {
            int position = 0;
            string innerText = string.Empty;
            foreach (InnerJoin inner in innerJoin)
            {
                if (position == 0)
                {
                    innerText = inner.tableA;
                }

                innerText += $" {INNER} {inner.tableB} {ON} {inner.conditions.column} {equal} {inner.conditions.value}";
                position++;
            }
            return innerText;
        }
        public string OrderBySQL(Order orderBy)
        {
            return $"{ORDER} { string.Join(", ", orderBy.fields) } { orderBy.order }";
        }

        public CDictionary<string, string> WhereEmployeeId(CDictionary<string, string> conditions, int? employeeId)
        {
            return WhereCommon(conditions, "employeeId", null, $"{ employeeId }");
        }
        public CDictionary<string, string> WhereIncidenceState(CDictionary<string, string> conditions, int state)
        {
            return WhereCommon(conditions, "state", null, $"{ state }");
        }
        public CDictionary<string, string> WhereIncidence(CDictionary<string, string> conditions, int incidenceId)
        {
            return WhereCommon(conditions, "id", null, $"{ incidenceId }");
        }
        public CDictionary<string, string> WhereOwnerId(CDictionary<string, string> conditions, int ownerId)
        {
            return WhereCommon(conditions, "ownerId", null, $"{ ownerId }");
        }
        public CDictionary<string, string> WhereUsername(CDictionary<string, string> conditions, string username)
        {
            return WhereCommon(conditions, "username", null, $"{ username }");
        }
        public CDictionary<string, string> WherePassword(CDictionary<string, string> conditions, string password)
        {
            return WhereCommon(conditions, "password", null, GetMD5(password));
        }
        public CDictionary<string, string> WhereEmployee(CDictionary<string, string> conditions, int employee)
        {
            return WhereCommon(conditions, "employee", null, $"{ employee }");
        }
        public CDictionary<string, string> WhereEmployeeTypeName(CDictionary<string, string> conditions, string typeName)
        {
            return WhereCommon(conditions, "name", null, $"{ typeName }");
        }
        public CDictionary<string, string> WhereIncidenceId(CDictionary<string, string> conditions, int incidenceId)
        {
            return WhereCommon(conditions, "incidenceId", null, $"{ incidenceId }");
        }
        public CDictionary<string, string> WhereIncidenceId(CDictionary<string, string> conditions, int? incidenceId)
        {
            return WhereCommon(conditions, "incidenceId", null, $"{ incidenceId }");
        }
        public CDictionary<string, string> WhereNoteType(CDictionary<string, string> conditions, string noteType)
        {
            return WhereCommon(conditions, "noteType", null, $"{ noteType }");
        }
        public CDictionary<string, string> WhereNoteTypeId(CDictionary<string, string> conditions, int noteType)
        {
            return WhereCommon(conditions, "noteTypeId", null, $"'{ noteType }'");
        }
        public CDictionary<string, string> WhereNotDeleted(CDictionary<string, string> conditions)
        {
            return WhereCommon(conditions, "deleted", "<>", $"{ 1 }");
        }
        public CDictionary<string, string> WhereId(CDictionary<string, string> conditions, int? id)
        {
            return WhereCommon(conditions, "id", null, id.ToString());
        }
        public CDictionary<string, string> WherePieceId(CDictionary<string, string> conditions, int? id)
        {
            return WhereCommon(conditions, "id", "=", id.ToString());
        }
        public CDictionary<string, string> WherePieceId(CDictionary<string, string> conditions, IList<int> ids)
        {
            return WhereCommon(conditions, "id", "IN", string.Join(", ", ids));
        }
        private static CDictionary<string, string> WhereCommon(CDictionary<string, string> conditions, string column, string key, string value)
        {
            conditions.Add(column, key, value);
            return conditions;
        }

        public string GetMD5(string str)
        {
            MD5 md5 = MD5.Create();
            ASCIIEncoding encoding = new();
            byte[] stream = null;
            StringBuilder sb = new();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        #endregion

        #region SQLServer
        //CONNECTION
        public void ConnectionFn()
        {
            string user = "test";
            string pass = "123456";

            this.stringconnection = $@"Data Source=localhost\SQLEXPRESS;DataBase=Incidences;Persist Security Info=True;USER ID={user};Password={pass};MultipleActiveResultSets=true;";
            this.connection = new SqlConnection(this.stringconnection);
        }
        public IDbCommand command
        {
            get { return get_sql; }
            set { get_sql = value; }
        }
        public IDataReader GetReader()
        {
            return get_sql.ExecuteReader();
        }
        public void Open()
        {
            this.connection.Open();
        }
        public void Close()
        {
            this.connection.Close();
        }
        public bool Call(string text, string type = null)
        {
            ConnectionFn();
            Open();
            get_sql = connection.CreateCommand();
            get_sql.CommandText = text;
            return get_sql.ExecuteScalar() != null;
        }
        public bool Call(string text)
        {
            ConnectionFn();
            Open();
            get_sql = connection.CreateCommand();
            get_sql.CommandText = text;
            return get_sql.ExecuteNonQuery() > 0;
        }

        #endregion
    }
}
