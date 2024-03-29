﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Models
{
    public class Delegation
    {
        public int DelegationID { get; set; }

        [Required]
        [ForeignKey("StudentGroups.StudentGroupID")]
        public int StudentGroupID { get; set; }
        public StudentGroup StudentGroup { get; set; }

        [Required]
        [ForeignKey("Students.StudentID")]
        public int StudentID { get; set; }
        public Student Student { get; set; }
    }
}
