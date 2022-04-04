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
    [Authorize(Policy = "WebAppUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IM_API_Context _context;

        public StudentsController(IM_API_Context context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        [Authorize(Policy = "Students.Read")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            return await StudentsForUser().ToListAsync();
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

        // PUT: api/Students/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Policy = "Students.Update")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            
            return NoContent();
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

        [Authorize(Policy = "Students.Update")]
        [Authorize(Policy = "Students.SeeAll")]
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
                s.ExternalID = columns[4];

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
                    //_context.Students.Remove(s);
                    s.IsArchived = true;
                }
            }

            _context.SaveChanges();

            return Ok(new { name = file.Name, size = file.Length});
        }

        [Authorize(Policy = "Students.Update")]
        [Authorize(Policy = "Students.SeeAll")]
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
