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
                new Student {FirstName="James", LastName="Harry", ExternalID="0"},
                new Student {FirstName="Anna", LastName="Hermione", ExternalID="1"},
                new Student {FirstName="Lucius", LastName="Draco", ExternalID="2"},
                new Student {FirstName="Abby", LastName="Marcus", ExternalID="3"},
                new Student {FirstName="Phil", LastName="Woodley", ExternalID="4"},
                new Student {FirstName="Jacob", LastName="Burch", ExternalID="5"},

                new Student {FirstName="Tiffany", LastName="Garza", ExternalID="6"},
                new Student {FirstName="Gary", LastName="Rooney", ExternalID="7"},
                new Student {FirstName="Harry", LastName="Beach", ExternalID="8"},
                new Student {FirstName="Linus", LastName="Goodwin", ExternalID="9"},
                new Student {FirstName="Colin", LastName="Pittman", ExternalID="10"},
                new Student {FirstName="Bill", LastName="Murray", ExternalID="11"},

                new Student {FirstName="Spencer", LastName="Peacock", ExternalID="12"},
                new Student {FirstName="Jess", LastName="Mcghee", ExternalID="13"},
                new Student {FirstName="Peter", LastName="Herring", ExternalID="14"},
                new Student {FirstName="Gavin", LastName="Ryder", ExternalID="15"},
                new Student {FirstName="Fiona", LastName="Prince", ExternalID="16"},
                new Student {FirstName="John", LastName="Cecily", ExternalID="17"},

                new Student {FirstName="Zeke", LastName="Mohammed", ExternalID="18"},
                new Student {FirstName="Tammy", LastName="Cruz", ExternalID="19"},
                new Student {FirstName="Ursula", LastName="Hayward", ExternalID="20"},
                new Student {FirstName="Josh", LastName="Louisa", ExternalID="21"},
                new Student {FirstName="Jeremy", LastName="Zack", ExternalID="22"},
                new Student {FirstName="Rhys", LastName="Nojus", ExternalID="23"},

                new Student {FirstName="Raimi", LastName="Ismail", ExternalID="24"},
                new Student {FirstName="Gus", LastName="Powell", ExternalID="25"},
                new Student {FirstName="Julie", LastName="Maheen", ExternalID="26"},
                new Student {FirstName="Robert", LastName="Zubair", ExternalID="27"},
                new Student {FirstName="Wendy", LastName="Arissa", ExternalID="28"},
                new Student {FirstName="Hank", LastName="Astrid", ExternalID="29"},
            };

            context.Students.AddRange(students);
            context.SaveChanges();
        }
    }
}
