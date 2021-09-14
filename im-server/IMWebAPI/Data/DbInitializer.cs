using System;
using System.Collections.Generic;

namespace IMWebAPI.Data
{
    public class DbInitializer
    {
        public static void Initialize(IM_API_Context context)
        {
#if DEBUG
            context.Database.EnsureDeleted();
#endif
            context.Database.EnsureCreated();

            User_Seeding.SeedUsers(context);
            Student_Seeding.SeedStudents(context);
            Report_Seeding.SeedReports(context);

        }
    }
}

