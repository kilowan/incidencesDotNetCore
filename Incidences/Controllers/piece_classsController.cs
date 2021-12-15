using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Incidences.Data;
using Incidences.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Incidences.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class piece_classsController : ControllerBase
    {
        private readonly IncidenceContext _context;

        public piece_classsController(IncidenceContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<piece_class>>> GetPieceClass()
        {
            return await _context.PieceClasss.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<piece_class>> Getpiece_classs(int id)
        {
            var piece_classs = await _context.PieceClasss.FindAsync(id);

            if (piece_classs == null)
            {
                return NotFound();
            }

            return piece_classs;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Putpiece_classs(int id, piece_class piece_classs)
        {
            if (id != piece_classs.id)
            {
                return BadRequest();
            }

            _context.Entry(piece_classs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!piece_classsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<piece_class>> Postpiece_classs(piece_class piece_classs)
        {
            _context.PieceClasss.Add(piece_classs);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (piece_classsExists(piece_classs.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("Getpiece_classs", new { id = piece_classs.id }, piece_classs);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Deletepiece_classs(int id)
        {
            var piece_classs = await _context.PieceClasss.FindAsync(id);
            if (piece_classs == null)
            {
                return NotFound();
            }

            _context.PieceClasss.Remove(piece_classs);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool piece_classsExists(int id)
        {
            return _context.PieceClasss.Any(e => e.id == id);
        }
    }
}
