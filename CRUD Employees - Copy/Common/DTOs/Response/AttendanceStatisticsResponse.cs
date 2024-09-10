using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Response
{
    public class AttendanceStatisticsResponse
    {
        public int level { get; set; }
        public Guid? employeeId { get; set; }
        public double workingHours { get; set; }
        public double attendancePercentage { get; set; }
    }
}
