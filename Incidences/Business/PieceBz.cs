using Incidences.Data;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;

namespace Incidences.Business
{
    public class PieceBz : IPieceBz
    {
        private readonly IPieceData pieceData;
        public PieceBz(IPieceData pieceData)
        {
            this.pieceData = pieceData;
        }
        public bool InsertPiece(PieceDto piece)
        {
            try
            {
                return this.pieceData.InsertPiece(piece);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool AddPiece(PieceDto piece)
        {
            try
            {
                return InsertPiece(piece);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool InsertPiecesSql(IList<int> pieces, int incidenceId)
        {
            try
            {
                return pieceData.InsertPiecesSql(pieces, incidenceId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool DeletePiecesSql(IList<int> pieces, int incidenceId)
        {
            try
            {
                return pieceData.DeletePiecesSql(pieces, incidenceId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdatePiece(PieceDto piece, int id)
        {
            try
            {
                return pieceData.UpdatePiece(piece, id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdatePiece(int id, bool deleted)
        {
            try
            {
                return pieceData.UpdatePiece(id, deleted);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool DeletePiece(int id)
        {
            try
            {
                return UpdatePiece(id, true);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IList<Piece> GetPieces(int? piece = null)
        {
            try
            {
                return pieceData.GetPieces(piece);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
