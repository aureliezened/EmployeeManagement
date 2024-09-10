using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class Employee
    {
        public Guid employeeId { get; set; }
        public int employeeIdentifier { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public DateTime birthDate { get; set; }
        public string? phoneNumber { get; set; }
        public int jobTitle { get; set; }
        public int department { get; set; }
        public int status { get; set; }
        public int branch { get; set; }
        public string? profilePictureUrl { get; set; }
        public DateTime joinedAt { get; set; }
        public ICollection<EmployeeAttendance> Attendances { get; set; }
        public ICollection<AttendanceStatistics> attendanceStatistics { get; set; }
        public Department Department { get; set; }
        public EmployeeStatus Status { get; set; }
        public Branch Branch { get; set; }
        public JobTitle JobTitle { get; set; }
        public DefaultWorkingHours defaultWorkingHours { get; set; }

    }
}