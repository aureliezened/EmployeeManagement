using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.Request
{
    public class EmployeeWithAttendanceDTO
    {
        // Employee properties
        public string fullName { get; set; }
        public string email { get; set; }
        public DateTime birthDate { get; set; }
        public string phoneNumber { get; set; }
        public string jobTitle { get; set; }
        public string department { get; set; }
        public string status { get; set; }
        public string branch { get; set; }

      //  [FromForm]
       // public IFormFile? profilePicture { get; set; }
        //public DateTime joinedAt { get; set; } // adjust the joinedAt in pgAdmin to take the current date

        // EmployeeAttendance properties
        public List<AttendanceDTO> Attendances { get; set; } = new List<AttendanceDTO>();

        public class AttendanceDTO
        {
            public DateTime attendanceDate { get; set; }
            public TimeSpan? checkIn { get; set; }
            public TimeSpan? checkOut { get; set; }
        }
    }

}