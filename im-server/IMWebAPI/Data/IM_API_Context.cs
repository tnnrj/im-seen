using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IMWebAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace IMWebAPI.Data
{
    public class IM_API_Context : IdentityDbContext<ApplicationUser>
    {
        public IM_API_Context (DbContextOptions<IM_API_Context> options)
            : base(options)
        {
        }

        public DbSet<IMWebAPI.Models.Student> Students { get; set; }
        public DbSet<IMWebAPI.Models.Observation> Observations { get; set; }
        public DbSet<IMWebAPI.Models.Group> Groups { get; set; }
        public DbSet<IMWebAPI.Models.Delegation> Delegations { get; set; }
        public DbSet<IMWebAPI.Models.Supporter> Supporters { get; set; }
        public DbSet<IMWebAPI.Models.Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Observation>().ToTable("Observations");
            modelBuilder.Entity<Group>().ToTable("Groups");
            modelBuilder.Entity<Delegation>().ToTable("Delegations");
            modelBuilder.Entity<Supporter>().ToTable("Supporters");
            modelBuilder.Entity<Report>().ToTable("Reports");

            base.OnModelCreating(modelBuilder);
        }
    }
}
