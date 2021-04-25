using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Report
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int reportID { get; set; }

        public User reporter { get; set; }

        public Student student { get; set; }

        public string description { get; set; }

        public int severity { get; set; }

        public DateTime reportDate { get; set; }
    }
}
