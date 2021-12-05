using System.ComponentModel.DataAnnotations.Schema;

namespace Incidences.Data.Models
{
    public partial class state : baseClass
    {
        public string name { get; set; }

        public virtual incidence Incidence { get; set; }
    }
}
