using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Branch
    {
        public int branchId { get; set; }
        public string branchName { get; set; }
        public ICollection<Employee> Employees { get; set; }

    }
}
