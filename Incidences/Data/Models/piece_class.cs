using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data.Models
{
    public partial class piece_class : baseClass
    {
        public string name { get; set; }
        public int typeId { get; set; }
        public bool deleted { get; set; }

        /*[ForeignKey(nameof(typeId))]
        [InverseProperty(nameof(id))]
        public virtual note_type NoteType { get; set; }*/
    }
}
