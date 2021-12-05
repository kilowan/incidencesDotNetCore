using Incidences.Data.Models;
using Incidences.Models.Incidence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Incidences.Data
{
    public class PieceData : IPieceData
    {
        private readonly IncidenceContext _context;

        public PieceData(IncidenceContext context)
        {
            _context = context;
        }

        public Piece SelectPieceById(int pieceId)
        {
            try
            {
                piece_class picla = _context.PieceClasss
                    .Include(a => a.PieceType)
                    .Where(pi => pi.id == pieceId)
                    .FirstOrDefault();
                if (picla != null)
                {
                    return new(
                        picla.id, 
                        picla.name, 
                        new PieceType() 
                        { 
                            Id = picla.PieceType.id, 
                            Name = picla.PieceType.name, 
                            Description = picla.PieceType.description 
                        }, 
                        picla.deleted
                    );
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
                int id = _context.PieceClasss
                    .Select(pie => pie.id)
                    .Max()+1;
                _context.PieceClasss.Add(new()
                {
                    name = piece.name,
                    typeId = (int)piece.typeId,
                    id = id
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
                    int? old = _context.IncidencePieceLogs
                        .Select(incidence => incidence.id)
                        .Max();
                    if (old == null) old = 1;
                    else old += 1;
                    ipls.Add(new incidence_piece_log()
                    {
                        pieceId = pieceId,
                        incidenceId = incidenceId,
                        id = (int)old
                    });
                }
                _context.IncidencePieceLogs.AddRange(ipls);
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
                IList<piece_class> piece_classes = _context.PieceClasss
                    .Where(pc => pieces.Contains(pc.id))
                    .ToList();
                _context.PieceClasss.UpdateRange(piece_classes);
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
                piece_class picla = _context.PieceClasss
                    .Where(pi => pi.id == id)
                    .FirstOrDefault();
                if (picla != null)
                {
                    picla.deleted = 1;
                    _context.PieceClasss.Update(picla);
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
                piece_class pi = _context.PieceClasss
                    .Where(piece => piece.id == id)
                    .FirstOrDefault();
                if (pi != null)
                {
                    if (piece.deleted != null) pi.deleted = (byte)piece.deleted;
                    if (piece.name != null) pi.name = piece.name;
                    if (piece.typeId != null) pi.typeId = (int)piece.typeId;
                    _context.PieceClasss.Update(pi);
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
                piece_class pi = _context.PieceClasss
                    .Where(piece => piece.id == id)
                    .FirstOrDefault();
                if (pi != null)
                {
                    pi.deleted = deleted;
                    _context.PieceClasss.Update(pi);
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
                    piece_classes = _context.PieceClasss
                        .Where(pi => pi.id == piece)
                        .ToList();
                }
                else
                {
                    piece_classes = _context.PieceClasss
                        .Include(pi => pi.PieceType)
                        .ToList();
                }

                IList<Piece> pieces = new List<Piece>();
                foreach (piece_class piece_class in piece_classes)
                {
                    piece_type pity = _context.PieceTypes
                        .Find(piece_class.typeId);
                    pieces.Add(
                        new(
                            piece_class.id, 
                            piece_class.name, 
                            new PieceType(pity), 
                            piece_class.deleted
                        )
                    );
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
            IList<int> ipls = _context.IncidencePieceLogs
                .Where(ipl => ipl.incidenceId == incidenceId)
                .Select(ipl => ipl.id)
                .ToList();

            IList<piece_class> cleanPieces = _context.PieceClasss
                .Include(ipl => ipl.PieceType)
                .Where(pie => ipls.Contains(pie.id))
                .ToList();

            IList<Piece> pieces = new List<Piece>();
            foreach (piece_class ipl in cleanPieces)
            {
                pieces.Add(
                    new Piece() 
                    { 
                        Deleted = ipl.deleted,
                        Id = ipl.id,
                        Name = ipl.name,
                        Type = new PieceType() 
                        { 
                            Id = ipl.PieceType.id,
                            Name = ipl.PieceType.name, 
                            Description = ipl.PieceType.description 
                        }
                    }
                );
            }

            return pieces;
        }
    }
}
