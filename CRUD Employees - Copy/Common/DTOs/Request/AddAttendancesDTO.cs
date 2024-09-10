using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Request
{
    public class AddAttendancesDTO
    {
        public DateOnly attendanceDate { get; set; }
        public TimeSpan? checkIn { get; set; }
        public TimeSpan? checkOut { get; set; }
        public int status { get; set; }
    }
}
