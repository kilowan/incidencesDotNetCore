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

        // GET: PieceController
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
        /*
        // GET: PieceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PieceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PieceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PieceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PieceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PieceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
