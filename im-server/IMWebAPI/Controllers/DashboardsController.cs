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
    public class DashboardsController : ControllerBase
    {
        private readonly IM_API_Context _context;

        public DashboardsController(IM_API_Context context)
        {
            _context = context;
        }

        // GET: api/GetMyDashboard
        [HttpGet]
        [Authorize(Policy = "Dashboards.Read")]
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

        // DELETE: api/Dashboards/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Dashboards.Delete")]
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

        /// <summary>
        /// Policies to access endpoints in this controller
        /// </summary>
        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("Dashboards.Create", policy => policy.RequireClaim("Dashboards.Create"));
            options.AddPolicy("Dashboards.Read", policy => policy.RequireClaim("Dashboards.Read"));
            options.AddPolicy("Dashboards.Update", policy => policy.RequireClaim("Dashboards.Update"));
            options.AddPolicy("Dashboards.Delete", policy => policy.RequireClaim("Dashboards.Delete"));
            options.AddPolicy("Dashboards.Archive", policy => policy.RequireClaim("Dashboards.Archive"));
        }
    }
}

