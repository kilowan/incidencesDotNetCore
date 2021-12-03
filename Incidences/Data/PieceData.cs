using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Incidences.Data
{
    public class PieceData : IPieceData
    {
        private readonly IncidenceContext _context;
        private readonly IPieceTypeData pieceType;

        public PieceData(IncidenceContext context, IPieceTypeData pieceType)
        {
            _context = context;
            this.pieceType = pieceType;
        }

        public Piece SelectPieceById(int pieceId)
        {
            try
            {
                piece_class picla = _context.PieceClass
                    .Where(pi => pi.id == pieceId)
                    .FirstOrDefault();
                if (picla != null)
                {
                    PieceType pity = pieceType.SelectPieceTypeById(picla.typeId);
                    return new(picla.id, picla.name, pity, Convert.ToBoolean(picla.deleted));
                }
                else return new Piece();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool InsertPiece(PieceDto piece)
        {
            try
            {
                _context.PieceClass.Add(new()
                {
                    name = piece.name,
                    typeId = (int)piece.typeId
                });

                if (_context.SaveChanges() != 1) throw new Exception("Pieza no insertada");
                return true;
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
                IList<incidence_piece_log> ipls = new List<incidence_piece_log>();
                foreach (int pieceId in pieces)
                {
                    ipls.Add(new incidence_piece_log()
                    {
                        pieceId = pieceId,
                        incidenceId = incidenceId
                    });
                }
                _context.IncidencePieceLog.AddRange(ipls);
                if (_context.SaveChanges() != 1) throw new Exception("Piezas no insertadas");

                return true;
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
                IList<piece_class> piece_classes = _context.PieceClass
                    .Where(pc => pieces.Contains(pc.id))
                    .ToList();
                _context.PieceClass.UpdateRange(piece_classes);
                if (_context.SaveChanges() != 1) throw new Exception("Piezas no eliminadas");
                return true;
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
                piece_class picla = _context.PieceClass
                    .Where(pi => pi.id == id)
                    .FirstOrDefault();
                if (picla != null)
                {
                    picla.deleted = 1;
                    _context.PieceClass.Update(picla);
                    if (_context.SaveChanges() != 1) throw new Exception("Pieza no eliminada");
                    return true;
                }
                else return false;
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
                piece_class pi = _context.PieceClass
                    .Where(piece => piece.id == id)
                    .FirstOrDefault();
                if (pi != null)
                {
                    if (piece.deleted != null) pi.deleted = (byte)piece.deleted;
                    if (piece.name != null) pi.name = piece.name;
                    if (piece.typeId != null) pi.typeId = (int)piece.typeId;
                    _context.PieceClass.Update(pi);
                    if (_context.SaveChanges() != 1) throw new Exception("Pieza no actualizada");
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdatePiece(int id, byte deleted)
        {
            try
            {
                piece_class pi = _context.PieceClass
                    .Where(piece => piece.id == id)
                    .FirstOrDefault();
                if (pi != null)
                {
                    pi.deleted = deleted;
                    _context.PieceClass.Update(pi);
                    if (_context.SaveChanges() != 1) throw new Exception("Pieza no actualizada");
                    return true;
                }
                else return false;
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

                IList<piece_class> piece_classes;
                if (piece != null)
                {
                    piece_classes = _context.PieceClass
                        .Where(pi => pi.id == piece)
                        .ToList();
                }
                else
                {
                    piece_classes = _context.PieceClass
                        .ToList();
                }

                IList<Piece> pieces = new List<Piece>();
                foreach (piece_class piece_class in piece_classes)
                {
                    PieceType pity = pieceType.SelectPieceTypeById(piece_class.typeId);
                    pieces.Add(new(piece_class.id, piece_class.name, pity, Convert.ToBoolean(piece_class.deleted)));
                }

                return pieces;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IList<Piece> GetPiecesByIncidenceId(int incidenceId) 
        {
            IList<int> ipls = _context.IncidencePieceLog
                .Where(ipl => ipl.incidenceId == incidenceId)
                .Select(ipl => ipl.id)
                .ToList();

            IList<piece_class> cleanPieces = _context.PieceClass
                .Where(pie => ipls.Contains(pie.id))
                .ToList();

            IList<Piece> pieces = new List<Piece>();
            foreach (piece_class ipl in cleanPieces)
            {
                pieces.Add(
                    new Piece() 
                    { 
                        Deleted = Convert.ToBoolean(ipl.deleted),
                        Id = ipl.id,
                        Name = ipl.name,
                        Type = pieceType.SelectPieceTypeById(ipl.typeId)
                    }
                );
            }

            return pieces;
        }
    }
}
