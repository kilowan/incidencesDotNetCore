using Incidences.Data.Models;
using Incidences.Models.Incidence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Incidences.Data
{
    public class PieceTypeData : IPieceTypeData
    {
        private readonly IncidenceContext _context;

        public PieceTypeData(IncidenceContext context)
        {
            _context = context;
        }

        public IList<PieceType> SelectPieceType()
        {
            try
            {
                IList<piece_type> pitys = _context.PieceTypes.ToList();
                IList<PieceType> pieceTypes = new List<PieceType>();
                foreach (piece_type pity in pitys)
                {
                    pieceTypes.Add(
                        new PieceType()
                        {
                            Description = pity.description,
                            Id = pity.id,
                            Name = pity.name
                        }
                    );
                }
                return pieceTypes;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public PieceType SelectPieceTypeById(int pieceTypeId)
        {
            try
            {
                piece_type pity = _context.PieceTypes
                    .Where(pity => pity.id == pieceTypeId)
                    .FirstOrDefault();

                return new PieceType()
                {
                    Description = pity.description,
                    Id = pity.id,
                    Name = pity.name
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
