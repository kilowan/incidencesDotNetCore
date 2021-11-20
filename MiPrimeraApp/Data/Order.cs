using System.Collections.Generic;

namespace MiPrimeraApp.Data.Models
{
    public class Order
    {
        public string order;
		public IList<string> fields;
        public Order(string field)
        {
            this.fields = new List<string> { field };
        }
        public Order(string field, string order)
        {
            this.fields = new List<string> { field };
            this.order = order;
        }
        public Order(IList<string> fields)
        {
            this.fields = fields;
            this.order = "ASC";
        }
        public Order(IList<string> fields, string order)
        {
            this.fields = fields;
            this.order = order;
        }
    }
}
