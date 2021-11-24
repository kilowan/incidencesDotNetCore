using Incidences.Business;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraApp.Models.Incidence;
using System.Collections.Generic;

namespace Incidences.Controllers
{
    [Route("api/PieceType")]
    [ApiController]
    public class PieceTypeController : ControllerBase
    {
        private readonly IPieceTypeBz pieceType;
        public PieceTypeController(IPieceTypeBz pieceType)
        {
            this.pieceType = pieceType;
        }

        [HttpGet]
        public IList<PieceType> Index()
        {
            return this.pieceType.SelectPieceTypeList();
        }

        [HttpGet("{id}")]
        public PieceType Details(int id)
        {
            return this.pieceType.SelectPieceTypeById(id);
        }
    }
}
