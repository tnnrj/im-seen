using IMWebAPI.Models;
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

            var users = new User[]
            {
                new User {userName="Dumbledore"},

                new User {userName="McGonagall"},

                new User {userName="Filch"}
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
