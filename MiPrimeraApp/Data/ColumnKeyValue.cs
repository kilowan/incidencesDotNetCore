namespace MiPrimeraApp.Data
{
    public class ColumnKeyValue
    {
        public string key;
        public string column;
        public string value;
        public ColumnKeyValue(string column, string key, string value)
        {
			this.column = column;
			this.key = key;
			this.value = value;
        }
    }
}
