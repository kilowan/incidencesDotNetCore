using System.Collections.Generic;

namespace Incidences.Data
{
    public class ColumnsKeysValues<T, K>
    {
        private IList<ColumnKeyValue<T, K>> keyValue;
        private string connector;
        private ColumnsKeysValues<T, K> children;

        public IList<ColumnKeyValue<T, K>> KeyValue
        {
            get
            {
                return keyValue;
            }
            set
            {
                keyValue = value;
            }
        }
        public string Connector
        {
            get
            {
                return connector;
            }
            set
            {
                connector = value;
            }
        }
        public ColumnsKeysValues<T, K> Children
        {
            get
            {
                return children;
            }
            set
            {
                children = value;
            }
        }
    }
}
