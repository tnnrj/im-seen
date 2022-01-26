/**
 * This file defines the model class for a Dashboard.
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Models
{
    public class Dashboard
    {
        public int DashboardID { get; set; }

        [ForeignKey("Users.UserName")]
        public string UserName { get; set; }

        public string DashboardText { get; set; }

    }
}
