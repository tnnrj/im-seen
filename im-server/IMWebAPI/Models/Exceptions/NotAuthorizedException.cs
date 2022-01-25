using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Models.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException() : base("User is not authorized to perform this action") { }
        public NotAuthorizedException(string message) : base(message) { }
    }
}
