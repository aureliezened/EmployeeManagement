    using Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Common.DTOs.Response
    {
        public class AllWebUsers
        {
            public Guid webUserId { get; set; }
            public DateTime createdAt { get; set; }
            public string fullName { get; set; }
            public string userName { get; set; }
            public string email { get; set; }
            public string? msisdn { get; set; }
            public string webRole { get; set; }

        }
    }
