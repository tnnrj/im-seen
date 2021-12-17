/**
 * This file contains several API endpoints involving the creation, modification, retrieval, and deletion of individual and collections of Users
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMWebAPI.Data;
using IMWebAPI.Models;
using IMWebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IMWebAPI.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IM_API_Context _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(IM_API_Context context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, ApplicationUser user)
        {
            if (id != user.UserName)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostUser(ApplicationUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserName }, user);
        }

        // POST: api/Users/Update
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult<Observation>> Update([FromBody] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var old_user = await _userManager.FindByNameAsync(user.UserName);

                    old_user.FirstName = user.FirstName;
                    old_user.LastName = user.LastName;

                    old_user.JobTitle = user.JobTitle;

                    if (user.Role == "Administrator" || user.Role == "PrimaryActor" || user.Role == "SupportingActor" || user.Role == "Observer")
                    {
                        var currRoles = await _userManager.GetRolesAsync(old_user);
                        await _userManager.RemoveFromRolesAsync(old_user, currRoles);
                        await _userManager.AddToRoleAsync(old_user, user.Role);

                        old_user.Role = user.Role;
                    }

                    var result = await _userManager.UpdateAsync(old_user);

                    if (!result.Succeeded)
                        return new BadRequestObjectResult(new { Message = "User update failed." });

                    return Ok(new { Message = "User information has been updated." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationUser>> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserName == id);
        }
    }
}
