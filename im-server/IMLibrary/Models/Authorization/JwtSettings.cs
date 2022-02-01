using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Models
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
