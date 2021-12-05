using System;
using System.Collections.Generic;

namespace Incidences.Models.Incidence
{
    public class IncidenceDto
    {
        public string owner { get; set; }
        public int ownerId { get; set; }
        public string solver { get; set; }
        public int? solverId { get; set; }
        public DateTime? initDateTime { get; set; }
        public DateTime? finishDateTime { get; set; }
        public IList<int> piecesAdded { get; set; }
        public IList<int> piecesDeleted { get; set; }
        public string note { get; set; }
        public int? state { get; set; }
    }
}
