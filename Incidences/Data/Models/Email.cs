using System.ComponentModel.DataAnnotations.Schema;

namespace Incidences.Data.Models
{
    public class Email : baseClass
    {
        public string name { get; set; }
        public string domain { get; set; }
        public int employeeId { get; set; }

        [ForeignKey(nameof(Email.employeeId))]
        public employee Employee { get; set; }
    }
}
