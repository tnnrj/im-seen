using IMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Data
{
    public class Student_Seeding
    {
        public static void SeedStudents(IM_API_Context context)
        {
            // Look for any existing Users.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            var students = new Student[]
            {
                new Student {studentName="Harry"},

                new Student {studentName="Hermione"},

                new Student {studentName="Draco"}
            };

            context.Students.AddRange(students);
            context.SaveChanges();
        }
    }
}
