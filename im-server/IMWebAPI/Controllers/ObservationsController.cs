using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMWebAPI.Data;
using IMWebAPI.Models;
using Microsoft.AspNetCore.Cors;
using IMWebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace IMWebAPI.Controllers
{
    [Authorize(Roles = "Administrator, PrimaryActor, SupportingActor")]
    [ApiController]
    [EnableCors]
    [Route("api/Observations")]
    public class ObservationsController : ControllerBase
    {
        private readonly IM_API_Context _context;

        public ObservationsController(IM_API_Context context)
        {
            _context = context;
        }

        // GET: api/Observations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Observation>>> GetObservation()
        {
            return await _context.Observations.ToListAsync();
        }

        // GET: api/Observations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Observation>> GetObservation(int id)
        {
            var observ = await _context.Observations.FindAsync(id);

            if (observ == null)
            {
                return NotFound();
            }

            return observ;
        }

        [HttpGet]//, Authorize]
        [Route("Student-Severity")]
        public async Task<ActionResult<IEnumerable<Object>>> GetObservationsSeverity()
        {
            var query = await _context.Observations.GroupBy(observ => observ.StudentName)
                .Select(group => new { 
                                        studentName = group.Key, 
                                        severity = group.Sum(g => g.Severity)
                                     }).ToListAsync();

            if (query == null)
            {
                return NotFound();
            }

            return query;
        }

        // PUT: api/Observations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObservation(int id, Observation observ)
        {
            if (id != observ.ObservationID)
            {
                return BadRequest();
            }

            _context.Entry(observ).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObservationExists(id))
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

        // POST: api/Observations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Observation>> PostObservation(Observation observ)
        {
            _context.Observations.Add(observ);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ObservationExists(observ.ObservationID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetObservation", new { id = observ.ObservationID }, observ);
        }

        // DELETE: api/Observations/5
        [Authorize(Roles = "Administrator, PrimaryActor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Observation>> DeleteObservation(int id)
        {
            var observ = await _context.Observations.FindAsync(id);
            if (observ == null)
            {
                return NotFound();
            }

            _context.Observations.Remove(observ);
            await _context.SaveChangesAsync();

            return observ;
        }

        private bool ObservationExists(int id)
        {
            return _context.Observations.Any(e => e.ObservationID == id);
        }
    }
}
