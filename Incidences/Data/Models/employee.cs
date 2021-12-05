using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Incidences.Data.Models
{
    public partial class employee : baseClass
    {
        public string dni { get; set; }
        public string name { get; set; }
        public string surname1 { get; set; }
        public string surname2 { get; set; }
        public int typeId { get; set; }
        public int state { get; set; }

        [ForeignKey(nameof(employee.typeId))]
        public virtual employee_range EmployeeRange { get; set; }

        [ForeignKey(nameof(employee.id))]
        public virtual Credentials Credentials { get; set; }

        [InverseProperty("Employee")]
        public virtual IList<Notes> Notes { get; set; }

    }
}
