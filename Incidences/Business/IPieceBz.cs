using Incidences.Models.Incidence;
using System.Collections.Generic;

namespace Incidences.Business
{
    public interface IPieceBz
    {
        public bool InsertPiece(PieceDto piece);
        public bool AddPiece(PieceDto piece);
        public bool InsertPiecesSql(IList<int> pieces, int incidenceId);
        public bool DeletePiecesSql(IList<int> pieces, int incidenceId);
        public bool UpdatePiece(PieceDto piece, int id);
        public bool UpdatePiece(int id, byte deleted);
        public bool DeletePiece(int id);
        public IList<Piece> GetPieces(int? piece = null);
    }
}
