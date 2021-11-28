using System;

namespace Incidences.Models.Incidence
{
    public class NoteDto
    {
        public string noteStr { get; set; }
        public DateTime date { get; set; }
        public int? typeId { get; set; }
    }
}
