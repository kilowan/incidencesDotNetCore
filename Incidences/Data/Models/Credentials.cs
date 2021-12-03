using System.ComponentModel.DataAnnotations.Schema;

namespace Incidences.Data.Models
{
    public partial class Credentials : baseClass
    {
        public string username { get; set; }
        public string password { get; set; }
        public int employeeId { get; set; }

        [ForeignKey(nameof(employeeId))]
        public virtual employee Employee { get; set; }
    }
}
