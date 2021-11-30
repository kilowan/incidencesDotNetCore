namespace Incidences.Data.Models
{
    public class ColumnKeyValue<Tkey, TValue>
    {
        public Tkey key { get; set; }
        public Tkey column { get; set; }
        public TValue value { get; set; }
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
