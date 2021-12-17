/**
 * This file defines the model class for an ApplicationUser, which extends IdentityUser.
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string JobTitle { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public string Role { get; set; }
    }
}
