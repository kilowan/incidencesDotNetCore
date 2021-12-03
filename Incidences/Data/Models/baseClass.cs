using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data.Models
{
    public partial class baseClass
    {
        [Key]
        public int id { get; set; }
    }
}
