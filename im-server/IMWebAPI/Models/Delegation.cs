using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Delegation
    {
        public int DelegationID { get; set; }

        public int GroupID { get; set; }

        public int StudentID { get; set; }

        public Group Group { get; set; }

        public Student Student { get; set; }
    }
}
