using IMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Data
{
    public class Report_Seeding
    {
        public static void SeedReports(IM_API_Context context)
        {
            // Look for any existing Users.
            if (context.Reports.Any())
            {
                return;   // DB has been seeded
            }

            var reports = new Report[]
            {
                new Report {reportDate=new DateTime(1994,10,31,19,34,0),
                    reporter=context.Users.Where(u => u.userName == "Dumbledore").Select(u => u.userID).FirstOrDefault(),
                    studentName=context.Students.Where(s => s.studentName == "Harry").Select(s => s.studentName).FirstOrDefault(),
                    severity=4, description="Put his own name into the Goblet of Fire even though he is underage. Recklessness is concerning."},

                new Report {reportDate=new DateTime(1991,10,31,18,12,0),
                    reporter=context.Users.Where(u => u.userName == "McGonagall").Select(u => u.userID).FirstOrDefault(),
                    studentName=context.Students.Where(s => s.studentName == "Hermione").Select(s => s.studentName).FirstOrDefault(),
                    severity=2, description="I didn't see her at the Halloween Feast. But another student mentioned she was crying in the bathroom."},

                new Report {reportDate=new DateTime(1994,12,25,22,15,33),
                    reporter=context.Users.Where(u => u.userName == "Filch").Select(u => u.userID).FirstOrDefault(),
                    studentName=context.Students.Where(s => s.studentName == "Hermione").Select(s => s.studentName).FirstOrDefault(),
                    severity=1, description="Towards the end of the night, she was crying on the staircase outside the ballroom. Possibly due to drama with a Weasley boy."},

                new Report {reportDate=new DateTime(1994,12,25,22,15,33),
                    reporter=context.Users.Where(u => u.userName == "Filch").Select(u => u.userID).FirstOrDefault(),
                    studentName=context.Students.Where(s => s.studentName == "Draco").Select(s => s.studentName).FirstOrDefault(),
                    severity=3, description="He was sulking on the seventh floor left corridor, pacing back and forth and looking like he was up to no good. Then he disappeared."},
            };

            context.Reports.AddRange(reports);
            context.SaveChanges();
        }
    }
}
