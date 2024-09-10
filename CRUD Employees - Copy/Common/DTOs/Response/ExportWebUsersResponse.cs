using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Response
{
    public class ExportWebUsersResponse
    {
        public DateTime CreatedAt { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Msisdn { get; set; }
        public string WebRole { get; set; }
    }
}
