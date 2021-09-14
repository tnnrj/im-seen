using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Observation
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int ObservationID { get; set; }

        [ForeignKey("Users.UserName")]
        public string UserName { get; set; }

        [ForeignKey("Students.StudentID")]
        public int? StudentID { get; set; }

        public string StudentName { get; set; }

        public string Description { get; set; }

        public int Severity { get; set; }

        public DateTime ObservationDate { get; set; }

        public string Action { get; set; }

        public string Event { get; set; }
    }
}
