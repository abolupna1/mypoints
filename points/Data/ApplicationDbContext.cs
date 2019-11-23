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

        public DbSet<AppUserDepartment> AppUserDepartments { get; set; }
        public DbSet<BusinessAndAchievement> BusinessAndAchievements { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Occasion> Occasions { get; set; }
        public DbSet<EvaluationCriteria> EvaluationCriterias { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<AppUserDepartment>()
              .HasKey(t => new { t.Id, t.UserId, t.DepartmentId });

            builder.Entity<AppUserDepartment>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserDepatrments)
                .HasForeignKey(pt => pt.UserId);

            builder.Entity<AppUserDepartment>()
                .HasOne(pt => pt.Department)
                .WithMany(p => p.UserDepatrments)
                .HasForeignKey(pt => pt.DepartmentId);

            builder.Entity<BusinessAndAchievement>()
            .HasKey(t => new { t.Id, t.EmployeeId, t.TimesOfEvaluationAndPerformanceId });

            builder.Entity<BusinessAndAchievement>()
                .HasOne(pt => pt.Employee)
                .WithMany(p => p.BusinessAndAchievements)
                .HasForeignKey(pt => pt.EmployeeId);

            builder.Entity<BusinessAndAchievement>()
                .HasOne(pt => pt.TimesOfEvaluationAndPerformance)
                .WithMany(p => p.BusinessAndAchievements)
                .HasForeignKey(pt => pt.TimesOfEvaluationAndPerformanceId);

            builder.Entity<Course>()
           .HasKey(t => new { t.Id, t.EmployeeId, t.TimesOfEvaluationAndPerformanceId });

            builder.Entity<Course>()
                .HasOne(pt => pt.Employee)
                .WithMany(p => p.Courses)
                .HasForeignKey(pt => pt.EmployeeId);

            builder.Entity<Course>()
                .HasOne(pt => pt.TimesOfEvaluationAndPerformance)
                .WithMany(p => p.Courses)
                .HasForeignKey(pt => pt.TimesOfEvaluationAndPerformanceId);


            builder.Entity<Occasion>()
           .HasKey(t => new { t.Id, t.EmployeeId, t.TimesOfEvaluationAndPerformanceId });

            builder.Entity<Occasion>()
                .HasOne(pt => pt.Employee)
                .WithMany(p => p.Occasions)
                .HasForeignKey(pt => pt.EmployeeId);

            builder.Entity<Occasion>()
                .HasOne(pt => pt.TimesOfEvaluationAndPerformance)
                .WithMany(p => p.Occasions)
                .HasForeignKey(pt => pt.TimesOfEvaluationAndPerformanceId);



            builder.Entity<Evaluation>()
           .HasKey(t => new { t.Id, t.EmployeeId, t.TimesOfEvaluationAndPerformanceId });

            builder.Entity<Evaluation>()
                .HasOne(pt => pt.Employee)
                .WithMany(p => p.Evaluations)
                .HasForeignKey(pt => pt.EmployeeId);

            builder.Entity<Evaluation>()
                .HasOne(pt => pt.TimesOfEvaluationAndPerformance)
                .WithMany(p => p.Evaluations)
                .HasForeignKey(pt => pt.TimesOfEvaluationAndPerformanceId);



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
