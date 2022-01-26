/**
 * This file defines a config pattern for various settings necessary for a JWT bearer token implementation.
 * Values for these settings can be adjusted in the appsettings.json file.
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Configuration
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int AccessLifeInSecs { get; set; }
        public int RefreshLifeInSecs { get; set; }
        public int ObserverRefreshLifeInSecs { get; set; }
    }
}
