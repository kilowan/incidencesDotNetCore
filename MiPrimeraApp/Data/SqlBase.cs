using Incidences.Data;
using MiPrimeraApp.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MiPrimeraApp.Data
{
    public class SqlBase : ISqlBase
    {
        private string stringconnection;
        private IDbConnection connection;
        private IDbCommand get_sql;

        #region string generation

        public bool Insert(string table, CDictionary<string, string> data)
        {
            string text = $"INSERT INTO { table } ({ string.Join(", ", data.Keys) }) VALUES ({ string.Join(", ", data.Values) })";
            return Call(text);
        }
        public bool Select(Select select)
        {
            return Call(select.GetSentence(), "");
        }
        public bool Select(string query) 
        {
            return Call(query, "");
        }
        public bool MultiSelect(IList<string> queries) 
        {
            return Call(string.Join(" UNION ALL ", queries), "");
        }
        public string GetSentence(int state, string column, int userId) 
        {
            if (column == "ownerId")
            {
                return $"SELECT COUNT(*) AS counter FROM incidence WHERE state = '{ state }' AND { column } = '{ userId }'";
            }
            else 
            {
                return $"SELECT COUNT(*) AS counter FROM incidence WHERE state = '{ state }' AND ({ column } = '{ userId }' OR { column } IS NULL)";
            }
        }
        public bool MultiSelectSQL(IList<Select> queries)
        {
            IList<string> sentences = new List<string>();
            foreach (Select query in queries)
            {
                sentences.Add(query.GetSentence());
            }

            string text = string.Join(" UNION ALL ", sentences);
            return Call(text, "");
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

            return $"WHERE { string.Join(" AND ", results) }";
        }
        public bool Update(string table, CDictionary<string, string> columns, CDictionary<string, string> conditions)
        {
            IList<string> conditionsValues = new List<string>();
            foreach (ColumnKeyValue<string, string> item in columns.Get())
            {
                conditionsValues.Add($"{ item.column } = '{ item.value }'");
            }

            string text = $"UPDATE { table } SET { string.Join(", ", conditionsValues) } { Where(conditions) }";
            return Call(text);
        }
        public void Delete()
        {

        }
        public string GroupBySQL(IList<string> fields)
        {
            return " GROUP BY " + string.Join(", ", fields);
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
                innerText = innerText + " INNER JOIN " + inner.tableB + " ON " + inner.conditions.column + " = " + inner.conditions.value;
                position++;
            }
            return innerText;
        }
        public string OrderBySQL(Order orderBy)
        {
            return $"ORDER BY { string.Join(", ", orderBy.fields) } { orderBy.order }";
        }

        private static Select GetCountSentence(int state, string column, int userId)
        {
            return new Select(
                "incidence",
                new List<string> { "COUNT(*) AS counter" },
                new CDictionary<string, string> {
                 { "state", null, state.ToString() },
                 { column, null, userId.ToString() }
                }
               );
        }
        public IList<Select> GetArray(int iterations, string field, int value)
        {
            IList<Select> sentences = new List<Select>();
            for (int i = 1; i <= iterations; i++)
            {
                sentences.Add(GetCountSentence(i, field, value));
            }

            return sentences;
        }
        public IList<string> GetStringArray(int iterations, string field, int value)
        {
            IList<string> sentences = new List<string>();
            for (int i = 1; i <= iterations; i++)
            {
                sentences.Add(GetSentence(i, field, value));
            }

            return sentences;
        }
        public IDictionary<string, int> GetCounters(bool result, IDictionary<string, int> counters)
        {
            if (result)
            {
                string[] names = new string[] { "new", "old", "closed", "hidden" };
                int counter = 0;
                using IDataReader reader = this.get_sql.ExecuteReader();
                while (reader.Read())
                {
                    counters[names[counter]] += (int)reader.GetValue(0);
                    counters["total"] += (int)reader.GetValue(0);
                    counter++;
                }

                this.Close();
            }

            return counters;
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
        public IDbCommand command {
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
