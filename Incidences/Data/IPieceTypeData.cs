using Incidences.Models.Incidence;
using System.Collections.Generic;

namespace Incidences.Data
{
    public interface IPieceTypeData
    {
        public IList<PieceType> SelectPieceType();
        public PieceType SelectPieceTypeById(int pieceTypeId);
    }
}
