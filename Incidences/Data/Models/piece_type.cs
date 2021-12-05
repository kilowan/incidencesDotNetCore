﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Incidences.Data.Models
{
    public partial class piece_type : baseClass
    {
        public string name { get; set; }
        public string description { get; set; }

        [InverseProperty("PieceType")]
        public virtual piece_class Piece { get; set; }
    }
}
