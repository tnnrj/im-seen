using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Dashboard
    {
        public int DashboardID { get; set; }

        [ForeignKey("Users.UserID")]
        public int UserID { get; set; }

        public string DashboardText { get; set; }

    }
}
