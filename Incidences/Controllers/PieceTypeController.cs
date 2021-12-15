using Incidences.Business;
using Microsoft.AspNetCore.Mvc;
using Incidences.Models.Incidence;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public IList<PieceType> Index()
        {
            return this.pieceType.SelectPieceTypeList();
        }

        [HttpGet("{id}")]
        [Authorize]
        public PieceType Details(int id)
        {
            return this.pieceType.SelectPieceTypeById(id);
        }
    }
}
