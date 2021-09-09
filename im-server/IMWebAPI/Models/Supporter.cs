using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models
{
    public class Supporter
    {
        public int SupporterID { get; set; }

        public int GroupID { get; set; }

        public int UserID { get; set; }

        public Group Group { get; set; }

        public User User { get; set; }
    }
}
