using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Models
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
