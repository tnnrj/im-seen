using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Supporter
    {
        public int SupporterID { get; set; }

        [ForeignKey("Groups.GroupID")]
        public int GroupID { get; set; }

        [ForeignKey("Users.UserID")]
        public int UserID { get; set; }
    }
}
