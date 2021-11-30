using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System.Collections.Generic;

namespace Incidences.Data
{
    public interface IPieceTypeData
    {
        public IList<PieceType> SelectPieceType(CDictionary<string, string> conditions = null);
        public PieceType SelectPieceTypeById(int pieceTypeId);
    }
}
