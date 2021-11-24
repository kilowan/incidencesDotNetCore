using Incidences.Models.Incidence;
using MiPrimeraApp.Models.Incidence;
using System.Collections.Generic;

namespace Incidences.Business
{
    public interface IPieceBz
    {
        public bool InsertPiece(PieceDto piece);
        public bool AddPieceFn(PieceDto piece);
        public bool InsertPiecesSql(IList<int> pieces, int incidenceId);
        public bool DeletePiecesSql(IList<int> pieces, int incidenceId);
        public bool DeletePiece(int id);
        public bool UpdatePiece(PieceDto piece, int id);
        public bool UpdatePiece(int id, bool deleted);
        public bool DeletePieceFn(int id);
        public IList<Piece> GetPieces(int? piece = null);
    }
}
