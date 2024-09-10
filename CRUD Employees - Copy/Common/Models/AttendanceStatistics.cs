using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class AttendanceStatistics
    {
        public int statisticsId { get; set; }
        public DateOnly date { get; set; }
        public int? day { get; set; }
        public string? month { get; set; }
        public int year { get; set; }
        public int level { get; set; }
        public double workingHours { get; set; }
        public double attendancePercentage { get; set; }
        public Guid employeeId { get; set; }

        public Employee employee { get; set; }
    }
}
