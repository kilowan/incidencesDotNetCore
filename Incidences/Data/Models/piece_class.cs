using System.ComponentModel.DataAnnotations.Schema;

namespace Incidences.Data.Models
{
    public partial class piece_class : baseClass
    {
        public string name { get; set; }
        public int typeId { get; set; }
        public byte deleted { get; set; }

        [ForeignKey(nameof(piece_class.typeId))]
        public virtual piece_type PieceType { get; set; }
    }
}
