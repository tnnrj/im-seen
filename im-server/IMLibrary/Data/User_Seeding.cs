using IMLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IMLibrary.Data
{
    public class User_Seeding
    {
        public static async Task SeedUsers(IM_API_Context context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            // Create roles if they don't exist
            try
            {
                if (roleManager.Roles.Count() == 0)
                {
                    var adminRole = new IdentityRole("Administrator");
                    roleManager.CreateAsync(adminRole).Wait();
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("WebAppUser", ""));
                    // by default admin has all actions on all controllers
                    #region Admin Controller Roles
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Dashboards.Create", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Dashboards.Read", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Dashboards.Update", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Dashboards.Delete", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Dashboards.Archive", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Delegations.Create", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Delegations.Read", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Delegations.Update", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Delegations.Delete", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Delegations.Archive", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Observations.Create", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Observations.Read", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Observations.Update", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Observations.Delete", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Observations.Archive", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Reports.Create", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Reports.Read", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Reports.Update", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Reports.Delete", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Reports.Archive", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("StudentGroups.Create", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("StudentGroups.Read", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("StudentGroups.Update", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("StudentGroups.Delete", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("StudentGroups.Archive", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Students.Create", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Students.Read", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Students.Update", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Students.Delete", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Students.Archive", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Supporters.Create", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Supporters.Read", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Supporters.Update", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Supporters.Delete", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Supporters.Archive", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Users.Create", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Users.Read", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Users.Update", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Users.Delete", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Users.Archive", ""));
                    #endregion
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Students.SeeAll", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Observations.SeeAll", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Tokens.Revoke", ""));


                    var primaryRole = new IdentityRole("PrimaryActor");
                    roleManager.CreateAsync(primaryRole).Wait();
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("WebAppUser", ""));
                    // by default, primary gets read on all controllers and CRUA on observations
                    #region Primary Controller Roles
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("Dashboards.Read", ""));
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("Dashboards.Update", ""));
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("Delegations.Read", ""));
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("Observations.Create", ""));
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("Observations.Read", ""));
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("Observations.Update", ""));
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("Observations.Archive", ""));
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("Reports.Read", ""));
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("StudentGroups.Read", ""));
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("Students.Read", ""));
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("Supporters.Read", ""));
                    _ = roleManager.AddClaimAsync(primaryRole, new Claim("Users.Read", ""));
                    #endregion
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Students.SeeAll", ""));
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("Observations.SeeAll", ""));

                    var supportingRole = new IdentityRole("SupportingActor");
                    roleManager.CreateAsync(supportingRole).Wait();
                    _ = roleManager.AddClaimAsync(adminRole, new Claim("WebAppUser", ""));
                    // supporting only has what is necessary to view dash and observations, and update observations
                    #region Supporting Controller Roles
                    _ = roleManager.AddClaimAsync(supportingRole, new Claim("Dashboards.Read", ""));
                    _ = roleManager.AddClaimAsync(supportingRole, new Claim("Dashboards.Update", ""));
                    _ = roleManager.AddClaimAsync(supportingRole, new Claim("Observations.Create", ""));
                    _ = roleManager.AddClaimAsync(supportingRole, new Claim("Observations.Read", ""));
                    _ = roleManager.AddClaimAsync(supportingRole, new Claim("Observations.Update", ""));
                    _ = roleManager.AddClaimAsync(supportingRole, new Claim("Reports.Read", ""));
                    _ = roleManager.AddClaimAsync(supportingRole, new Claim("Students.Read", ""));
                    #endregion

                    var observerRole = new IdentityRole("Observer");
                    roleManager.CreateAsync(observerRole).Wait();
                    // observer can only do one thing
                    _ = roleManager.AddClaimAsync(observerRole, new Claim("Observations.Create", ""));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error creating role.", e);
            }

            // Look for any existing Users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }


            var hasher = new PasswordHasher<ApplicationUser>();
            var user = new ApplicationUser
            {
                UserName = "dumbledore",
                Email = "dumbledore@hogwa.rts",
                JobTitle = "Headmaster",
                FirstName = "Albus",
                LastName = "Dumbledore",
                NormalizedEmail = "DUMBLEDORE@HOGWA.RTS",
                NormalizedUserName = "DUMBLEDORE",
                EmailConfirmed = true,
                Role = "Administrator"
            };
            user.PasswordHash = hasher.HashPassword(user, "test");
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, "Administrator");

            user = new ApplicationUser
            {
                UserName = "mcgonagall",
                Email = "mcgonagall@hogwa.rts",
                JobTitle = "Assistant Headmaster/Head of Gryffindor",
                FirstName = "Minerva",
                LastName = "McGonagall",
                NormalizedEmail = "MCGONAGALL@HOGWA.RTS",
                NormalizedUserName = "MCGONAGALL",
                Role = "PrimaryActor"
            };
            user.PasswordHash = hasher.HashPassword(user, "test");
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, "PrimaryActor");

            user = new ApplicationUser
            {
                UserName = "sprout",
                Email = "sprout@hogwa.rts",
                JobTitle = "Head of Hufflepuff",
                FirstName = "Pomona",
                LastName = "Sprout",
                NormalizedEmail = "SPROUT@HOGWA.RTS",
                NormalizedUserName = "SPROUT",
                Role = "PrimaryActor"
            };
            user.PasswordHash = hasher.HashPassword(user, "test");
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, "PrimaryActor");

            user = new ApplicationUser
            {
                UserName = "flitwick",
                Email = "Flitwick@hogwa.rts",
                JobTitle = "Head of Ravenclaw",
                FirstName = "Filius",
                LastName = "Flitwick",
                NormalizedEmail = "FLITWICK@HOGWA.RTS",
                NormalizedUserName = "FLITWICK",
                Role = "PrimaryActor"
            };
            user.PasswordHash = hasher.HashPassword(user, "test");
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, "PrimaryActor");

            user = new ApplicationUser
            {
                UserName = "snape",
                Email = "snape@hogwa.rts",
                JobTitle = "Head of Slytherin",
                FirstName = "Severus",
                LastName = "Snape",
                NormalizedEmail = "SNAPE@HOGWA.RTS",
                NormalizedUserName = "SNAPE",
                Role = "PrimaryActor"
            };
            user.PasswordHash = hasher.HashPassword(user, "test");
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, "PrimaryActor");

            user = new ApplicationUser
            {
                UserName = "filch",
                Email = "filch@hogwa.rts",
                JobTitle = "Caretaker",
                FirstName = "Argus",
                LastName = "Filch",
                NormalizedEmail = "FILCH@HOGWA.RTS",
                NormalizedUserName = "FILCH",
                Role = "SupportingActor"
            };
            user.PasswordHash = hasher.HashPassword(user, "test");
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, "SupportingActor");

            await context.SaveChangesAsync();
        }
    }
}
