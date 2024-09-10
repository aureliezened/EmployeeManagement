using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Request
{
    public class DefaultWorkingHoursDTO
    {
        public TimeOnly startTime { get; set; }
        public TimeOnly endTime { get; set; }
        public Guid employeeId { get; set; }
    }
}
