using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMLibrary.Models.Authorization
{
    public class RuntimeConfigItem
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
