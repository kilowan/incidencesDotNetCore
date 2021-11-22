using MiPrimeraApp.Data.Models;
using System.Collections.Generic;

namespace MiPrimeraApp.Data
{
    public class Select : SqlBase
    {
        private IList<string> tables;
        private IList<string> columns;
        private CDictionary<string, string> conditions;
        private IList<string> group;
        private IList<InnerJoin> inner;
        private Order orderBy;

        public Select(string table)
        {
            tables = new List<string>();
            tables.Add(table);
            columns = new List<string>();
            columns.Add("*");
        }
        public Select(string table, IList<string> columns)
        {
            tables = new List<string>();
            tables.Add(table);
            this.columns = columns;
            conditions = null;
            group = null;
            orderBy = null;
            inner = null;
        }
        public Select(string table, IList<string> columns, CDictionary<string, string> conditions)
        {
            tables = new List<string>();
            tables.Add(table);
            this.columns = columns;
            this.conditions = conditions;
            group = null;
            orderBy = null;
            inner = null;
        }
        public Select(string table, IList<string> columns, CDictionary<string, string> conditions, IList<string> group)
        {
            tables = new List<string>();
            tables.Add(table);
            this.columns = columns;
            this.conditions = conditions;
            this.group = group;
            orderBy = null;
            inner = null;
        }
        public Select(string table, IList<string> columns, Order orderBy)
        {
            this.tables = new List<string>();
            this.tables.Add(table);
            this.columns = columns;
            this.conditions = null;
            this.group = null;
            this.orderBy = orderBy;
            this.inner = null;
        }
        public Select(string table, IList<string> columns, CDictionary<string, string> conditions, IList<string> group, Order orderBy)
        {
            tables = new List<string>();
            tables.Add(table);
            this.columns = columns;
            this.conditions = conditions;
            this.group = group;
            this.orderBy = orderBy;
            inner = null;
        }
        public Select(IList<string> tables, IList<string> columns, CDictionary<string, string> conditions, IList<string> group, IList<InnerJoin> inner, Order orderBy)
        {
            this.tables = tables;
            this.columns = columns;
            this.conditions = conditions;
            this.group = group;
            this.inner = inner;
            this.orderBy = orderBy;
        }
        public string GetSentence()
        {
            string text;
            if (inner != null)
            {
                text = $"SELECT { string.Join(", ", columns) } FROM { InnerJoinSQL(inner) }";
            }
            else
            {
                text = $"SELECT { string.Join(", ", columns) } FROM { tables[0] }";
            }

            if (conditions != null)
            {
                text = $"{ text } { Where(conditions) }";
            }
            if (group != null)
            {
                text = $"{ text } { GroupBySQL(group) }";
            }
            if (orderBy != null)
            {
                text = $"{ text } { OrderBySQL(orderBy) }";
            }
            return text;
        }
    }
}
