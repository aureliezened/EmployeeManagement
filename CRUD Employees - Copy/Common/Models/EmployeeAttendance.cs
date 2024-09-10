namespace Common.Models
{
    public class EmployeeAttendance

    {
        public int attendanceId { get; set; }

        public DateOnly attendanceDate { get; set; }

        private TimeSpan _checkIn;
        public TimeSpan checkIn
        {
            get => _checkIn;
            set => _checkIn = value;
        }

        private TimeSpan _checkOut;
        public TimeSpan checkOut
        {
            get => _checkOut;
            set => _checkOut = value;
        }

        public Guid employeeId { get; set; } //foreign key referencing the Employee
        public int status { get; set; }
        public Employee Employee { get; set; } // this is the employee associated with this attendance record
        public AttendanceStatus AttendanceStatus { get; set; }
        
    }
}
