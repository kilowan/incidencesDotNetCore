using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data.Models
{
    public partial class note_type : baseClass
    {
        public string name { get; set; }

        [InverseProperty("NoteType")]
        public virtual Notes Notes { get; set; }
    }
}
