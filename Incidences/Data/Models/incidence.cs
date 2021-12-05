using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Incidences.Data.Models
{
    public partial class incidence : baseClass
    {
        public int ownerId { get; set; }
        public int? solverId { get; set; }
        public DateTime open_dateTime { get; set; }
        public DateTime? close_dateTime { get; set; }
        public int state { get; set; }

        [ForeignKey(nameof(incidence.ownerId))]
        public virtual employee owner { get; set; }

        [ForeignKey(nameof(incidence.solverId))]
        public virtual employee solver { get; set; }

        [InverseProperty("Incidence")]
        public virtual IList<Notes> notes { get; set; }

        public virtual IList<incidence_piece_log> pieces { get; set; }

        [ForeignKey(nameof(incidence.state))]
        public virtual state State { get; set; }
    }
}
