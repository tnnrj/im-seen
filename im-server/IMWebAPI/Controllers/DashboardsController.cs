﻿using System;
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
    public class DashboardsController : ControllerBase
    {
        private readonly IM_API_Context _context;

        public DashboardsController(IM_API_Context context)
        {
            _context = context;
        }

        // unused actions are commented for now

        // GET: api/Dashboards
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Dashboard>>> GetDashboard()
        //{
        //    return await _context.Dashboards.ToListAsync();
        //}

        //// GET: api/Dashboards/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Dashboard>> GetDashboard(int id)
        //{
        //    var dashboard = await _context.Dashboards.FindAsync(id);

        //    if (dashboard == null)
        //    {
        //        return NotFound();
        //    }

        //    return dashboard;
        //}

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

        // POST: api/UpdateMyDashboard
        [HttpPost]
        [Authorize(Policy = "Dashboards.Update")]
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

        // POST: api/Dashboards
        //[HttpPost]
        //public async Task<ActionResult<Dashboard>> PostDashboard(Dashboard dashboard)
        //{
        //    _context.Dashboards.Add(dashboard);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetDashboard", new { id = dashboard.DashboardID }, dashboard);
        //}

        // DELETE: api/Dashboards/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Dashboard>> DeleteDashboard(int id)
        //{
        //    var dashboard = await _context.Dashboards.FindAsync(id);
        //    if (dashboard == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Dashboards.Remove(dashboard);
        //    await _context.SaveChangesAsync();

        //    return dashboard;
        //}

        //private bool DashboardExists(int id)
        //{
        //    return _context.Dashboards.Any(e => e.DashboardID == id);
        //}
    }
}

