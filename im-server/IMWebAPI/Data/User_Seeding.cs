using IMWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Data
{
    public class User_Seeding
    {
        public static async void SeedUsers(IM_API_Context context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            // Create roles if they don't exist
            try
            {
                if (roleManager.Roles.Count() == 0)
                {
                    roleManager.CreateAsync(new IdentityRole("Administrator")).Wait();
                    roleManager.CreateAsync(new IdentityRole("PrimaryActor")).Wait();
                    roleManager.CreateAsync(new IdentityRole("SupportingActor")).Wait();
                    roleManager.CreateAsync(new IdentityRole("Observer")).Wait();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error creating role.");
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
            };
            user.PasswordHash = hasher.HashPassword(user, "test");
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, "Administrator");

            user = new ApplicationUser
            {
                UserName = "mcgonagall",
                Email = "mcgonagall@hogwa.rts",
                JobTitle = "Assistant Headmaster",
                FirstName = "Minerva",
                LastName = "McGonagall",
                NormalizedEmail = "MCGONAGALL@HOGWA.RTS",
                NormalizedUserName = "MCGONAGALL"
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
                NormalizedUserName = "FILCH"
            };
            user.PasswordHash = hasher.HashPassword(user, "test");
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, "Observer");

            await context.SaveChangesAsync();
        }
    }
}
