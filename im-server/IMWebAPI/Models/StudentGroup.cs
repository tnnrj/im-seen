/**
 * This file defines the model class for a StudentGroup.
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class StudentGroup
    {
        public int StudentGroupID { get; set; }

        [Required]
        [DefaultValue("")]
        [MaxLength(50)]
        public string StudentGroupName { get; set; }

        [Required]
        [ForeignKey("Users.UserName")]
        public string PrimaryUserName { get; set; }
    }
}
