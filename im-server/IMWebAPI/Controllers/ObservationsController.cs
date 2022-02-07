using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMLibrary.Data;
using IMLibrary.Models;
using Microsoft.AspNetCore.Cors;
using IMLibrary.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using IMWebAPI.Models.Exceptions;
using IMLibrary.Logic;

namespace IMWebAPI.Controllers
{
    [Authorize(Policy = "WebAppUser")]
    [ApiController]
    [EnableCors]
    [Route("api/Observations")]
    public class ObservationsController : ControllerBase
    {
        private readonly IM_API_Context _context;
        private readonly IQueryable<Observation> _myObservations;
        private readonly bool _allowAnonymous;
        private readonly IObservationLogic _observationLogic;

        public ObservationsController(IM_API_Context context, IObservationLogic observationLogic, IConfiguration configuration)
        {
            _context = context;
            _allowAnonymous = configuration.GetSection("AppSettings").GetValue<bool>("AllowAnonymousObservation");
            _observationLogic = observationLogic;

            _myObservations =
                from observ in _context.Observations

                join student in _context.Students
                on observ.StudentID equals student.StudentID

                join delegation in _context.Delegations
                on student.StudentID equals delegation.Student.StudentID

                join g in _context.StudentGroups
                on delegation.StudentGroup.StudentGroupID equals g.StudentGroupID

                join supporter in _context.Supporters
                on g.StudentGroupID equals supporter.StudentGroup.StudentGroupID

                where supporter.UserName == User.Identity.Name
                select observ;
        }

        // GET: api/Observations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Observation>>> GetObservation()
        {
            if (User.IsInRole("SupportingActor"))
            {
                return await _myObservations.ToListAsync();
            }


            return await _context.Observations.ToListAsync();
        }

        // GET: api/Observations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Observation>> GetObservation(int id)
        {
            var observ = User.IsInRole("SupportingActor")
                ? await _myObservations.FirstOrDefaultAsync(o => o.ObservationID == id)
                : await _context.Observations.FindAsync(id);

            if (observ == null)
            {
                return NotFound();
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
            if (!_allowAnonymous && (!User?.Claims.Any() ?? false))
            {
                throw new NotAuthorizedException();
            }

            // Match to existing student by name if possible
            var matchingStudent = _context.Students
                .Where(s => (s.FirstName == observ.StudentFirstName) && (s.LastName == observ.StudentLastName))
                .FirstOrDefault();
            if (matchingStudent != null)
                observ.StudentID = matchingStudent.StudentID;

            observ.Status = "New";

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
                    var oldObs = await _context.Observations.FindAsync(id);

                    if (_observationLogic.NeedsScoreRecalc(oldObs, observ))
                    {
                        oldObs.WeightedScore = _observationLogic.CalculateWeightedScore(observ);
                    }

                    oldObs.StudentID = observ.StudentID;
                    oldObs.StudentFirstName = observ.StudentFirstName;
                    oldObs.StudentLastName = observ.StudentLastName;

                    oldObs.Severity = observ.Severity;
                    oldObs.Action = observ.Action;
                    oldObs.Event = observ.Event;
                    oldObs.Status = observ.Status;

                    _context.Update(oldObs);
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

            return Ok("Updated");
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
