/**
 * This file defines the model class for a Supporter.
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Supporter
    {
        public int SupporterID { get; set; }

        [Required]
        [ForeignKey("StudentGroups.StudentGroupID")]
        public int StudentGroupID { get; set; }
        public StudentGroup StudentGroup { get; set; }

        [Required]
        [ForeignKey("Users.UserName")]
        public string UserName { get; set; }
    }
}
