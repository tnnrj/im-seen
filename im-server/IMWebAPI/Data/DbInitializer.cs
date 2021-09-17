using System;
using System.Collections.Generic;

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
            Observation_Seeding.SeedObservations(context);
            Report_Seeding.SeedReports(context);
        }
    }
}

