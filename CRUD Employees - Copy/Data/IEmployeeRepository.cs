using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Common.DTOs.Request;
using Common.DTOs.Response;
using System.Text.Json;


namespace Data
{
    public interface IEmployeeRepository
    {
        Task AddEmployee(EmployeeWithAttendanceDTO? employeeWithAttendance);
        Task<IEnumerable<StatusListResponse>> GetAllStatusesAsync();
        Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync();
        Task<IEnumerable<JobTitleResponse>> GetAllJobsAsync();
        Task<IEnumerable<BranchResponse>> GetAllBranchesAsync();
        Task EditEmployee(Guid employeeId, Dictionary<string, object> updates);
        Task<bool?> CheckEmployeeExistsAsync(Guid employeeId);
        Task DeleteEmployeeAsync(Guid employeeId);
        Task<PaginatedEmployeesResponse> GetAllEmployeesAsync(int page, int pageSize, string? generalSearch);
        Task<Guid?> getEmployeeId(string email);
        Task<EmployeeNameID?> GetEmployee(Guid employeeId);
        Task<bool> AddAttendancesAsync(Guid employeeId, List<AddAttendancesDTO> attendances);
        Task<EmployeeResponse?> GetEmployeeDetails(Guid employeeId);
        Task<bool> AddEmployeeWorkingDefaultHours(DefaultWorkingHoursDTO? data);
        Task EditWorkingHours(Guid employeeId, Dictionary<string, object> updates);
    }
}

