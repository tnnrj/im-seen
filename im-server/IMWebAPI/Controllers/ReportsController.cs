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
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace IMWebAPI.Controllers
{
    [Authorize(Policy = "WebAppUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IM_API_Context _context;
        private readonly IQueryRunner _queryRunner;

        private static readonly string s_supportingReportJoin = @"
JOIN Delegations ON Delegations.StudentID = Observations.StudentID
JOIN Supporters ON Supporters.StudentGroupID = Delegations.StudentGroupID
WHERE Supporters.UserName = '{UserName}'";

        public ReportsController(IM_API_Context context, IQueryRunner queryRunner)
        {
            _context = context;
            _queryRunner = queryRunner;
        }

        // GET: api/Reports
        [HttpGet]
        [Authorize(Policy = "Reports.Read")]
        public async Task<ActionResult<IEnumerable<Report>>> GetReport()
        {
            return await _context.Reports.ToListAsync();
        }

        // GET: api/Reports/5
        [HttpGet("{id}")]
        [Authorize(Policy = "Reports.Read")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            return report;
        }

        [HttpGet]
        [Route("GetDataForReport")]
        [Authorize(Policy = "Reports.Read")]
        public async Task<ActionResult<string>> GetDataForReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report == null || string.IsNullOrEmpty(report.Query))
            {
                return NotFound();
            }

            var sql = report.Query;
            if (!User.HasClaim("Students.SeeAll", ""))
            {
                sql = sql.Replace("{joinAndWhere}", s_supportingReportJoin).Replace("{UserName}", User.Identity.Name);
            }
            else
            {
                sql = sql.Replace("{joinAndWhere}", "NATURAL JOIN Students WHERE 1=1");
            }

            var data = _queryRunner.ExecuteAsJson(sql);

            var response = new JObject();
            response.Add("name", report.ReportName);
            response.Add("axis1Name", report.Axis1Name);
            response.Add("axis2Name", report.Axis2Name);
            response.Add("data", data);
            return Content(response.ToString(), "application/json");
        }

        // POST: api/Reports
        [HttpPost]
        [Authorize(Policy = "Reports.Create")]
        public async Task<ActionResult<Report>> PostReport(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReport", new { id = report.ReportID }, report);
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Reports.Delete")]
        public async Task<ActionResult<Report>> DeleteReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return report;
        }

        /// <summary>
        /// Policies to access endpoints in this controller
        /// </summary>
        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("Reports.Create", policy => policy.RequireClaim("Reports.Create"));
            options.AddPolicy("Reports.Read", policy => policy.RequireClaim("Reports.Read"));
            options.AddPolicy("Reports.Update", policy => policy.RequireClaim("Reports.Update"));
            options.AddPolicy("Reports.Delete", policy => policy.RequireClaim("Reports.Delete"));
            options.AddPolicy("Reports.Archive", policy => policy.RequireClaim("Reports.Archive"));
        }
    }
}
