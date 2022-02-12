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
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace IMWebAPI.Controllers
{
    [Authorize(Roles = "Administrator, PrimaryActor, SupportingActor")]
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
        public async Task<ActionResult<IEnumerable<int>>> GetMyStudents()
        {
            if (User.IsInRole("SupportingActor"))
            {
                return await supporterQuery.Select(s => s.StudentID).ToListAsync();
            }
            else if (User.IsInRole("PrimaryActor"))
            {
                return await primaryQuery.Select(s => s.StudentID).ToListAsync();
            }
            else if (User.IsInRole("Administrator"))
            {
                return await _context.Students.Select(s => s.StudentID).ToListAsync();
            }

            return BadRequest();
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

        // POST: api/Students
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("CSVBulkUpload")]
        public async Task<IActionResult> CSVBulkUpload(IFormFile file)
        {
            // copy file to temp file
            string filePath = null;
            if (file.Length > 0)
            {
                filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
            }

            string[] lines = System.IO.File.ReadAllLines(filePath);
   
            List<Student> students = new List<Student>();

            // reads and adds to list
            for (int i = 1; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split(",");
                Student s = new Student();

                // checks empty fields
                if (String.IsNullOrEmpty(columns[0]) || String.IsNullOrEmpty(columns[2]) || String.IsNullOrEmpty(columns[4]))
                {
                    return BadRequest("Incorrect format. First Name, Last Name, ExternalID cannot be empty.");
                }

                s.FirstName = columns[0];
                s.MiddleName = columns[1];
                s.LastName = columns[2];

                // checks date format
                DateTime dob;
                if (DateTime.TryParse(columns[3], out dob))
                {
                    s.DOB = dob;
                }
                else
                {
                    return BadRequest("Incorrect format. DateOfBirth not in correct format (mm/dd/yyyy).");
                }

                // checks number format
                int extID;
                if (int.TryParse(columns[4], out extID))
                {
                    s.ExternalID = extID;
                }
                else
                {
                    return BadRequest("Incorrect format. ExternalID must be a number.");
                }

                // checks boolean format
                bool archived;
                if (Boolean.TryParse(columns[5], out archived))
                {
                    s.IsArchived = archived;
                }
                else
                {
                    return BadRequest("Incorrect format. IsArchived accepts TRUE or FALSE.");
                }

                students.Add(s);
            }

            // adds and updates to db
            foreach (Student s in students)
            {
                // checks if externalid exists in db
                var stu = await _context.Students.FirstOrDefaultAsync(st => st.ExternalID == s.ExternalID);
                if (stu is null)
                {
                    _context.Students.Add(s);
                }
                else
                {
                    // otherwise, update existing record
                    s.StudentID = stu.StudentID;
                    _context.Entry(stu).CurrentValues.SetValues(s);
                }
            }
            
            // archives missing records
            foreach (Student s in _context.Students)
            {
                if (!students.Exists(el => el.ExternalID == s.ExternalID))
                {
                    _context.Students.Remove(s);
                }
            }

            _context.SaveChanges();

            return Ok(new { name = file.Name, size = file.Length});
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet, DisableRequestSizeLimit]
        [Route("CSVDownload")]
        public async Task<IActionResult> CSVDownload()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "student_template.csv");

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), filePath);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
    }
}
