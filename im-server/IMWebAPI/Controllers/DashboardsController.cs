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
    public class DashboardsController : ControllerBase
    {
        private readonly IM_API_Context _context;

        public DashboardsController(IM_API_Context context)
        {
            _context = context;
        }

        // GET: api/Dashboards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dashboard>>> GetDashboard()
        {
            return await _context.Dashboards.ToListAsync();
        }

        // GET: api/Dashboards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dashboard>> GetDashboard(int id)
        {
            var dashboard = await _context.Dashboards.FindAsync(id);

            if (dashboard == null)
            {
                return NotFound();
            }

            return dashboard;
        }

        // GET: api/GetMyDashboard
        [HttpGet]
        [Route("GetMyDashboard")]
        public ActionResult<Dashboard> GetMyDashboard()
        {
            var dashboard = _context.Dashboards.FirstOrDefault(d => d.UserName == User.Identity.Name);

            if (dashboard == null)
            {
                // default dashboard configuration with no user assigned
                dashboard = _context.Dashboards.FirstOrDefault(d => string.IsNullOrEmpty(d.UserName));
            }

            return dashboard;
        }

        [HttpPost]
        [Route("UpdateMyDashboard")]
        public async Task<IActionResult> UpdateMyDashboard([FromBody] string dashboardText)
        {
            var dashboard = _context.Dashboards.FirstOrDefault(d => d.UserName == User.Identity.Name);

            if (dashboard == null)
            {
                _context.Dashboards.Add(new Dashboard
                {
                    UserName = User.Identity.Name,
                    DashboardText = dashboardText
                });
            }
            else
            {
                dashboard.DashboardText = dashboardText;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Dashboards/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDashboard(int id, Dashboard dashboard)
        {
            if (id != dashboard.DashboardID)
            {
                return BadRequest();
            }

            _context.Entry(dashboard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DashboardExists(id))
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

        // POST: api/Dashboards
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Dashboard>> PostDashboard(Dashboard dashboard)
        {
            _context.Dashboards.Add(dashboard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDashboard", new { id = dashboard.DashboardID }, dashboard);
        }

        // DELETE: api/Dashboards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dashboard>> DeleteDashboard(int id)
        {
            var dashboard = await _context.Dashboards.FindAsync(id);
            if (dashboard == null)
            {
                return NotFound();
            }

            _context.Dashboards.Remove(dashboard);
            await _context.SaveChangesAsync();

            return dashboard;
        }

        private bool DashboardExists(int id)
        {
            return _context.Dashboards.Any(e => e.DashboardID == id);
        }
    }
}

