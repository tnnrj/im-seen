using IMLibrary.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMLibrary.Data
{
    class Config_Seeding
    {
        public static void SeedConfigs(IM_API_Context context)
        {
            if (context.RuntimeConfigItems.Any())
            {
                return;   // DB has been seeded
            }

            var configs = new RuntimeConfigItem[]
            {
                new RuntimeConfigItem
                {
                    Key = "DaysToDecay",
                    Value = "14"
                },
                new RuntimeConfigItem
                {
                    Key = "RelevantDaysWindow",
                    Value = "4"
                },
                new RuntimeConfigItem
                {
                    Key = "AssignedStatusModifier",
                    Value = "-1"
                },
                new RuntimeConfigItem
                {
                    Key = "ResolvedStatusModifier",
                    Value = "ZERO"
                }
            };

            context.RuntimeConfigItems.AddRange(configs);
            context.SaveChanges();
        }
    }
}
