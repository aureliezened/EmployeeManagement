using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Response
{
    public class EmployeeResponse
    {
        public Guid employeeId { get; set; }
        public DateTime joinedAt { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string? phoneNumber { get; set; }
        public DateTime birthDate { get; set; }
        public string jobTitle { get; set; }
        public string department { get; set; }
        public string status { get; set; }
        public string branch { get; set; }

    }
}