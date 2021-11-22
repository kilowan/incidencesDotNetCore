using Incidences.Models.Incidence;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraApp.Business;
using MiPrimeraApp.Models.Incidence;
using System.Collections.Generic;

namespace MiPrimeraApp.Controllers
{
    [Route("api/Piece")]
    [ApiController]
    public class PieceController : ControllerBase
    {
        private PieceBz piece;
        public PieceController()
        {
            this.piece = new();
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
            return this.piece.DeletePieceFn(id);
        }

        [HttpPost]
        public bool Create(PieceDto piece)
        {
            return this.piece.AddPieceFn(piece);
        }
    }
}
