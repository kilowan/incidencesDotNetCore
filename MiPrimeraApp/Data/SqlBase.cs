using MiPrimeraApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Data
{
    public class SqlBase
    {
        private string stringconnection;
        private IDbConnection connection;
        private IDataReader reader;
        public IDbCommand get_sql { get; }
        #region string generation
        public bool Insert(string table, CDictionary<string, string, string> data, IDbCommand conexion = null)
        {
            string text = $"INSERT INTO { table } ({ string.Join(", ", data.Keys) }) VALUES ({ string.Join(", ", data.Values) })";
            return Call(text, conexion);
        }
        public object select(select $select, mysqli $conexion = null)
        {
            return call($select->getSentence(), $conexion);
        }
        public multiSelectSQL(array $queries, mysqli $conexion = null)
        {
            $text = '';
            $sentences = [];
            foreach ($queries as $query) {
                array_push($sentences, $query->getSentence());
            }
            $text = implode(' UNION ALL ', $sentences);
            return call($text, $conexion);
            }
        public where(array $conditions)
        {
        $results = [];
            foreach ($conditions as $condition) {
                if (!$condition->key) {
                $result = "$condition->column = '$condition->value'";
                } else
                {
                $result = $condition->column.' '.$condition->key.' '.$condition->value;
                }

                array_push($results, $result);
            }
            return ' WHERE '.implode(' AND ', $results);
        }
        public update(mysqli $conexion = null, string $table, dictionary $columns, dictionary $conditions)
        {
        $conditionsValues = [];
            foreach ($columns->get() as $data) {
                array_push($conditionsValues, "$data->column = '$data->value'");
            }
        
        $text = 'UPDATE '.$table.' SET '.implode(', ', $conditionsValues).where($conditions->get());
            return call($text, $conexion);
        }
        public void delete()
        {

        }
        public string GroupBySQL(IList<string> fields)
        {
            return " GROUP BY " + string.Join(", ", fields);
        }
        public string InnerJoinSQL(IList<InnerJoin> innerJoin)
        {
		    int position = 0;
		    string innerText = '';
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
        #endregion

        #region SQLServer
        //CONNECTION
        public IDbCommand ConnectionFn()
        {
            string user = "test";
            string pass = "123456";

            this.stringconnection = $@"Data Source=localhost\SQLEXPRESS;DataBase=Incidences;Persist Security Info=True;USER ID={user};Password={pass};MultipleActiveResultSets=true;";
            this.connection = new SqlConnection(this.stringconnection);
            this.connection.Open();
            return this.connection.CreateCommand();
        }
        public bool Call(string text, IDbCommand conexion = null, string type = null)
        {
            if (conexion == null) conexion = ConnectionFn();
            return get_sql.ExecuteScalar() != null ? true : false;
        }
        public bool Call(string text, IDbCommand conexion = null)
        {
            if (conexion == null) conexion = ConnectionFn();
            return get_sql.ExecuteNonQuery() > 0 ? true : false;
        }

        #endregion
    }
}
