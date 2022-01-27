/**
 * This file contains several API endpoints involving the creation, modification, retrieval, and deletion of individual and collections of Delegations
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

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
    [Authorize(Roles = "Administrator, PrimaryActor, SupportingActor")]
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
        public async Task<ActionResult<IEnumerable<Delegation>>> GetDelegation()
        {
            return await _context.Delegations.ToListAsync();
        }

        // GET: api/Delegations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Delegation>> GetDelegation(int id)
        {
            var delegation = await _context.Delegations.FindAsync(id);

            if (delegation == null)
            {
                return NotFound();
            }

            return delegation;
        }

        // PUT: api/Delegations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDelegation(int id, Delegation delegation)
        {
            if (id != delegation.DelegationID)
            {
                return BadRequest();
            }

            _context.Entry(delegation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DelegationExists(id))
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

        // POST: api/Delegations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Delegation>> PostDelegation(Delegation delegation)
        {
            _context.Delegations.Add(delegation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDelegation", new { id = delegation.DelegationID }, delegation);
        }

        // DELETE: api/Delegations/5
        [HttpDelete("{id}")]
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

        private bool DelegationExists(int id)
        {
            return _context.Delegations.Any(e => e.DelegationID == id);
        }
    }
}