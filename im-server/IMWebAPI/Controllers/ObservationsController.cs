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
using Microsoft.AspNetCore.Identity;

namespace IMWebAPI.Controllers
{
    [Authorize(Roles = "Administrator, PrimaryActor, SupportingActor")]
    [ApiController]
    [EnableCors]
    [Route("api/Observations")]
    public class ObservationsController : ControllerBase
    {
        private readonly IM_API_Context _context;
        private readonly IQueryable<Observation> myObservQuery;

        public ObservationsController(IM_API_Context context)
        {
            _context = context;

            myObservQuery =
                from observ in _context.Observations

                join student in _context.Students
                on observ.StudentID equals student.StudentID

                join delegation in _context.Delegations
                on student.StudentID equals delegation.Student.StudentID

                join g in _context.Groups
                on delegation.Group.GroupID equals g.GroupID

                join supporter in _context.Supporters
                on g.GroupID equals supporter.Group.GroupID

                where supporter.UserName == User.Identity.Name
                select observ;
        }

        // GET: api/Observations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Observation>>> GetObservation()
        {
            if (User.IsInRole("SupportingActor"))
            {

                return await myObservQuery.ToListAsync();
            }


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

            if (User.IsInRole("SupportingActor"))
            {
                var myObservs = await myObservQuery.ToListAsync();
                if (!myObservs.Contains(observ))
                {
                    return NotFound();
                }
            }

            return observ;
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
            // Match to existing student by name if possible
            var matchingStudent = _context.Students
                .Where(s => (s.FirstName == observ.StudentFirstName) && (s.LastName == observ.StudentLastName))
                .FirstOrDefault();
            if (matchingStudent != null)
                observ.StudentID = matchingStudent.StudentID;

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

        // POST: api/Observations/Update
        [Authorize(Roles = "Administrator, PrimaryActor")]
        [HttpPost("{id}")]
        [Route("Update/{id}")]
        public async Task<ActionResult<Observation>> UpdateObservation(int id, [FromBody] Observation observ)
        {
            if (id != observ.ObservationID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var old_observ = await _context.Observations.FindAsync(id);

                    old_observ.StudentID = observ.StudentID;
                    old_observ.StudentFirstName = observ.StudentFirstName;
                    old_observ.StudentLastName = observ.StudentLastName;

                    old_observ.Severity = observ.Severity;
                    old_observ.Action = observ.Action;
                    old_observ.Event = observ.Event;
                    old_observ.Status = observ.Status;

                    _context.Update(old_observ);
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!ObservationExists(observ.ObservationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Ok();
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
