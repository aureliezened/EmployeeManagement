using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class AttendanceStatus
    {
        public int statusId {  get; set; }
        public string StatusName { get; set; }
        public ICollection<EmployeeAttendance> Attendances { get; set; }
    }
}
