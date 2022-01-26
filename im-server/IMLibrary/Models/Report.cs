﻿/**
 * This file defines the model class for a Report.
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Models
{
    public class Report
    {
        public int ReportID { get; set; }

        public string ReportName { get; set; }

        public string Query { get; set; }

        public string Axis1Name { get; set; }
        public string Axis2Name { get; set; }
        public string AvailableChartTypes { get; set; }
    }
}
