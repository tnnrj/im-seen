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
            // Look for any existing reports.
            if (context.Reports.Any())
            {
                return;   // DB has been seeded
            }

            var reports = new Report[]
            {
                new Report
                {
                    ReportName = "Students Grouped by Total Observation Severity",
                    Query = "SELECT StudentName as name, SUM(Severity) AS value FROM Observations GROUP BY StudentName",
                    Axis1Name = "Student",
                    Axis2Name = "Total Observation Severity"
                },
                new Report
                {
                    ReportName = "Observation Frequency by Student and Date",
                    Query = "SELECT StudentName as name, CAST(ObservationDate AS DATE) as date, COUNT(*) as value FROM Observations GROUP BY StudentName, CAST(ObservationDate AS DATE) ORDER BY ObservationDate",
                    Axis1Name = "Date",
                    Axis2Name = "Frequency of Observations"
                },
                new Report
                {
                    ReportName = "Observation Severity by Student and Date",
                    Query = "SELECT StudentName as name, CAST(ObservationDate AS DATE) as date, SUM(Severity) as value FROM Observations GROUP BY StudentName, CAST(ObservationDate AS DATE) ORDER BY ObservationDate",
                    Axis1Name = "Date",
                    Axis2Name = "Severity of Observations"
                }
            };

            context.Reports.AddRange(reports);
            context.SaveChanges();
        }
    }
}
