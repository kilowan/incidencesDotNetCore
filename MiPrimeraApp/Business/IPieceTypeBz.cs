using MiPrimeraApp.Models.Incidence;
using System.Collections.Generic;

namespace Incidences.Business
{
    public interface IPieceTypeBz
    {
        public PieceType SelectPieceTypeById(int pieceTypeId);
        public IList<PieceType> SelectPieceTypeList();
    }
}
