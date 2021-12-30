using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HRSystem.Models
{
    public partial class HR_SystemContext : DbContext
    {

        public HR_SystemContext(DbContextOptions<HR_SystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<AttendenceEmployee> AttendenceEmployees { get; set; }
        public virtual DbSet<AnnualVacation> AnnualVacations { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendenceEmployee>().HasKey(n => new { n.Att_id, n.Emp_Ssn });
            modelBuilder.Entity<AttendenceEmployee>()
                .HasOne(n => n.Attendances)
                .WithMany(n => n.AttendenceEmployees)
                .HasForeignKey(n => n.Att_id);
            modelBuilder.Entity<AttendenceEmployee>()
                .HasOne(n => n.Employees)
                .WithMany(n => n.AttendenceEmployees)
                .HasForeignKey(n => n.Emp_Ssn);

       modelBuilder.Entity<User>().HasIndex(u => new { u.Username , u.User_Email  } )
        .IsUnique();

        }




    }
}
