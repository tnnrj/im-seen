using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMLibrary.Data;
using IMLibrary.Models;
using IMLibrary.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace IMWebAPI.Controllers
{
    [Authorize(Policy = "WebAppUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentGroupsController : ControllerBase
    {
        private readonly IM_API_Context _context;

        public StudentGroupsController(IM_API_Context context)
        {
            _context = context;
        }

        // GET: api/StudentGroups
        [HttpGet]
        [Authorize(Policy = "StudentGroups.Read")]
        public async Task<ActionResult<IEnumerable<StudentGroup>>> GetStudentGroup()
        {
            return await _context.StudentGroups.ToListAsync();
        }

        // GET: api/StudentGroups/5
        [HttpGet("{id}")]
        [Authorize(Policy = "StudentGroups.Read")]
        public async Task<ActionResult<StudentGroup>> GetStudentGroup(int id)
        {
            var @StudentGroup = await _context.StudentGroups.FindAsync(id);

            if (@StudentGroup == null)
            {
                return NotFound();
            }

            return @StudentGroup;
        }

        // POST: api/StudentGroups
        [HttpPost]
        [Authorize(Policy = "StudentGroups.Create")]
        public async Task<ActionResult<StudentGroup>> PostStudentGroup(StudentGroup @StudentGroup)
        {
            _context.StudentGroups.Add(@StudentGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentGroup", new { id = @StudentGroup.StudentGroupID }, @StudentGroup);
        }

        // POST: api/StudentGroup/Update
        [HttpPost("{id}")]
        [Route("Update/{id}")]
        [Authorize(Policy = "StudentGroups.Update")]
        public async Task<ActionResult<Observation>> Update(int id, [FromBody] StudentGroup group)
        {
            if (id != group.StudentGroupID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var old_group = await _context.StudentGroups.FindAsync(id);

                    var primary_user = await _context.Users.Where(u => u.UserName == group.PrimaryUserName).SingleOrDefaultAsync();
                    if (primary_user.UserName == group.PrimaryUserName && primary_user.Role == "PrimaryActor")
                        old_group.PrimaryUserName = group.PrimaryUserName;
                    else

                    old_group.StudentGroupName = group.StudentGroupName;


                    _context.Update(old_group);
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentGroupExists(group.StudentGroupID))
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

        // DELETE: api/StudentGroups/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "StudentGroups.Delete")]
        public async Task<ActionResult<StudentGroup>> DeleteStudentGroup(int id)
        {
            var @StudentGroup = await _context.StudentGroups.FindAsync(id);
            if (@StudentGroup == null)
            {
                return NotFound();
            }

            _context.StudentGroups.Remove(@StudentGroup);
            await _context.SaveChangesAsync();

            return @StudentGroup;
        }

        private bool StudentGroupExists(int id)
        {
            return _context.StudentGroups.Any(e => e.StudentGroupID == id);
        }

        /// <summary>
        /// Policies to access endpoints in this controller
        /// </summary>
        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("StudentGroups.Create", policy => policy.RequireClaim("StudentGroups.Create"));
            options.AddPolicy("StudentGroups.Read", policy => policy.RequireClaim("StudentGroups.Read"));
            options.AddPolicy("StudentGroups.Update", policy => policy.RequireClaim("StudentGroups.Update"));
            options.AddPolicy("StudentGroups.Delete", policy => policy.RequireClaim("StudentGroups.Delete"));
            options.AddPolicy("StudentGroups.Archive", policy => policy.RequireClaim("StudentGroups.Archive"));
        }
    }
}