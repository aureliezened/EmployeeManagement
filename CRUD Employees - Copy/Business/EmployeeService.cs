using System;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Common.DTOs.Request;
using Common.DTOs.Response;
using Common.Models;
using Data;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Common.Helpers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Business
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;


        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }
        public async Task<bool> AddEmployeeAsync(EmployeeWithAttendanceDTO employeeWithAttendance)
        {
            try
            {
                if (employeeWithAttendance == null ||
                string.IsNullOrWhiteSpace(employeeWithAttendance.fullName) ||
                string.IsNullOrWhiteSpace(employeeWithAttendance.email) ||
                string.IsNullOrWhiteSpace(employeeWithAttendance.phoneNumber) ||
                string.IsNullOrWhiteSpace(employeeWithAttendance.jobTitle) ||
                string.IsNullOrWhiteSpace(employeeWithAttendance.department) ||
                string.IsNullOrWhiteSpace(employeeWithAttendance.status) ||
                string.IsNullOrWhiteSpace(employeeWithAttendance.branch) ||
                employeeWithAttendance.birthDate == default)
                {
                    return false;
                }

                await _employeeRepository.AddEmployee(employeeWithAttendance);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddEmployeeAsync)}: EmployeeService.");
                throw;
            }
        }

        public async Task<IEnumerable<StatusListResponse>> GetAllStatusesAsync()
        {
            _logger.LogInformation($"{nameof(GetAllStatusesAsync)}: EmployeeService.");
            try
            {
                return await _employeeRepository.GetAllStatusesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllStatusesAsync)}: EmployeeService.");
                throw;

            }
        }

        public async Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync()
        {
            _logger.LogInformation($"{nameof(GetAllDepartmentsAsync)}: EmployeeService.");
            try
            {
                return await _employeeRepository.GetAllDepartmentsAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllDepartmentsAsync)}: EmployeeService.");
                throw;

            }
        }

        public async Task<IEnumerable<JobTitleResponse>> GetAllJobsAsync()
        {
            _logger.LogInformation($"{nameof(GetAllJobsAsync)}: EmployeeService.");
            try
            {
                return await _employeeRepository.GetAllJobsAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllJobsAsync)}: EmployeeService.");
                throw;

            }
        }

        public async Task<IEnumerable<BranchResponse>> GetAllBranchesAsync()
        {
            _logger.LogInformation($"{nameof(GetAllBranchesAsync)}: EmployeeService.");
            try
            {
                return await _employeeRepository.GetAllBranchesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllBranchesAsync)}: EmployeeService.");
                throw;

            }
        }

        public async Task<bool?> CheckEmployeeExistsAsync(Guid employeeId)
        {
            _logger.LogInformation($"{nameof(CheckEmployeeExistsAsync)}: EmployeeService.");
            try
            {

                return await _employeeRepository.CheckEmployeeExistsAsync(employeeId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(CheckEmployeeExistsAsync)}: EmployeeService.");
                throw;
            }
        }

        public async Task<bool> EditEmployee(Guid employeeId, Dictionary<string, object> updates)
        {
            _logger.LogInformation($"{nameof(EditEmployee)}: EmployeeService.");
            try
            {
                if (updates == null || updates.Count == 0)
                {
                    return false; // Invalid input
                }

                await _employeeRepository.EditEmployee(employeeId, updates);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EditEmployee)}: EmployeeService.");
                throw;
            }
        }

        public async Task DeleteEmployeeAsync(Guid employeeId)
        {
            _logger.LogInformation($"{nameof(DeleteEmployeeAsync)}: EmployeeService.");

            try
            {
                await _employeeRepository.DeleteEmployeeAsync(employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(DeleteEmployeeAsync)}: EmployeeService.");
                throw;
            }
        }

        public async Task<PaginatedEmployeesResponse> GetAllEmployeesAsync(int page, int pageSize, string? generalSearch)
        {
            _logger.LogInformation($"{nameof(GetAllEmployeesAsync)}: EmployeeService.");
            try
            {
                return await _employeeRepository.GetAllEmployeesAsync(page, pageSize, generalSearch);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllEmployeesAsync)}: EmployeeService.");
                throw;

            }
        }

        public async Task<Guid?> getEmployeeId(string email)
        {
            _logger.LogInformation($"{nameof(getEmployeeId)}: EmployeeService.");
            try
            {
                return await _employeeRepository.getEmployeeId(email);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(getEmployeeId)}: EmployeeService.");
                throw;

            }
        }

        public async Task<EmployeeNameID?> GetEmployee(Guid employeeId)
        {
            _logger.LogInformation($"{nameof(GetEmployee)}: EmployeeService.");
            try
            {
                return await _employeeRepository.GetEmployee(employeeId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetEmployee)}: EmployeeService.");
                throw;

            }

        }

        public async Task<bool> AddAttendancesAsync(Guid employeeId, List<AddAttendancesDTO> attendances)
        {
            _logger.LogInformation($"{nameof(AddAttendancesAsync)}: EmployeeService.");
            try
            {
                return await _employeeRepository.AddAttendancesAsync(employeeId, attendances);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddAttendancesAsync)}: EmployeeService.");
                throw;

            }
        }

        public async Task<EmployeeResponse?> GetEmployeeDetails(Guid employeeId)
        {
            _logger.LogInformation($"{nameof(GetEmployeeDetails)}: EmployeeService.");
            try
            {
                return await _employeeRepository.GetEmployeeDetails(employeeId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetEmployeeDetails)}: EmployeeService.");
                throw;

            }

        }

        public async Task<bool> AddEmployeeWorkingDefaultHours(DefaultWorkingHoursDTO? data)
        {
            _logger.LogInformation($"{nameof(AddEmployeeWorkingDefaultHours)}: EmployeeService.");

            try
            {
                return await _employeeRepository.AddEmployeeWorkingDefaultHours(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddEmployeeWorkingDefaultHours)}: EmployeeService.");
                throw;
            }
        }

        public async Task<bool> EditWorkingHours(Guid employeeId, Dictionary<string, object> updates)
        {
            _logger.LogInformation($"{nameof(EditWorkingHours)}: EmployeeService.");
            try
            {
                if (updates == null || updates.Count == 0)
                {
                    return false; 
                }

                await _employeeRepository.EditWorkingHours(employeeId, updates);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EditWorkingHours)}: EmployeeService.");
                throw;
            }
        }
    }
}
    