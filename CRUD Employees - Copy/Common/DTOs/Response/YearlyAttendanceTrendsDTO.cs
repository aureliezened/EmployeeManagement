using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Response
{
    public class YearlyAttendanceTrendsDTO
    {
        public int Year { get; set; }
        public List<MonthlyAttendancePercentageDTO> MonthlyTrends { get; set; }
    }
}
