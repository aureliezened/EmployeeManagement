﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Response
{
    public class PaginatedWebUsersResponse
    {
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<AllWebUsers> WebUsers { get; set; }
    }
}
