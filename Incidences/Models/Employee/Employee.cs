using Incidences.Data.Models;

namespace Incidences.Models.Employee
{
    public class Employee
    {
        private string name;
        private string surname1;
        private string surname2;
        private string fullName;
        private string dni;
        private TypeRange type;
        private int? id;
        private int state;

        public string FullName
        {
            get
            {
                return this.fullName;
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                name = value;
            }
        }
        public string Surname1
        {
            get
            {
                return surname1;
            }
            set
            {
                surname1 = value;
            }
        }
        public string Surname2
        {
            get
            {
                return surname2;
            }
            set
            {
                surname2 = value;
            }
        }
        public string Dni
        {
            get
            {
                return dni;
            }
            set
            {
                dni = value;
            }
        }
        public TypeRange Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
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
        public int State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public Employee()
        {

        }
        public Employee(employee emp)
        {
            this.dni = emp.dni;
            this.name = emp.name;
            this.surname1 = emp.surname1;
            this.surname2 = emp.surname2;
            this.fullName = $"{ name } { surname1 } { surname2 }";
            this.type = new TypeRange(emp.EmployeeRange);
            this.id = emp.id;
            this.state = emp.state;
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
        public Employee(string dni, string name, string surname1, string surname2, TypeRange type, int id, int state)
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
    }
}
