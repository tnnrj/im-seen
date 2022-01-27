/**
 * This file contains several API endpoints involving the creation, modification, retrieval, and deletion of individual and collections of StudentGroups
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

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
    [Authorize(Roles = "Administrator, PrimaryActor, SupportingActor")]
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
        public async Task<ActionResult<IEnumerable<StudentGroup>>> GetStudentGroup()
        {
            return await _context.StudentGroups.ToListAsync();
        }

        // GET: api/StudentGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentGroup>> GetStudentGroup(int id)
        {
            var @StudentGroup = await _context.StudentGroups.FindAsync(id);

            if (@StudentGroup == null)
            {
                return NotFound();
            }

            return @StudentGroup;
        }

        // PUT: api/StudentGroups/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentGroup(int id, StudentGroup @StudentGroup)
        {
            if (id != @StudentGroup.StudentGroupID)
            {
                return BadRequest();
            }

            _context.Entry(@StudentGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentGroupExists(id))
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

        // POST: api/StudentGroups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<StudentGroup>> PostStudentGroup(StudentGroup @StudentGroup)
        {
            _context.StudentGroups.Add(@StudentGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentGroup", new { id = @StudentGroup.StudentGroupID }, @StudentGroup);
        }

        // POST: api/StudentGroup/Update
        [HttpPost("{id}")]
        [Route("Update/{id}")]
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
    }
}