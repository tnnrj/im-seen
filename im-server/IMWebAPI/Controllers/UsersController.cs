using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMLibrary.Data;
using IMLibrary.Models;
using IMLibrary.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IMWebAPI.Controllers
{
    [Authorize(Policy = "WebAppUser")]
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
        [Authorize(Policy = "Users.Read")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Policy = "Users.Read")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Policy = "Users.Create")]
        public async Task<ActionResult<ApplicationUser>> PostUser(ApplicationUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserName }, user);
        }

        // POST: api/Users/Update
        [HttpPost]
        [Route("Update")]
        [Authorize(Policy = "Users.Update")]
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
        [HttpDelete("{id}")]
        [Authorize(Policy = "Users.Delete")]
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

        /// <summary>
        /// Policies to access endpoints in this controller
        /// </summary>
        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("Users.Create", policy => policy.RequireClaim("Users.Create"));
            options.AddPolicy("Users.Read", policy => policy.RequireClaim("Users.Read"));
            options.AddPolicy("Users.Update", policy => policy.RequireClaim("Users.Update"));
            options.AddPolicy("Users.Delete", policy => policy.RequireClaim("Users.Delete"));
            options.AddPolicy("Users.Archive", policy => policy.RequireClaim("Users.Archive"));
        }
    }
}
