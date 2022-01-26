/**
 * This file contains methods for seeding the Dashboards table of the IM_API_Context
 * Written by Steven Carpadakis & Tanner Jorgensen, U of U School of Computing, Senior Capstone 2021
 **/

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
                    DashboardText = "[{\"layout\":\"ThreeTwoTB\",\"elements\":[{\"reportID\":4,\"chartType\":\"Bar\",\"width\":50,\"height\":50},{\"reportID\":4,\"chartType\":\"BubbleCloud\",\"width\":50,\"height\":50},{\"reportID\":2,\"chartType\":\"Line\",\"width\":100,\"height\":50}]},{\"layout\":\"FourOneTB\",\"elements\":[{\"reportID\":3,\"chartType\":\"Line\",\"width\":100,\"height\":50},{\"reportID\":1,\"chartType\":\"Pie\",\"width\":33.33,\"height\":50},{\"reportID\":4,\"chartType\":\"BubbleCloud\",\"width\":33.33,\"height\":50},{\"reportID\":4,\"chartType\":\"Pie\",\"width\":33.34,\"height\":50}]},{\"layout\":\"FourTB\",\"elements\":[{\"reportID\":2,\"chartType\":\"Line\",\"width\":75.14,\"height\":52.19},{\"reportID\":0,\"chartType\":\"None\",\"width\":24.86,\"height\":52.19},{\"reportID\":0,\"chartType\":\"None\",\"width\":25.14,\"height\":47.81},{\"reportID\":3,\"chartType\":\"Line\",\"width\":74.86,\"height\":47.81}]}]"
                }
            };

            context.Dashboards.AddRange(dashboards);
            context.SaveChanges();
        }
    }
}
