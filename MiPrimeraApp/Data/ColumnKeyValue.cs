using System.Collections.Generic;

namespace MiPrimeraApp.Data
{
    public class ColumnKeyValue<Tkey, TValue>
    {
        public Tkey key;
        public Tkey column;
        public TValue value;
        public ColumnKeyValue()
        {

        }
        public ColumnKeyValue(Tkey column, Tkey key, TValue value)
        {
			this.column = column;
			this.key = key;
			this.value = value;
        }
    }
}
