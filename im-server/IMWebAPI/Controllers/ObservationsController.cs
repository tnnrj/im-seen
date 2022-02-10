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
        private readonly bool _allowAnonymous;
        private readonly IObservationLogic _observationLogic;

        public ObservationsController(IM_API_Context context, IObservationLogic observationLogic, IConfiguration configuration)
        {
            _context = context;
            _allowAnonymous = configuration.GetSection("AppSettings").GetValue<bool>("AllowAnonymousObservation");
            _observationLogic = observationLogic;
        }

        // GET: api/Observations
        [HttpGet]
        [Authorize(Policy = "Observations.Read")]
        public async Task<ActionResult<IEnumerable<Observation>>> GetObservation()
        {
            return await ObservationsForUser().ToListAsync();
        }

        // GET: api/Observations/5
        [HttpGet("{id}")]
        [Authorize(Policy = "Observations.Read")]
        public async Task<ActionResult<Observation>> GetObservation(int id)
        {
            var observ = await ObservationsForUser().FirstOrDefaultAsync(o => o.ObservationID == id);

            if (observ == null)
            {
                return NotFound();
            }

            return observ;
        }

        // POST: api/Observations
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Observation>> PostObservation(Observation observ)
        {
            if (!_allowAnonymous && (!User?.HasClaim("Observations.Create", "") ?? false))
            {
                throw new NotAuthorizedException();
            }

            // Match to existing student by name if possible
            var matchingStudent = _observationLogic.MatchingStudent(observ);
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
        [Authorize(Policy = "Observations.Update")]
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
        [HttpDelete("{id}")]
        [Authorize(Policy = "Observations.Delete")]
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

        private IQueryable<Observation> ObservationsForUser(bool forceFilter = false)
        {
            // claim allows a user to see all students
            // for now, admins never have students assigned
            if (User.IsInRole("Administrator") || (User.HasClaim("Observations.SeeAll", "") && !forceFilter))
            {
                return from observ in _context.Observations
                       select observ;
            }

            return from observ in _context.Observations

                join student in _context.Students
                on observ.StudentID equals student.StudentID

                join delegation in _context.Delegations
                on student.StudentID equals delegation.Student.StudentID

                join g in _context.StudentGroups
                on delegation.StudentGroup.StudentGroupID equals g.StudentGroupID

                from s in _context.Supporters
                where g.StudentGroupID == s.StudentGroup.StudentGroupID

                where s.UserName == User.Identity.Name || g.PrimaryUserName == User.Identity.Name
                select observ;
        }

        /// <summary>
        /// Policies to access endpoints in this controller
        /// </summary>
        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("Observations.Create", policy => policy.RequireClaim("Observations.Create"));
            options.AddPolicy("Observations.Read", policy => policy.RequireClaim("Observations.Read"));
            options.AddPolicy("Observations.Update", policy => policy.RequireClaim("Observations.Update"));
            options.AddPolicy("Observations.Delete", policy => policy.RequireClaim("Observations.Delete"));
            options.AddPolicy("Observations.Archive", policy => policy.RequireClaim("Observations.Archive"));
            options.AddPolicy("Observations.SeeAll", policy => policy.RequireClaim("Observations.SeeAll"));
        }
    }
}
