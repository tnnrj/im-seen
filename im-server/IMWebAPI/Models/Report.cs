using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Report
    {
        public int ReportID { get; set; }

        public string ReportName { get; set; }

        public string Query { get; set; }
    }
}
