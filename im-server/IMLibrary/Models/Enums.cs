using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary.Models
{
    public enum BatchResult
    {
        None = 0,
        Success = 1,
        Partial = 2,
        NoData = 3,
        Failure = 4
    }
}
