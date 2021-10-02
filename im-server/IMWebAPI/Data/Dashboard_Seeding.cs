using IMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Data
{
    public class Dashboard_Seeding
    {
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
                    DashboardText = "[{\"layout\":\"ThreeTwoTB\",\"elements\":[{\"reportID\":3,\"chartType\":\"Bar\",\"width\":50,\"height\":50},{\"reportID\":3,\"chartType\":\"BubbleCloud\",\"width\":50,\"height\":50},{\"reportID\":2,\"chartType\":\"Line\",\"width\":100,\"height\":50}]},{\"layout\":\"TwoTB\",\"elements\":[{\"reportID\":3,\"chartType\":\"BubbleCloud\",\"width\":100,\"height\":50},{\"reportID\":3,\"chartType\":\"BubbleCloud\",\"width\":100,\"height\":50}]},{\"layout\":\"ThreeTwoLR\",\"elements\":[{\"reportID\":3,\"chartType\":\"BubbleCloud\",\"width\":50,\"height\":50},{\"reportID\":3,\"chartType\":\"BubbleCloud\",\"width\":50,\"height\":50},{\"reportID\":3,\"chartType\":\"BubbleCloud\",\"width\":50,\"height\":100}]}]"
                }
            };

            context.Dashboards.AddRange(dashboards);
            context.SaveChanges();
        }
    }
}
