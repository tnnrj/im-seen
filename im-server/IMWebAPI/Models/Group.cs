using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Group
    {
        public int GroupID { get; set; }

        [Required]
        [DefaultValue("")]
        [MaxLength(50)]
        public string GroupName { get; set; }

        [Required]
        [ForeignKey("Users.UserName")]
        public string PrimaryUserName { get; set; }
    }
}
