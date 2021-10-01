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
                new Student {LastName="Harry"},
                new Student {LastName="Hermione"},
                new Student {LastName="Draco"},
                new Student {LastName="Marcus"},
                new Student {LastName="Woodley"},
                new Student {LastName="Burch"},

                new Student {LastName="Garza"},
                new Student {LastName="Rooney"},
                new Student {LastName="Beach"},
                new Student {LastName="Goodwin"},
                new Student {LastName="Pittman"},
                new Student {LastName="Murray"},

                new Student {LastName="Peacock"},
                new Student {LastName="Mcghee"},
                new Student {LastName="Herring"},
                new Student {LastName="Ryder"},
                new Student {LastName="Prince"},
                new Student {LastName="Cecily"},

                new Student {LastName="Mohammed"},
                new Student {LastName="Cruz"},
                new Student {LastName="Hayward"},
                new Student {LastName="Louisa"},
                new Student {LastName="Zack"},
                new Student {LastName="Nojus"},

                new Student {LastName="Ismail"},
                new Student {LastName="Powell"},
                new Student {LastName="Maheen"},
                new Student {LastName="Zubair"},
                new Student {LastName="Arissa"},
                new Student {LastName="Astrid"},
            };

            context.Students.AddRange(students);
            context.SaveChanges();
        }
    }
}
