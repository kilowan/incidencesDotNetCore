using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Incidences.Data;
using Incidences.Data.Models;

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

        // GET: api/piece_classs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<piece_class>>> GetPieceClass()
        {
            return await _context.PieceClasss.ToListAsync();
        }

        // GET: api/piece_classs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<piece_class>> Getpiece_classs(int id)
        {
            var piece_classs = await _context.PieceClasss.FindAsync(id);

            if (piece_classs == null)
            {
                return NotFound();
            }

            return piece_classs;
        }

        // PUT: api/piece_classs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

        // POST: api/piece_classs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
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

        // DELETE: api/piece_classs/5
        [HttpDelete("{id}")]
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
