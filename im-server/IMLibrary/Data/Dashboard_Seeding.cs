using IMLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Data
{
    public class Dashboard_Seeding
    {
		private static string _defaultDash = @"[
	{
		""layout"": ""ThreeTwoTB"",
		""elements"": [
			{
				""reportID"": {score},
				""chartType"": ""Bar"",
				""width"": 50,
				""height"": 50
			},
			{
				""reportID"": {score},
				""chartType"": ""BubbleCloud"",
				""width"": 50,
				""height"": 50
			},
			{
				""reportID"": {sevDate},
				""chartType"": ""Line"",
				""width"": 100,
				""height"": 50
			}
		]
	},
	{
		""layout"": ""FourOneTB"",
		""elements"": [
			{
				""reportID"": {freqDate},
				""chartType"": ""Line"",
				""width"": 100,
				""height"": 50
			},
			{
				""reportID"": {sev},
				""chartType"": ""Pie"",
				""width"": 33.33,
				""height"": 50
			},
			{
				""reportID"": {score},
				""chartType"": ""BubbleCloud"",
				""width"": 33.33,
				""height"": 50
			},
			{
				""reportID"": {score},
				""chartType"": ""Pie"",
				""width"": 33.34,
				""height"": 50
			}
		]
	},
	{
		""layout"": ""FourTB"",
		""elements"": [
			{
				""reportID"": {sevDate},
				""chartType"": ""Line"",
				""width"": 75.14,
				""height"": 52.19
			},
			{
				""reportID"": 0,
				""chartType"": ""None"",
				""width"": 24.86,
				""height"": 52.19
			},
			{
				""reportID"": 0,
				""chartType"": ""None"",
				""width"": 25.14,
				""height"": 47.81
			},
			{
				""reportID"": {freqDate},
				""chartType"": ""Line"",
				""width"": 74.86,
				""height"": 47.81
			}
		]
	}
]";

		public static void SeedDashboards(IM_API_Context context)
        {
            // Look for any existing Users.
            if (context.Dashboards.Any())
            {
                return;   // DB has been seeded
            }

			var dashboards = new Dashboard[]
            {
                new Dashboard
                { // default dashboard for everyone, no username
                    DashboardText = _defaultDash
						.Replace("{score}", context.Reports.First(r => r.ReportName == "Students Grouped by Total Observation Score").ReportID.ToString())
						.Replace("{sevDate}", context.Reports.First(r => r.ReportName == "Observation Severity by Student and Date").ReportID.ToString())
						.Replace("{freqDate}", context.Reports.First(r => r.ReportName == "Observation Frequency by Student and Date").ReportID.ToString())
						.Replace("{sev}", context.Reports.First(r => r.ReportName == "Observations Grouped by Severity").ReportID.ToString())
		}
            };

            context.Dashboards.AddRange(dashboards);
            context.SaveChanges();
        }
    }
}
