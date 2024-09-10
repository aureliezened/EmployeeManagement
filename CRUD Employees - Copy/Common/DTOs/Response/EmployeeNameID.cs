using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Response
{
    public class EmployeeNameID
    {
        public Guid employeeId {  get; set; }
        public int employeeIdentifier { get; set; }
        public string fullName { get; set; }
        public string profilePictureUrl { get; set; }
    }
}
