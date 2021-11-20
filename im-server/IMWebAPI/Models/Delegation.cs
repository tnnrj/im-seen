﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Delegation
    {
        public int DelegationID { get; set; }

        [Required]
        [ForeignKey("StudentGroups.StudentGroupID")]
        public int StudentGroupID { get; set; }
        [Required]
        public StudentGroup StudentGroup { get; set; }

        [Required]
        [ForeignKey("Students.StudentID")]
        public int StudentID { get; set; }
        [Required]
        public Student Student { get; set; }
    }
}
