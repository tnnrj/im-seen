using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Report
    {
        public int reportID { get; set; }

        public int userID { get; set; }

        public int studentID { get; set; }

        public string studentName { get; set; }

        public string description { get; set; }

        public int severity { get; set; }

        public DateTime reportDate { get; set; }
    }
}
