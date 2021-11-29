using Incidences.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
