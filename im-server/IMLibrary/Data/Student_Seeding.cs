/**
 * This file contains methods for seeding the Students table of the IM_API_Context
 * Written by Steven Carpadakis & Tanner Jorgensen, U of U School of Computing, Senior Capstone 2021
 **/

using IMLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Data
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
                new Student {FirstName="James", LastName="Harry"},
                new Student {FirstName="Anna", LastName="Hermione"},
                new Student {FirstName="Lucius", LastName="Draco"},
                new Student {FirstName="Abby", LastName="Marcus"},
                new Student {FirstName="Phil", LastName="Woodley"},
                new Student {FirstName="Jacob", LastName="Burch"},

                new Student {FirstName="Tiffany", LastName="Garza"},
                new Student {FirstName="Gary", LastName="Rooney"},
                new Student {FirstName="Harry", LastName="Beach"},
                new Student {FirstName="Linus", LastName="Goodwin"},
                new Student {FirstName="Colin", LastName="Pittman"},
                new Student {FirstName="Bill", LastName="Murray"},

                new Student {FirstName="Spencer", LastName="Peacock"},
                new Student {FirstName="Jess", LastName="Mcghee"},
                new Student {FirstName="Peter", LastName="Herring"},
                new Student {FirstName="Gavin", LastName="Ryder"},
                new Student {FirstName="Fiona", LastName="Prince"},
                new Student {FirstName="John", LastName="Cecily"},

                new Student {FirstName="Zeke", LastName="Mohammed"},
                new Student {FirstName="Tammy", LastName="Cruz"},
                new Student {FirstName="Ursula", LastName="Hayward"},
                new Student {FirstName="Josh", LastName="Louisa"},
                new Student {FirstName="Jeremy", LastName="Zack"},
                new Student {FirstName="Rhys", LastName="Nojus"},

                new Student {FirstName="Raimi", LastName="Ismail"},
                new Student {FirstName="Gus", LastName="Powell"},
                new Student {FirstName="Julie", LastName="Maheen"},
                new Student {FirstName="Robert", LastName="Zubair"},
                new Student {FirstName="Wendy", LastName="Arissa"},
                new Student {FirstName="Hank", LastName="Astrid"},
            };

            context.Students.AddRange(students);
            context.SaveChanges();
        }
    }
}
