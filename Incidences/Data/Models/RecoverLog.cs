using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Incidences.Data.Models
{
    public class RecoverLog : baseClass
    {
        public string code { get; set; }
        public int employeeIdId { get; set; }
        public DateTime date { get; set; }
        public bool active { get; set; }

        [ForeignKey(nameof(RecoverLog.employeeIdId))]
        public virtual employee Employee { get; set; }
    }
}
