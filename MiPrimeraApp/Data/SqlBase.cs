using MiPrimeraApp.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MiPrimeraApp.Data
{
    public class SqlBase
    {
        private string stringconnection;
        private IDbConnection connection;
        //private IDataReader reader;
        protected IDbCommand get_sql { get; set; }
        #region string generation

        protected bool Insert(string table, CDictionary<string, string> data)
        {
            string text = $"INSERT INTO { table } ({ string.Join(", ", data.Keys) }) VALUES ({ string.Join(", ", data.Values) })";
            return Call(text);
        }
        protected bool Select(Select select)
        {
            return Call(select.GetSentence(), "");
        }
        protected bool MultiSelectSQL(IList<Select> queries)
        {
            IList<string> sentences = new List<string>();
            foreach (Select query in queries)
            {
                sentences.Add(query.GetSentence());
            }

            string text = string.Join(" UNION ALL ", sentences);
            return Call(text, "");
        }
        protected string Where(CDictionary<string, string> conditions)
        {
            IList<string> results = new List<string>();
            foreach (ColumnKeyValue<string, string> item in conditions.Get())
            {
                string result;
                if (string.IsNullOrEmpty(item.key)) result = $"{ item.column } = '{ item.value }'";
                else result = $"{ item.column } { item.key } { item.value }";

                results.Add(result);
            }

            return $"WHERE { string.Join(" AND ", results) }";
        }
        protected bool Update(string table, CDictionary<string, string> columns, CDictionary<string, string> conditions)
        {
            IList<string> conditionsValues = new List<string>();
            foreach (ColumnKeyValue<string, string> item in columns.Get())
            {
                conditionsValues.Add($"{ item.column } = '{ item.value }'");
            }

            string text = $"UPDATE { table } SET { string.Join(", ", conditionsValues) } { Where(conditions) }";
            return Call(text);
        }
        protected void Delete()
        {

        }
        protected string GroupBySQL(IList<string> fields)
        {
            return " GROUP BY " + string.Join(", ", fields);
        }
        protected string InnerJoinSQL(IList<InnerJoin> innerJoin)
        {
		    int position = 0;
		    string innerText = string.Empty;
            foreach (InnerJoin inner in innerJoin)
            {
                if (position == 0)
                {
                    innerText = inner.tableA;
                }
                innerText = innerText + " INNER JOIN " + inner.tableB + " ON " + inner.conditions.column + " = " + inner.conditions.value;
                position++;
            }
            return innerText;
        }
        protected string OrderBySQL(Order orderBy)
        {
            return $"ORDER BY { string.Join(", ", orderBy.fields) } { orderBy.order }";
        }
        #endregion

        #region SQLServer
        //CONNECTION
        protected IDbCommand ConnectionFn()
        {
            string user = "test";
            string pass = "123456";

            this.stringconnection = $@"Data Source=localhost\SQLEXPRESS;DataBase=Incidences;Persist Security Info=True;USER ID={user};Password={pass};MultipleActiveResultSets=true;";
            this.connection = new SqlConnection(this.stringconnection);
            this.connection.Open();
            return this.connection.CreateCommand();
        }
        protected bool Call(string text, string type = null)
        {
            get_sql = ConnectionFn();
            get_sql.CommandText = text;
            return get_sql.ExecuteScalar() != null;
        }
        protected bool Call(string text)
        {
            get_sql = ConnectionFn();
            get_sql.CommandText = text;
            return get_sql.ExecuteNonQuery() > 0;
        }

        #endregion
    }
}
