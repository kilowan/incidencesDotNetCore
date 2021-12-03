using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;

namespace Incidences.Data
{
    public interface IPieceData
    {
        public bool InsertPiece(PieceDto piece);
        public bool InsertPiecesSql(IList<int> pieces, int incidenceId);
        public bool DeletePiecesSql(IList<int> pieces, int incidenceId);
        public Piece SelectPieceById(int pieceId);
        public bool DeletePiece(int id);
        public bool UpdatePiece(PieceDto piece, int id);
        public bool UpdatePiece(int id, bool deleted);
        public IList<Piece> GetPieces(int? piece = null);
        public IList<Piece> GetPiecesByIncidenceId(int incidenceId);
    }
}
