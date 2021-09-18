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
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace IMWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IM_API_Context _context;
        private readonly QueryRunner _queryRunner;

        public ReportsController(IM_API_Context context, QueryRunner queryRunner)
        {
            _context = context;
            _queryRunner = queryRunner;
        }

        // GET: api/Reports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetReport()
        {
            return await _context.Reports.ToListAsync();
        }

        // GET: api/Reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            return report;
        }

        [Authorize]
        [HttpGet]
        [Route("GetDataForReport")]
        public async Task<ActionResult<string>> GetDataForReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report == null || string.IsNullOrEmpty(report.Query))
            {
                return NotFound();
            }

            var data = _queryRunner.ExecuteAsJson(report.Query);

            var response = new JObject();
            response.Add("name", report.ReportName);
            response.Add("data", data);
            return Content(response.ToString(), "application/json");
        }

        // PUT: api/Reports/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReport(int id, Report report)
        {
            if (id != report.ReportID)
            {
                return BadRequest();
            }

            _context.Entry(report).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(id))
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

        // POST: api/Reports
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Report>> PostReport(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReport", new { id = report.ReportID }, report);
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
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

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.ReportID == id);
        }
    }
}
