using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Common.DTOs.Request;
using Common.DTOs.Response;
using Common.Models;
using Microsoft.AspNetCore.Http;

namespace Business
{
    public interface IEmployeeService
    {
        Task<bool> AddEmployeeAsync(EmployeeWithAttendanceDTO employeeWithAttendance);
        Task<IEnumerable<StatusListResponse>> GetAllStatusesAsync();
        Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync();
        Task<IEnumerable<JobTitleResponse>> GetAllJobsAsync();
        Task<IEnumerable<BranchResponse>> GetAllBranchesAsync();
        Task<bool?> CheckEmployeeExistsAsync(Guid employeeId);
        Task<bool> EditEmployee(Guid employeeId, Dictionary<string, object> updates);
        Task DeleteEmployeeAsync(Guid employeeId);
        Task<PaginatedEmployeesResponse> GetAllEmployeesAsync(int page, int pageSize, string? generalSearch);
        Task<Guid?> getEmployeeId(string email);
        Task<EmployeeNameID?> GetEmployee(Guid employeeId);
        Task<bool> AddAttendancesAsync(Guid employeeId, List<AddAttendancesDTO> attendances);
        Task<EmployeeResponse?> GetEmployeeDetails(Guid employeeId);
        Task<bool> AddEmployeeWorkingDefaultHours(DefaultWorkingHoursDTO? data);
        Task<bool> EditWorkingHours(Guid employeeId, Dictionary<string, object> updates);
    }
}

