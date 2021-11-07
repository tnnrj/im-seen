using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Delegation
    {
        public int DelegationID { get; set; }

        [Required]
        public StudentGroup StudentGroup { get; set; }

        [Required]
        public Student Student { get; set; }
    }
}
