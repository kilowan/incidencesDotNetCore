namespace MiPrimeraApp.Models.Employee
{
    public class Employee
    {
        public string name;
        public string surname1;
        public string surname2;
        private string fullName;
        public string dni;
        public TypeRange type;
        public int? id;
        public bool state;
        public Employee()
        {

        }
        public Employee(string dni, string name, string surname1)
        {
            this.dni = dni;
            this.name = name;
            this.surname1 = surname1;
            this.fullName = $"{ name } { surname1 }";
        }
        public Employee(string dni, string name, string surname1, string surname2)
        {
            this.dni = dni;
            this.name = name;
            this.surname1 = surname1;
            this.surname2 = surname2;
            this.fullName = $"{ name } { surname1 } { surname2 }";
        }
        public Employee(string dni, string name, string surname1, string surname2, TypeRange type, int id, bool state)
        {
            this.dni = dni;
            this.name = name;
            this.surname1 = surname1;
            this.surname2 = surname2;
            this.fullName = $"{ name } { surname1 } { surname2 }";
            this.type = type;
            this.id = id;
            this.state = state;
        }
        public void SetType(int id, string name) 
        {
            this.type = new TypeRange(id, name);
        }
        public string FullName {
            get 
            {
                return this.fullName;
            }
        }
    }
}
