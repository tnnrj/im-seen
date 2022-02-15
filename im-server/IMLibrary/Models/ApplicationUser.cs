using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string JobTitle { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public string Role { get; set; }

        // these are here to consolidate references to roles
        public static readonly string Administrator = "Administrator";
        public static readonly string PrimaryActor = "PrimaryActor";
        public static readonly string SupportingActor = "SupportingActor";
        public static readonly string Observer = "Observer";
    }
}
