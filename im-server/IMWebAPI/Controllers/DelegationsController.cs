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
    public class DelegationsController : ControllerBase
    {
        private readonly IM_API_Context _context;

        public DelegationsController(IM_API_Context context)
        {
            _context = context;
        }

        // GET: api/Delegations
        [HttpGet]
        [Authorize(Policy = "Delegations.Read")]
        public async Task<ActionResult<IEnumerable<Delegation>>> GetDelegation()
        {
            return await _context.Delegations.ToListAsync();
        }

        // GET: api/Delegations/5
        [HttpGet("{id}")]
        [Authorize(Policy = "Delegations.Read")]
        public async Task<ActionResult<Delegation>> GetDelegation(int id)
        {
            var delegation = await _context.Delegations.FindAsync(id);

            if (delegation == null)
            {
                return NotFound();
            }

            return delegation;
        }

        // POST: api/Delegations
        [HttpPost]
        [Authorize(Policy = "Delegations.Create")]
        public async Task<ActionResult<Delegation>> PostDelegation(Delegation delegation)
        {
            _context.Delegations.Add(delegation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDelegation", new { id = delegation.DelegationID }, delegation);
        }

        // DELETE: api/Delegations/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Delegations.Delete")]
        public async Task<ActionResult<Delegation>> DeleteDelegation(int id)
        {
            var delegation = await _context.Delegations.FindAsync(id);
            if (delegation == null)
            {
                return NotFound();
            }

            _context.Delegations.Remove(delegation);
            await _context.SaveChangesAsync();

            return delegation;
        }

        /// <summary>
        /// Policies to access endpoints in this controller
        /// </summary>
        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("Delegations.Create", policy => policy.RequireClaim("Delegations.Create"));
            options.AddPolicy("Delegations.Read", policy => policy.RequireClaim("Delegations.Read"));
            options.AddPolicy("Delegations.Update", policy => policy.RequireClaim("Delegations.Update"));
            options.AddPolicy("Delegations.Delete", policy => policy.RequireClaim("Delegations.Delete"));
            options.AddPolicy("Delegations.Archive", policy => policy.RequireClaim("Delegations.Archive"));
        }
    }
}