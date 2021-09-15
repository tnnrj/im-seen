using IMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Data
{
    public class Observation_Seeding
    {
        public static void SeedObservations(IM_API_Context context)
        {
            // Look for any existing Users.
            if (context.Observations.Any())
            {
                return;   // DB has been seeded
            }

            var reports = new Observation[]
            {
                new Observation {ObservationDate=new DateTime(1994,10,31,19,34,0),
                    UserName="Dumbledore",
                    StudentName=context.Students.Where(s => s.LastName == "Harry").Select(s => s.LastName).FirstOrDefault(),
                    Severity=4, Description="Put his own name into the Goblet of Fire even though he is underage. Recklessness is concerning."},

                new Observation {ObservationDate=new DateTime(1991,10,31,18,12,0),
                    UserName="McGonagall",
                    StudentName=context.Students.Where(s => s.LastName == "Hermione").Select(s => s.LastName).FirstOrDefault(),
                    Severity=2, Description="I didn't see her at the Halloween Feast. But another student mentioned she was crying in the bathroom."},

                new Observation {ObservationDate=new DateTime(1994,12,25,22,15,33),
                    UserName="Filch",
                    StudentName=context.Students.Where(s => s.LastName == "Hermione").Select(s => s.LastName).FirstOrDefault(),
                    Severity=1, Description="Towards the end of the night, she was crying on the staircase outside the ballroom. Possibly due to drama with a Weasley boy."},

                new Observation {ObservationDate=new DateTime(1996,11,20,14,46,0),
                    UserName="Filch",
                    StudentName=context.Students.Where(s => s.LastName == "Draco").Select(s => s.LastName).FirstOrDefault(),
                    Severity=3, Description="He was sulking on the seventh floor left corridor, pacing back and forth and looking like he was up to no good. Then he disappeared."},
            };

            context.Observations.AddRange(reports);
            context.SaveChanges();
        }
    }
}
