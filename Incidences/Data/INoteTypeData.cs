using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data
{
    public interface INoteTypeData
    {
        public NoteType GetNoteTypeByName(string noteTypeName);
    }
}
