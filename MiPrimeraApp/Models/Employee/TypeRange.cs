namespace MiPrimeraApp.Models.Employee
{
    public class TypeRange
    {
        public int? id { get; set; }
        public string name { get; set; }
        public TypeRange(int? id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}