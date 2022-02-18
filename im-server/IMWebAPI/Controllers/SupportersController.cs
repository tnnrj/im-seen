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

namespace IMWebAPI.Controllers
{
    [Authorize(Policy = "WebAppUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class SupportersController : ControllerBase
    {
        private readonly IM_API_Context _context;

        public SupportersController(IM_API_Context context)
        {
            _context = context;
        }

        // GET: api/Supporters
        [HttpGet]
        [Authorize(Policy = "Supporters.Read")]
        public async Task<ActionResult<IEnumerable<Supporter>>> GetSupporter()
        {
            return await _context.Supporters.ToListAsync();
        }

        // GET: api/Supporters/5
        [HttpGet("{id}")]
        [Authorize(Policy = "Supporters.Read")]
        public async Task<ActionResult<Supporter>> GetSupporter(int id)
        {
            var supporter = await _context.Supporters.FindAsync(id);

            if (supporter == null)
            {
                return NotFound();
            }

            return supporter;
        }

        // POST: api/Supporters
        [HttpPost]
        [Authorize(Policy = "Supporters.Create")]
        public async Task<ActionResult<Supporter>> PostSupporter(Supporter supporter)
        {
            _context.Supporters.Add(supporter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSupporter", new { id = supporter.SupporterID }, supporter);
        }

        // DELETE: api/Supporters/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Supporters.Delete")]
        public async Task<ActionResult<Supporter>> DeleteSupporter(int id)
        {
            var supporter = await _context.Supporters.FindAsync(id);
            if (supporter == null)
            {
                return NotFound();
            }

            _context.Supporters.Remove(supporter);
            await _context.SaveChangesAsync();

            return supporter;
        }

        /// <summary>
        /// Policies to access endpoints in this controller
        /// </summary>
        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("Supporters.Create", policy => policy.RequireClaim("Supporters.Create"));
            options.AddPolicy("Supporters.Read", policy => policy.RequireClaim("Supporters.Read"));
            options.AddPolicy("Supporters.Update", policy => policy.RequireClaim("Supporters.Update"));
            options.AddPolicy("Supporters.Delete", policy => policy.RequireClaim("Supporters.Delete"));
            options.AddPolicy("Supporters.Archive", policy => policy.RequireClaim("Supporters.Archive"));
        }
    }
}
