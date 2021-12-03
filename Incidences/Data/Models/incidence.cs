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

        [ForeignKey(nameof(ownerId))]
        public virtual employee owner { get; set; }

        [ForeignKey(nameof(solverId))]
        public virtual employee solver { get; set; }

        [ForeignKey(nameof(id))]
        public virtual IList<Notes> notes { get; set; }

        [ForeignKey(nameof(id))]
        public virtual IList<piece_class> pieces { get; set; }
    }
}
