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
        public byte deleted { get; set; }

        [ForeignKey(nameof(typeId))]
        public virtual piece_type PieceType { get; set; }
    }
}
