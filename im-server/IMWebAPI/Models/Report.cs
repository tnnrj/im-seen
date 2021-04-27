using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Report
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int reportID { get; set; }

        [ForeignKey("Users.userID")]
        public int reporter { get; set; }

        [ForeignKey("Students.studentID")]
        public int studentID { get; set; }

        [ForeignKey("Students.studentName")]
        public string studentName { get; set; }

        public string description { get; set; }

        public int severity { get; set; }

        public DateTime reportDate { get; set; }
    }
}
