using System.ComponentModel.DataAnnotations.Schema;

namespace Incidences.Data.Models
{
    public partial class employee_range : baseClass
    {
        public string name { get; set; }

        [InverseProperty("EmployeeRange")]
        public virtual employee Employee { get; set; }
    }
}
