using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Business;
using Common.DTOs.Request;
using Common.DTOs.Response;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Common.Models;
using Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text.Json;



namespace EmployeeManagement.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IAttendanceStatisticsService _attendanceStatisticsService;

        public EmployeeController(IConfiguration configuration, ILogger<EmployeeController> logger, IAttendanceStatisticsService attendanceStatisticsService, IEmployeeService employeeService)
        {
            _configuration = configuration;
            _employeeService = employeeService;
            _attendanceStatisticsService = attendanceStatisticsService;
            _logger = logger;
        }

        [HttpPost("Add-Employee")]
        public async Task<ApiResponseType<Guid?>?> AddEmployee(EmployeeWithAttendanceDTO employeeWithAttendance)
        {
            _logger.LogInformation($"{nameof(AddEmployee)}");
            try
            {
                if (employeeWithAttendance == null)
                {
                    _logger.LogError("Invalid employee data provided");
                    var errorResponse = StatusCodeHelper.GetStatusResponse(2, (Guid?)null);
                    return errorResponse;
                }

                var result = await _employeeService.AddEmployeeAsync(employeeWithAttendance);
                if (!result)
                {
                    return StatusCodeHelper.GetStatusResponse(400, (Guid?)null);
                }
                var employeeId = await _employeeService.getEmployeeId(employeeWithAttendance.email);
                return StatusCodeHelper.GetStatusResponse(200, employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddEmployee)}: EmployeeController.");
                return StatusCodeHelper.GetStatusResponse(1, (Guid?)null);
            }
        }

        [HttpPost("Add-Attendance/{employeeId}")]
        public async Task<ApiResponse> AddAttendance(Guid employeeId, [FromBody] List<AddAttendancesDTO> attendances)
        {
            _logger.LogInformation($"{nameof(AddAttendance)}");
            try
            {
                bool? employeeExists = await _employeeService.CheckEmployeeExistsAsync(employeeId);
                if ((bool)!employeeExists)
                {
                    var notFoundResponse = StatusCodeHelper.GetStatusResponseWithoutType(8);
                    return notFoundResponse;
                }

                if (attendances == null)
                {
                    _logger.LogError("Invalid data provided");
                    var errorResponse = StatusCodeHelper.GetStatusResponseWithoutType(2);
                    return errorResponse;
                }

                var result = await _employeeService.AddAttendancesAsync(employeeId, attendances);
                if (!result)
                {
                    return StatusCodeHelper.GetStatusResponseWithoutType(400);
                }

                var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
                await _attendanceStatisticsService.UpdateStatisticsAsync(employeeId, currentDate);

                return StatusCodeHelper.GetStatusResponseWithoutType(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddAttendance)}: EmployeeController.");
                return StatusCodeHelper.GetStatusResponseWithoutType(1);
            }
        }

        [HttpGet("All-Statuses")]
        public async Task<ApiResponseType<IEnumerable<StatusListResponse?>?>> GetStatuses()
        {
            _logger.LogInformation($"{nameof(GetStatuses)}: EmployeeController.");
            try
            {
                var statuses = await _employeeService.GetAllStatusesAsync();
                var response = StatusCodeHelper.GetStatusResponseIEnumerable(200, statuses);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetStatuses)}: EmployeeController.");
                var errorResponse = StatusCodeHelper.GetStatusResponseIEnumerable(1, (IEnumerable<StatusListResponse?>?)null);
                return errorResponse;
            }
        }

        [HttpGet("All-Departments")]
        public async Task<ApiResponseType<IEnumerable<DepartmentResponse?>?>> GetDepartments()
        {
            _logger.LogInformation($"{nameof(GetDepartments)}: EmployeeController.");
            try
            {
                var departments = await _employeeService.GetAllDepartmentsAsync();
                var response = StatusCodeHelper.GetStatusResponseIEnumerable(200, departments);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetDepartments)}: EmployeeController.");
                var errorResponse = StatusCodeHelper.GetStatusResponseIEnumerable(1, (IEnumerable<DepartmentResponse?>?)null);
                return errorResponse;
            }
        }

        [HttpGet("All-JobTitles")]
        public async Task<ApiResponseType<IEnumerable<JobTitleResponse?>?>> GetJobs()
        {
            _logger.LogInformation($"{nameof(GetJobs)}: EmployeeController.");
            try
            {
                var jobs = await _employeeService.GetAllJobsAsync();
                var response = StatusCodeHelper.GetStatusResponseIEnumerable(200, jobs);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetJobs)}: EmployeeController.");
                var errorResponse = StatusCodeHelper.GetStatusResponseIEnumerable(1, (IEnumerable<JobTitleResponse?>?)null);
                return errorResponse;
            }
        }

        [HttpGet("All-Branches")]
        public async Task<ApiResponseType<IEnumerable<BranchResponse?>?>> GetBranches()
        {
            _logger.LogInformation($"{nameof(GetBranches)}: EmployeeController.");
            try
            {
                var branches = await _employeeService.GetAllBranchesAsync();
                var response = StatusCodeHelper.GetStatusResponseIEnumerable(200, branches);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetBranches)}: EmployeeController.");
                var errorResponse = StatusCodeHelper.GetStatusResponseIEnumerable(1, (IEnumerable<BranchResponse?>?)null);
                return errorResponse;
            }
        }

        [HttpPost("Edit-Employee-Details")]
        public async Task<ApiResponse> EditEmployee(Guid employeeId, Dictionary<string, object> updates)
        {
            _logger.LogInformation($"{nameof(EditEmployee)}: EmployeeController");
            try
            {
                bool? userExists = await _employeeService.CheckEmployeeExistsAsync(employeeId);
                if ((bool)!userExists)
                {
                    var notFoundResponse = StatusCodeHelper.GetStatusResponseWithoutType(8);
                    return notFoundResponse;
                }

                var result = await _employeeService.EditEmployee(employeeId, updates);
                if (!result)
                {
                    return StatusCodeHelper.GetStatusResponseWithoutType(400);
                }

                else return StatusCodeHelper.GetStatusResponseWithoutType(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EditEmployee)}: EmployeeController.");
                return StatusCodeHelper.GetStatusResponseWithoutType(1);
            }
        }

        [HttpDelete("Delete-Employee")]
        public async Task<ApiResponse> DeleteEmployee(Guid employeeId)
        {
            _logger.LogInformation($"{nameof(DeleteEmployee)}: EmployeeController");

            try
            {
                await _employeeService.DeleteEmployeeAsync(employeeId);
                return StatusCodeHelper.GetStatusResponseWithoutType(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(DeleteEmployee)}: EmployeeController.");

                return StatusCodeHelper.GetStatusResponseWithoutType(1);
            }
        }

        [HttpGet("View-All-Employees")]
        public async Task<ApiResponseType<PaginatedEmployeesResponse?>> GetAllEmployees(int page = 1, int pageSize = 10, string? generalSearch = null)
        {
            _logger.LogInformation($"{nameof(GetAllEmployees)} : EmployeeController.");

            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync(page, pageSize, generalSearch);

                if (employees == null)
                {
                    _logger.LogError("No employees found.");
                    return StatusCodeHelper.GetStatusResponse(11, (PaginatedEmployeesResponse?)null);
                }

                return StatusCodeHelper.GetStatusResponse(200, employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllEmployees)}: EmployeeController.");
                return StatusCodeHelper.GetStatusResponse(1, (PaginatedEmployeesResponse?)null);
            }
        }

        [HttpGet("Get-Employee-details")]
        public async Task<ApiResponseType<EmployeeResponse?>> GetEmployeeDetails(Guid EmployeeID)
        {
            _logger.LogInformation($"{nameof(GetEmployeeDetails)} : EmployeeController.");

            try
            {
                var employee = await _employeeService.GetEmployeeDetails(EmployeeID);
                return StatusCodeHelper.GetStatusResponse(200, employee);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetEmployeeDetails)}: EmployeeController.");
                return StatusCodeHelper.GetStatusResponse(1, (EmployeeResponse?)null);

            }

        }

        [HttpPost("Add-Employee-Default-Working-Hours")]
        public async Task<ApiResponse> AddEmployeeWorkingDefaultHours([FromBody] DefaultWorkingHoursDTO data)
        {
            _logger.LogInformation($"{nameof(AddEmployeeWorkingDefaultHours)} : EmployeeController.");

            try
            {
                if (data == null)
                {
                    _logger.LogError("Invalid data provided");
                    var errorResponse = StatusCodeHelper.GetStatusResponseWithoutType(12);
                    return errorResponse;
                }

                bool? employeeExists = await _employeeService.CheckEmployeeExistsAsync(data.employeeId);
                if ((bool)!employeeExists)
                {
                    var notFoundResponse = StatusCodeHelper.GetStatusResponseWithoutType(11);
                    return notFoundResponse;
                }

                var result = await _employeeService.AddEmployeeWorkingDefaultHours(data);
                if (!result)
                {
                    return StatusCodeHelper.GetStatusResponseWithoutType(400);
                }

                else return StatusCodeHelper.GetStatusResponseWithoutType(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddEmployeeWorkingDefaultHours)}: EmployeeController.");
                throw;
            }
        }

        [HttpPost("Edit-Employee-Working-Hours")]
        public async Task<ApiResponse> EditWorkingHours(Guid employeeId, Dictionary<string, object> updates)
        {
            _logger.LogInformation($"{nameof(EditWorkingHours)}: EmployeeController");
            try
            {
                bool? employeeExists = await _employeeService.CheckEmployeeExistsAsync(employeeId);
                if ((bool)!employeeExists)
                {
                    var notFoundResponse = StatusCodeHelper.GetStatusResponseWithoutType(8);
                    return notFoundResponse;
                }

                var result = await _employeeService.EditWorkingHours(employeeId, updates);
                if (!result)
                {
                    return StatusCodeHelper.GetStatusResponseWithoutType(400);
                }

                else return StatusCodeHelper.GetStatusResponseWithoutType(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EditWorkingHours)}: EmployeeController.");
                return StatusCodeHelper.GetStatusResponseWithoutType(1);
            }
        }
    }
}