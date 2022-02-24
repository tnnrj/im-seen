using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IMLibrary.Models;
using Microsoft.AspNetCore.Identity;
using IMLibrary.Models.Authorization;

namespace IMLibrary.Data
{
    public class IM_API_Context : IdentityDbContext<ApplicationUser>
    {
        public IM_API_Context (DbContextOptions<IM_API_Context> options)
            : base(options)
        {
        }

        public DbSet<IMLibrary.Models.Student> Students { get; set; }
        public DbSet<IMLibrary.Models.Observation> Observations { get; set; }
        public DbSet<IMLibrary.Models.StudentGroup> StudentGroups { get; set; }
        public DbSet<IMLibrary.Models.Delegation> Delegations { get; set; }
        public DbSet<IMLibrary.Models.Supporter> Supporters { get; set; }
        public DbSet<IMLibrary.Models.Report> Reports { get; set; }
        public DbSet<IMLibrary.Models.Dashboard> Dashboards { get; set; }
        public DbSet<RuntimeConfigItem> RuntimeConfigItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Observation>().ToTable("Observations");
            modelBuilder.Entity<StudentGroup>().ToTable("StudentGroups");
            modelBuilder.Entity<Delegation>().ToTable("Delegations");
            modelBuilder.Entity<Supporter>().ToTable("Supporters");
            modelBuilder.Entity<Report>().ToTable("Reports");
            modelBuilder.Entity<Dashboard>().ToTable("Dashboards");
            modelBuilder.Entity<RuntimeConfigItem>().ToTable("RuntimeConfigItems");

            // fields should default empty not null
            modelBuilder.Entity<Observation>(entity => entity.Property(m => m.StudentFirstName).HasDefaultValue(""));
            modelBuilder.Entity<Observation>(entity => entity.Property(m => m.StudentLastName).HasDefaultValue(""));
            modelBuilder.Entity<Observation>(entity => entity.Property(m => m.WeightedScore).HasDefaultValue(0));

            // for Identity-MySQL compatibility - see https://stackoverflow.com/questions/48678495/net-core-2-0-with-mysql-specified-key-was-too-long-max-key-length-is-3072-byt
            modelBuilder.Entity<ApplicationUser>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            modelBuilder.Entity<ApplicationUser>(entity => entity.Property(m => m.NormalizedEmail).HasMaxLength(85));
            modelBuilder.Entity<ApplicationUser>(entity => entity.Property(m => m.NormalizedUserName).HasMaxLength(85));
            modelBuilder.Entity<ApplicationUser>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            modelBuilder.Entity<IdentityRole>(entity => entity.Property(m => m.NormalizedName).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            modelBuilder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));

            // fields should be unique
            modelBuilder.Entity<Student>(entity => entity.HasIndex(e => e.ExternalID).IsUnique());

            base.OnModelCreating(modelBuilder);
        }
    }
}
