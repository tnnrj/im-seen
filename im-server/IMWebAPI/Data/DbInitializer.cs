using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Data
{
    public class DbInitializer
    {
        public static void Initialize(IM_API_Context context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            User_Seeding.SeedUsers(context);
            Student_Seeding.SeedStudents(context);
            Report_Seeding.SeedReports(context);

        }
    }
}

