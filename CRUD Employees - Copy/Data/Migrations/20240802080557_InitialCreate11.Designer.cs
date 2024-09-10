﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240802080557_InitialCreate11")]
    partial class InitialCreate11
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Common.DTOs.Response.MonthlyAttendancePercentageDTO", b =>
                {
                    b.Property<double>("averageAttendancePercentage")
                        .HasColumnType("double precision");

                    b.Property<string>("monthNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable((string)null);

                    b.ToView(null, (string)null);
                });

            modelBuilder.Entity("Common.Models.AttendanceStatistics", b =>
                {
                    b.Property<int>("statisticsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("statisticsId"));

                    b.Property<double>("attendancePercentage")
                        .HasColumnType("double precision");

                    b.Property<DateOnly>("date")
                        .HasColumnType("date");

                    b.Property<int?>("day")
                        .HasColumnType("integer");

                    b.Property<Guid>("employeeId")
                        .HasColumnType("uuid");

                    b.Property<int>("level")
                        .HasColumnType("integer");

                    b.Property<string>("month")
                        .HasColumnType("text");

                    b.Property<double>("workingHours")
                        .HasColumnType("double precision");

                    b.Property<int>("year")
                        .HasColumnType("integer");

                    b.HasKey("statisticsId");

                    b.HasIndex("employeeId");

                    b.ToTable("AttendanceStatistics", (string)null);
                });

            modelBuilder.Entity("Common.Models.AttendanceStatus", b =>
                {
                    b.Property<int>("statusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("statusId"));

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("statusId");

                    b.ToTable("AttendanceStatuses", (string)null);
                });

            modelBuilder.Entity("Common.Models.Branch", b =>
                {
                    b.Property<int>("branchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("branchId"));

                    b.Property<string>("branchName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("branchId");

                    b.ToTable("Branches", (string)null);
                });

            modelBuilder.Entity("Common.Models.DefaultWorkingHours", b =>
                {
                    b.Property<int>("defaultWorkingHoursId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("defaultWorkingHoursId"));

                    b.Property<Guid>("employeeId")
                        .HasColumnType("uuid");

                    b.Property<TimeOnly>("endTime")
                        .HasColumnType("time without time zone");

                    b.Property<TimeOnly>("startTime")
                        .HasColumnType("time without time zone");

                    b.HasKey("defaultWorkingHoursId");

                    b.HasIndex("employeeId")
                        .IsUnique();

                    b.ToTable("DefaultWorkingHours", (string)null);
                });

            modelBuilder.Entity("Common.Models.Department", b =>
                {
                    b.Property<int>("departmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("departmentId"));

                    b.Property<string>("departmentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("departmentId");

                    b.ToTable("Departments", (string)null);
                });

            modelBuilder.Entity("Common.Models.Employee", b =>
                {
                    b.Property<Guid>("employeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("birthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("branch")
                        .HasColumnType("integer");

                    b.Property<int>("department")
                        .HasColumnType("integer");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("employeeIdentifier")
                        .HasColumnType("integer");

                    b.Property<string>("fullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("jobTitle")
                        .HasColumnType("integer");

                    b.Property<string>("phoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("profilePictureUrl")
                        .HasColumnType("text");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.HasKey("employeeId");

                    b.HasIndex("branch");

                    b.HasIndex("department");

                    b.HasIndex("jobTitle");

                    b.HasIndex("status");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("Common.Models.EmployeeAttendance", b =>
                {
                    b.Property<int>("attendanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("attendanceId"));

                    b.Property<DateOnly>("attendanceDate")
                        .HasColumnType("date");

                    b.Property<TimeSpan>("checkIn")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("checkOut")
                        .HasColumnType("interval");

                    b.Property<Guid>("employeeId")
                        .HasColumnType("uuid");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.HasKey("attendanceId");

                    b.HasIndex("employeeId");

                    b.HasIndex("status");

                    b.ToTable("Attendances", (string)null);
                });

            modelBuilder.Entity("Common.Models.EmployeeStatus", b =>
                {
                    b.Property<int>("statusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("statusId"));

                    b.Property<string>("statusName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("statusId");

                    b.ToTable("EmployeeStatuses", (string)null);
                });

            modelBuilder.Entity("Common.Models.JobTitle", b =>
                {
                    b.Property<int>("jobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("jobId"));

                    b.Property<string>("jobName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("jobId");

                    b.ToTable("JobTitles", (string)null);
                });

            modelBuilder.Entity("Common.Models.WebRole", b =>
                {
                    b.Property<int>("webRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("webRoleId"));

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("webRoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("webRoleId");

                    b.ToTable("WebRoles", (string)null);
                });

            modelBuilder.Entity("Common.Models.WebUser", b =>
                {
                    b.Property<Guid>("webUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("fullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isPassChanged")
                        .HasColumnType("boolean");

                    b.Property<string>("msisdn")
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("webRole")
                        .HasColumnType("integer");

                    b.Property<int>("webUserIdentifier")
                        .HasColumnType("integer");

                    b.HasKey("webUserId");

                    b.HasIndex("webRole");

                    b.ToTable("WebUsers", (string)null);
                });

            modelBuilder.Entity("Common.Models.AttendanceStatistics", b =>
                {
                    b.HasOne("Common.Models.Employee", "employee")
                        .WithMany("attendanceStatistics")
                        .HasForeignKey("employeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("employee");
                });

            modelBuilder.Entity("Common.Models.DefaultWorkingHours", b =>
                {
                    b.HasOne("Common.Models.Employee", "employee")
                        .WithOne("defaultWorkingHours")
                        .HasForeignKey("Common.Models.DefaultWorkingHours", "employeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("employee");
                });

            modelBuilder.Entity("Common.Models.Employee", b =>
                {
                    b.HasOne("Common.Models.Branch", "Branch")
                        .WithMany("Employees")
                        .HasForeignKey("branch")
                        .IsRequired();

                    b.HasOne("Common.Models.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("department")
                        .IsRequired();

                    b.HasOne("Common.Models.JobTitle", "JobTitle")
                        .WithMany("Employees")
                        .HasForeignKey("jobTitle")
                        .IsRequired();

                    b.HasOne("Common.Models.EmployeeStatus", "Status")
                        .WithMany("Employees")
                        .HasForeignKey("status")
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Department");

                    b.Navigation("JobTitle");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Common.Models.EmployeeAttendance", b =>
                {
                    b.HasOne("Common.Models.Employee", "Employee")
                        .WithMany("Attendances")
                        .HasForeignKey("employeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Common.Models.AttendanceStatus", "AttendanceStatus")
                        .WithMany("Attendances")
                        .HasForeignKey("status")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AttendanceStatus");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Common.Models.WebUser", b =>
                {
                    b.HasOne("Common.Models.WebRole", "WebRole")
                        .WithMany("WebUsers")
                        .HasForeignKey("webRole")
                        .IsRequired();

                    b.Navigation("WebRole");
                });

            modelBuilder.Entity("Common.Models.AttendanceStatus", b =>
                {
                    b.Navigation("Attendances");
                });

            modelBuilder.Entity("Common.Models.Branch", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Common.Models.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Common.Models.Employee", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("attendanceStatistics");

                    b.Navigation("defaultWorkingHours")
                        .IsRequired();
                });

            modelBuilder.Entity("Common.Models.EmployeeStatus", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Common.Models.JobTitle", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Common.Models.WebRole", b =>
                {
                    b.Navigation("WebUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
