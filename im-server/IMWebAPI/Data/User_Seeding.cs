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
        public static void SeedUsers(IM_API_Context context)
        {
            // Look for any existing Users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var userStore = new UserStore<ApplicationUser>(context);

            var hasher = new PasswordHasher<ApplicationUser>();
            var user = new ApplicationUser
            {
                UserName = "dumbledore",
                Email = "dumbledore@hogwa.rts",
                JobTitle = "Headmaster",
                FirstName = "Albus",
                LastName = "Dumbledore"
            };
            user.PasswordHash = hasher.HashPassword(user, "test");
            userStore.CreateAsync(user);
            user = new ApplicationUser
            {
                UserName = "mcgonagall",
                Email = "mcgonagall@hogwa.rts",
                JobTitle = "Assistant Headmaster",
                FirstName = "Minerva",
                LastName = "McGonagall"
            };
            user.PasswordHash = hasher.HashPassword(user, "test");
            userStore.CreateAsync(user);
            user = new ApplicationUser
            {
                UserName = "filch",
                Email = "filch@hogwa.rts",
                JobTitle = "Caretaker",
                FirstName = "Argus",
                LastName = "Filch"
            };
            user.PasswordHash = hasher.HashPassword(user, "test");
            userStore.CreateAsync(user);
        }
    }
}
