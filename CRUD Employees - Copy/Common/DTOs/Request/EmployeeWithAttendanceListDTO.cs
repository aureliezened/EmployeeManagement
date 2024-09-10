//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Security.Cryptography.X509Certificates;
//using Common.DTOs.Response;

//namespace Common.DTOs.Request
//{
//    public class EmployeeWithAttendanceListDTO
//    {
//        // Employee properties
//        public int employeeIdentifier { get; set; }
//        public string fullName { get; set; }
//        public string email { get; set; }
//        public DateTime birthDate { get; set; }
//        public string? phoneNumber { get; set; }
//        public int jobTitle { get; set; }
//        public int department { get; set; }
//        public int status { get; set; }
//        public int branch { get; set; }
//        public byte[]? profilePicture { get; set; }

//        private DateTime joinedAt { get; set; }


//        // EmployeeAttendance properties
//        public List<EmployeeAttendanceDTO>? Attendances_list { get; set; } = new List<EmployeeAttendanceDTO>();

//        public string Attendances { get; set; }


//        public void deserializeAttendees()
//        {
//            if (!string.IsNullOrEmpty(Attendances))
//            {
//                Attendances_list = System.Text.Json.JsonSerializer.Deserialize<List<EmployeeAttendanceDTO>>(Attendances);

//            }
//        }

//        public EmployeeWithAttendanceListResponse GetResponse()
//        {
//            EmployeeWithAttendanceListResponse response = new EmployeeWithAttendanceListResponse();
//            response.employee_identifier = employeeIdentifier;
//            response.full_name = fullName;
//            response.email = email;
//            response.home_address = home_address;
//            response.phone_number = phone_number;
//            response.nssf = nssf;
//            response.position_name = position_name;
//            response.department_name = department_name;
//            response.status_name = status_name;
//            response.work_type_name = work_type_name;
//            response.salary = salary;
//            response.birth_date = birth_date;
//            response.joined_at = joined_at;
//            response.Attendances_list = Attendances_list;

//            return response;

//        }

//    }
//    public class EmployeeAttendanceDTO
//    {
//        public DateTime attendance_date { get; set; }
//        public TimeSpan? check_in { get; set; }
//        public TimeSpan? check_out { get; set; }
//    }


//}
