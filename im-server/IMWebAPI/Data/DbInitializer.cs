/**
 * This file initializes the database using various seeding files.
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

using IMWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMWebAPI.Data
{
    public class DbInitializer
    {
        public static void Initialize(IM_API_Context context, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
        {
            context.Database.EnsureDeleted(); // Comment this line out to enable persistent DB
            context.Database.EnsureCreated();

            Task t = User_Seeding.SeedUsers(context, um, rm);
            t.Wait();
            Student_Seeding.SeedStudents(context);
            Observation_Seeding.SeedObservations(context);
            Report_Seeding.SeedReports(context);
            Dashboard_Seeding.SeedDashboards(context);

            Group_Seeding.SeedGroups(context);
            Group_Seeding.SeedDelegations(context);
            Group_Seeding.SeedSupporters(context);
        }
    }
}

