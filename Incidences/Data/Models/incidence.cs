using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data.Models
{
    public partial class incidence : baseClass
    {
        public int ownerId { get; set; }
        public int? solverId { get; set; }
        public DateTime open_dateTime { get; set; }
        public DateTime? close_dateTime { get; set; }
        public int state { get; set; }

        /*[ForeignKey(nameof(ownerId))]
        [InverseProperty(nameof(id))]
        public virtual employee owner { get; set; }

        [ForeignKey(nameof(solverId))]
        [InverseProperty(nameof(id))]
        public virtual employee solver { get; set; }*/
    }
}
