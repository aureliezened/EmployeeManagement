using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class DefaultWorkingHours
    {
        public int defaultWorkingHoursId {  get; set; }
        public TimeOnly startTime {  get; set; }
        public TimeOnly endTime { get; set; }
        public Guid employeeId { get; set; }

        public Employee employee { get; set; }
    }
}
