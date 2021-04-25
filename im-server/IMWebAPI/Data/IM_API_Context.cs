using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IMWebAPI.Models;

namespace IMWebAPI.Data
{
    public class IM_API_Context : DbContext
    {
        public IM_API_Context (DbContextOptions<IM_API_Context> options)
            : base(options)
        {
        }

        public DbSet<IMWebAPI.Models.Student> Students { get; set; }

        public DbSet<IMWebAPI.Models.User> Users { get; set; }

        public DbSet<IMWebAPI.Models.Report> Reports { get; set; }
    }
}
