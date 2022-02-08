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
    public class StudentsController : ControllerBase
    {
        private readonly IM_API_Context _context;
        private readonly IQueryable<Student> supporterQuery;
        private readonly IQueryable<Student> primaryQuery;

        public StudentsController(IM_API_Context context)
        {
            _context = context;
            supporterQuery =
                from student in _context.Students

                join delegation in _context.Delegations
                on student.StudentID equals delegation.Student.StudentID

                join g in _context.StudentGroups
                on delegation.StudentGroup.StudentGroupID equals g.StudentGroupID

                join supporter in _context.Supporters
                on g.StudentGroupID equals supporter.StudentGroup.StudentGroupID

                where supporter.UserName == User.Identity.Name
                select student;

            primaryQuery =
                from student in _context.Students

                join delegation in _context.Delegations
                on student.StudentID equals delegation.Student.StudentID

                join g in _context.StudentGroups
                on delegation.StudentGroup.StudentGroupID equals g.StudentGroupID

                where g.PrimaryUserName == User.Identity.Name
                select student;
        }

        // GET: api/Students
        [HttpGet]
        [Authorize(Policy = "Students.Read")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            if (User.IsInRole("SupportingActor"))
            {
                return await supporterQuery.ToListAsync();
            }

            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/MyStudents
        [HttpGet]
        [Route("MyStudents")]
        [Authorize(Policy = "Students.Read")]
        public async Task<ActionResult<IEnumerable<int>>> GetMyStudents()
        {
            return await StudentsForUser(true).Select(s => s.StudentID).ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        [Authorize(Policy = "Students.Read")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await StudentsForUser().FirstOrDefaultAsync(s => s.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // POST: api/Students
        [HttpPost]
        [Authorize(Policy = "Students.Create")]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentID }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Students.Delete")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return student;
        }

        private IQueryable<Student> StudentsForUser(bool forceFilter = false)
        {
            // claim allows a user to see all students
            // for now, admins never have students assigned
            if (User.IsInRole("Administrator") || (User.HasClaim("Students.SeeAll", "") && !forceFilter))
            {
                return from student in _context.Students
                       select student;
            }

            return from student in _context.Students

                join delegation in _context.Delegations
                on student.StudentID equals delegation.Student.StudentID

                join g in _context.StudentGroups
                on delegation.StudentGroup.StudentGroupID equals g.StudentGroupID

                from supporter in _context.Supporters
                where g.StudentGroupID == supporter.StudentGroup.StudentGroupID

                where supporter.UserName == User.Identity.Name || g.PrimaryUserName == User.Identity.Name
                select student;
        }

        /// <summary>
        /// Policies to access endpoints in this controller
        /// </summary>
        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("Students.Create", policy => policy.RequireClaim("Students.Create"));
            options.AddPolicy("Students.Read", policy => policy.RequireClaim("Students.Read"));
            options.AddPolicy("Students.Update", policy => policy.RequireClaim("Students.Update"));
            options.AddPolicy("Students.Delete", policy => policy.RequireClaim("Students.Delete"));
            options.AddPolicy("Students.Archive", policy => policy.RequireClaim("Students.Archive"));
            options.AddPolicy("Students.SeeAll", policy => policy.RequireClaim("Students.SeeAll"));
        }
    }
}
