using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data.Models
{
    public partial class Notes : baseClass
    {
        public int employeeId { get; set; }
        public int incidenceId { get; set; }
        public int noteTypeId { get; set; }
        public string noteStr { get; set; }
        public DateTime date { get; set; }

        [ForeignKey(nameof(Notes.incidenceId))]
        public virtual incidence Incidence { get; set; }

        [ForeignKey(nameof(Notes.employeeId))]
        public virtual employee Employee { get; set; }

        [ForeignKey(nameof(Notes.noteTypeId))]
        public virtual note_type NoteType { get; set; }
    }
}
