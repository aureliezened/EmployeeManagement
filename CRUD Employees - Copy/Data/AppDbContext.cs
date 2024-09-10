using Common.DTOs.Response;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<AttendanceStatus> AttendanceStatuses { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }  //table Employees from the model Employee
        public DbSet<EmployeeAttendance> Attendances { get; set; } //table Attendances from the model EmployeeAttendance
        public DbSet<EmployeeStatus> EmployeeStatuses { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<WebRole> WebRoles { get; set; }
        public DbSet<WebUser> WebUsers { get; set; }
        public DbSet<AttendanceStatistics> AttendanceStatistics { get; set; }
        public DbSet<MonthlyAttendancePercentageDTO> MonthlyAttendancePercentageDTO { get; set; }
        public DbSet<DefaultWorkingHours> defaultWorkingHours { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<EmployeeAttendance>().ToTable("Attendances");
            modelBuilder.Entity<Branch>().ToTable("Branches");
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<EmployeeStatus>().ToTable("EmployeeStatuses");
            modelBuilder.Entity<WebRole>().ToTable("WebRoles");
            modelBuilder.Entity<JobTitle>().ToTable("JobTitles");
            modelBuilder.Entity<WebUser>().ToTable("WebUsers");
            modelBuilder.Entity<AttendanceStatus>().ToTable("AttendanceStatuses");
            modelBuilder.Entity<DefaultWorkingHours>().ToTable("DefaultWorkingHours");

            modelBuilder.Entity<AttendanceStatistics>().ToTable("AttendanceStatistics");

            modelBuilder.Entity<Employee>()
                .HasKey(e => e.employeeId);

            modelBuilder.Entity<EmployeeAttendance>()
                .HasKey(a => a.attendanceId);

            modelBuilder.Entity<Branch>()
                .HasKey(b => b.branchId);

            modelBuilder.Entity<Department>()
                .HasKey(d => d.departmentId);

            modelBuilder.Entity<EmployeeStatus>()
                .HasKey(s => s.statusId);

            modelBuilder.Entity<WebRole>()
                .HasKey(w => w.webRoleId);

            modelBuilder.Entity<JobTitle>()
               .HasKey(j => j.jobId);

            modelBuilder.Entity<WebUser>()
            .HasKey(w => w.webUserId);

            modelBuilder.Entity<AttendanceStatus>()
            .HasKey(at => at.statusId); 

            modelBuilder.Entity<AttendanceStatistics>()
            .HasKey(att => att.statisticsId);


            modelBuilder.Entity<DefaultWorkingHours>()
            .HasKey(d => d.defaultWorkingHoursId);

            //to configure the one to many relationship
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Attendances)
                .WithOne(a => a.Employee)
                .HasForeignKey(a => a.employeeId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Branch>()
                .HasMany(b => b.Employees)
                .WithOne(e => e.Branch)
                .HasForeignKey(e => e.branch)
                .OnDelete(DeleteBehavior.ClientSetNull); 

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.department)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<EmployeeStatus>()
                .HasMany(s => s.Employees)
                .WithOne(e => e.Status)
                .HasForeignKey(e => e.status)
                .OnDelete(DeleteBehavior.ClientSetNull);
            
            modelBuilder.Entity<JobTitle>()
                .HasMany(j => j.Employees)
                .WithOne(e => e.JobTitle)
                .HasForeignKey(e => e.jobTitle)
                .OnDelete(DeleteBehavior.ClientSetNull); 

            modelBuilder.Entity<WebRole>()
                .HasMany(w => w.WebUsers)
                .WithOne(e => e.WebRole)
                .HasForeignKey(e => e.webRole)
                .OnDelete(DeleteBehavior.ClientSetNull); 

            modelBuilder.Entity<AttendanceStatus>()
                .HasMany(at => at.Attendances)
                .WithOne(e => e.AttendanceStatus)
                .HasForeignKey(e => e.status)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
            .HasOne(e => e.defaultWorkingHours)
            .WithOne(d => d.employee)
            .HasForeignKey<DefaultWorkingHours>(d => d.employeeId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AttendanceStatistics>()
            .HasOne(att => att.employee)
            .WithMany(e => e.attendanceStatistics)
            .HasForeignKey(att => att.employeeId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MonthlyAttendancePercentageDTO>(entity =>
            {
                entity.HasNoKey(); // Specify that this entity does not have a primary key
                entity.ToView(null); // We are mapping to a function, not a table/view
            });


            base.OnModelCreating(modelBuilder);

        }

    }
}