using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Response
{
    public class MonthlyAttendancePercentageDTO
    {
        public string monthNumber { get; set; } 
        public double averageAttendancePercentage { get; set; }
    }
}
