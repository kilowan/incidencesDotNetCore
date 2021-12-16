namespace Incidences.Models.Employee
{
    public class EmployeeDto
    {
        public CredentialsDto credentials { get; set; }
        public string dni { get; set; }
        public string name { get; set; }
        public string surname1 { get; set; }
        public string surname2 { get; set; }
        public string type { get; set; }
        public int? typeId { get; set; }
        public bool? deleted { get; set; }
        public int? id { get; set; }
        public EmailDto email { get; set; }
    }
}
