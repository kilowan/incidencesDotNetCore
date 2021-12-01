using Incidences.Business;
using Incidences.Models.Incidence;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Incidences.Controllers
{
    [Route("api/Piece")]
    [ApiController]
    public class PieceController : ControllerBase
    {
        private readonly IPieceBz piece;
        public PieceController(IPieceBz piece)
        {
            this.piece = piece;
        }

        [HttpGet]
        public IList<Piece> Index()
        {
            return this.piece.GetPieces(null);
        }

        [HttpGet("{id}")]
        public IList<Piece> Details(int id)
        {
            return this.piece.GetPieces(id);
        }

        [HttpPut("{id}")]
        public bool Update(PieceDto piece, int id)
        {
            return this.piece.UpdatePiece(piece, id);
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return this.piece.DeletePiece(id);
        }

        [HttpPost]
        public bool Create(PieceDto piece)
        {
            return this.piece.AddPiece(piece);
        }
    }
}
