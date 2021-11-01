using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMWebAPI.Data;
using IMWebAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace IMWebAPI.Controllers
{
    [Authorize(Roles = "Administrator, PrimaryActor, SupportingActor")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IM_API_Context _context;
        private readonly IQueryable<Student> supporterQuery;
        private readonly IQueryable<int> primaryQuery;

        public StudentsController(IM_API_Context context)
        {
            _context = context;
            supporterQuery =
                from student in _context.Students

                join delegation in _context.Delegations
                on student.StudentID equals delegation.Student.StudentID

                join g in _context.Groups
                on delegation.Group.GroupID equals g.GroupID

                join supporter in _context.Supporters
                on g.GroupID equals supporter.Group.GroupID

                where supporter.UserName == User.Identity.Name
                select student;

            primaryQuery =
                from student in _context.Students

                join delegation in _context.Delegations
                on student.StudentID equals delegation.Student.StudentID

                join g in _context.Groups
                on delegation.Group.GroupID equals g.GroupID

                where g.PrimaryUserName == User.Identity.Name
                select student.StudentID;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            if (User.IsInRole("SupportingActor"))
            {
                return await supporterQuery.ToListAsync();
            }

            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/MyStudents
        [Authorize(Roles = "Administrator, PrimaryActor")]
        [HttpGet]
        [Route("MyStudents")]
        public async Task<ActionResult<IEnumerable<int>>> GetMyStudents()
        {

            return await primaryQuery.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            if (User.IsInRole("SupportingActor"))
            {
                var myStudents = await supporterQuery.ToListAsync();
                if (!myStudents.Contains(student))
                {
                    return NotFound();
                }
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentID }, student);
        }

        // DELETE: api/Students/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
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

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
    }
}
