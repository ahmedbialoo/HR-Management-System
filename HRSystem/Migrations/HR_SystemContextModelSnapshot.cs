﻿// <auto-generated />
using System;
using HRSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HRSystem.Migrations
{
    [DbContext(typeof(HR_SystemContext))]
    partial class HR_SystemContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("HRSystem.Models.AnnualVacation", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("AnnualVacations");
                });

            modelBuilder.Entity("HRSystem.Models.Attendance", b =>
                {
                    b.Property<int>("att_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("att_id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("TimeAtt")
                        .IsRequired()
                        .HasColumnType("time");

                    b.Property<TimeSpan?>("TimeLeave")
                        .IsRequired()
                        .HasColumnType("time");

                    b.Property<DateTime>("order")
                        .HasColumnType("datetime2");

                    b.HasKey("att_id");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("HRSystem.Models.AttendenceEmployee", b =>
                {
                    b.Property<int>("Att_id")
                        .HasColumnType("int");

                    b.Property<int>("Emp_Ssn")
                        .HasColumnType("int");

                    b.HasKey("Att_id", "Emp_Ssn");

                    b.HasIndex("Emp_Ssn");

                    b.ToTable("AttendenceEmployees");
                });

            modelBuilder.Entity("HRSystem.Models.Employee", b =>
                {
                    b.Property<int>("Emp_Ssn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Emp_Ssn"), 1L, 1);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeptName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Emp_Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Emp_Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Emp_FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Emp_Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Emp_NationalID")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<TimeSpan>("End_Time")
                        .HasColumnType("time");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Salary")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Start_Time")
                        .HasColumnType("time");

                    b.Property<string>("Vac1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vac2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("discount")
                        .HasColumnType("int");

                    b.Property<int>("overtime")
                        .HasColumnType("int");

                    b.HasKey("Emp_Ssn");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("HRSystem.Models.Group", b =>
                {
                    b.Property<int>("Group_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Group_Id"), 1L, 1);

                    b.Property<bool>("Attend_Add")
                        .HasColumnType("bit");

                    b.Property<bool>("Attend_Delete")
                        .HasColumnType("bit");

                    b.Property<bool>("Attend_Edit")
                        .HasColumnType("bit");

                    b.Property<bool>("Attend_show")
                        .HasColumnType("bit");

                    b.Property<bool>("Emp_Add")
                        .HasColumnType("bit");

                    b.Property<bool>("Emp_Delete")
                        .HasColumnType("bit");

                    b.Property<bool>("Emp_Edit")
                        .HasColumnType("bit");

                    b.Property<bool>("Emp_show")
                        .HasColumnType("bit");

                    b.Property<bool>("Group_Add")
                        .HasColumnType("bit");

                    b.Property<bool>("Group_Delete")
                        .HasColumnType("bit");

                    b.Property<bool>("Group_Edit")
                        .HasColumnType("bit");

                    b.Property<string>("Group_Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("Group_show")
                        .HasColumnType("bit");

                    b.Property<bool>("User_Add")
                        .HasColumnType("bit");

                    b.Property<bool>("User_Delete")
                        .HasColumnType("bit");

                    b.Property<bool>("User_Edit")
                        .HasColumnType("bit");

                    b.Property<bool>("User_show")
                        .HasColumnType("bit");

                    b.HasKey("Group_Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("HRSystem.Models.User", b =>
                {
                    b.Property<int>("User_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("User_Id"), 1L, 1);

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User_Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("User_Fname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("User_Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("Username", "User_Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HRSystem.Models.AttendenceEmployee", b =>
                {
                    b.HasOne("HRSystem.Models.Attendance", "Attendances")
                        .WithMany("AttendenceEmployees")
                        .HasForeignKey("Att_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HRSystem.Models.Employee", "Employees")
                        .WithMany("AttendenceEmployees")
                        .HasForeignKey("Emp_Ssn")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attendances");

                    b.Navigation("Employees");
                });

            modelBuilder.Entity("HRSystem.Models.User", b =>
                {
                    b.HasOne("HRSystem.Models.Group", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("HRSystem.Models.Attendance", b =>
                {
                    b.Navigation("AttendenceEmployees");
                });

            modelBuilder.Entity("HRSystem.Models.Employee", b =>
                {
                    b.Navigation("AttendenceEmployees");
                });

            modelBuilder.Entity("HRSystem.Models.Group", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
