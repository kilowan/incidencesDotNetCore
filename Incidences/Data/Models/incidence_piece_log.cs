using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data.Models
{
    public partial class incidence_piece_log : baseClass
    {
        public int pieceId { get; set; }
        public int incidenceId { get; set; }
        public int status { get; set; }

        [ForeignKey(nameof(incidence_piece_log.pieceId))]
        public virtual piece_class Piece { get; set; }
    }
}
