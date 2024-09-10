﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class WebUser
    {
        public Guid webUserId { get; set; }
        public int webUserIdentifier { get; set; }
        public DateOnly createdAt { get; set; }
        public string fullName { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string? msisdn { get; set; }
        public int webRole { get; set; }
        public bool isPassChanged { get; set; }
        public WebRole WebRole { get; set; }

    }
}