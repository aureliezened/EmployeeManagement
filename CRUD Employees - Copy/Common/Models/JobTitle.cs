using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class JobTitle
    {
        public int jobId { get; set; }
        public string jobName { get; set; }
        public ICollection<Employee> Employees { get; set; }

    }
}
