using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class EmployeeStatus
    {
        public int statusId { get; set; }
        public string statusName { get; set; }
        public ICollection<Employee> Employees { get; set; }

    }
}
