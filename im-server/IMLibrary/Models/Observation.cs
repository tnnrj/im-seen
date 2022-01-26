/**
 * This file defines the model class for an Observation.
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Models
{
    public enum ObservationStatus
    {
        New = 1,
        Viewed = 2,
        Assigned = 3,
        ActionTaken = 4,
        Closed = 5
    }

    public class Observation
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int ObservationID { get; set; }

        [ForeignKey("Users.UserName")]
        public string UserName { get; set; }

        [ForeignKey("Students.StudentID")]
        public int? StudentID { get; set; }

        [Required]
        [DefaultValue("")]
        [MaxLength(50)]
        public string StudentFirstName { get; set; }

        [Required]
        [DefaultValue("")]
        [MaxLength(50)]
        public string StudentLastName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Severity { get; set; }

        public DateTime ObservationDate { get; set; }

        public string Status { get; set; }

        public string Action { get; set; }

        public string Event { get; set; }
    }
}
