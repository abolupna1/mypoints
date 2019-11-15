using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using points.Models;

namespace points.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int,
      IdentityUserClaim<int>,
      AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimesOfEvaluationAndPerformance> TimesOfEvaluationAndPerformances { get; set; }

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
          

         

            builder.Entity<AppUserRole>(

                userRole =>
                {
                    userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                    userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                    userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles).HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                }

                );





        }



      
    }
}
