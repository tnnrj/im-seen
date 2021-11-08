﻿using IMWebAPI.Models;
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
                    Query = "SELECT TRIM(CONCAT(StudentFirstName, ' ', StudentLastName)) as name, SUM(Severity) AS value FROM Observations {joinAndWhere} GROUP BY StudentFirstName, StudentLastName, Observations.StudentID",
                    Axis1Name = "Student",
                    Axis2Name = "Total Observation Severity",
                    AvailableChartTypes = "Bar,Pie,BubbleCloud"
                },
                new Report
                {
                    ReportName = "Observation Frequency by Student and Date",
                    Query = "SELECT TRIM(CONCAT(StudentFirstName, ' ', StudentLastName)) as name, CAST(ObservationDate AS DATE) as date, COUNT(*) as value FROM Observations {joinAndWhere} GROUP BY StudentFirstName, StudentLastName, Observations.StudentID, ObservationDate ORDER BY ObservationDate",
                    Axis1Name = "Date",
                    Axis2Name = "Frequency of Observations",
                    AvailableChartTypes = "Line"
                },
                new Report
                {
                    ReportName = "Observation Severity by Student and Date",
                    Query = "SELECT TRIM(CONCAT(StudentFirstName, ' ', StudentLastName)) as name, CAST(ObservationDate AS DATE) as date, SUM(Severity) as value FROM Observations {joinAndWhere} GROUP BY StudentFirstName, StudentLastName, Observations.StudentID, ObservationDate ORDER BY ObservationDate",
                    Axis1Name = "Date",
                    Axis2Name = "Severity of Observations",
                    AvailableChartTypes = "Line"
                },
                new Report {
                    ReportName = "Observations Grouped by Severity",
                    Query = "SELECT Severity as name, COUNT(Severity) AS value FROM Observations {joinAndWhere} GROUP BY Severity",
                    Axis1Name = "Severity",
                    Axis2Name = "Proportion",
                    AvailableChartTypes = "Bar,Pie,BubbleCloud"
                }
            };

            context.Reports.AddRange(reports);
            context.SaveChanges();
        }
    }
}
