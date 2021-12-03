using Incidences.Data.Models;

namespace Incidences.Models.Employee
{
    public class TypeRange
    {
        private int? id;
        private string name;
        public int? Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public TypeRange(int? id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public TypeRange(employee_range er)
        {
            this.id = er.id;
            this.name = er.name;
        }
    }
}