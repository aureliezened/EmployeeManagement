using Common.DTOs.Request;
using Common.DTOs.Response;
using Common.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using Common.Models;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(AppDbContext dbContext, ILogger<EmployeeRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public async Task AddEmployee(EmployeeWithAttendanceDTO? employeeWithAttendance)
        {
            _logger.LogInformation($"{nameof(AddEmployee)}: EmployeeRepository.");
            try
            {
                var employeeDataJson = System.Text.Json.JsonSerializer.Serialize(employeeWithAttendance);
                var empUuid = Guid.NewGuid();

                var attendanceDataJson = employeeWithAttendance.Attendances != null ?
                                         System.Text.Json.JsonSerializer.Serialize(employeeWithAttendance.Attendances) :
                                         null;

                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "CALL add_employee(@emp_uuid, @employee_data, @attendance_data)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@employee_data", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = employeeDataJson });
                command.Parameters.Add(new NpgsqlParameter("@attendance_data", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = attendanceDataJson ?? (object)DBNull.Value });
                command.Parameters.Add(new NpgsqlParameter("@emp_uuid", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = empUuid });
              //  command.Parameters.Add(new NpgsqlParameter("@profilePictureBytes", NpgsqlTypes.NpgsqlDbType.Bytea) { Value = profilePictureBytes ?? (object)DBNull.Value });


                await _dbContext.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddEmployee)}: EmployeeRepository.");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task<bool> AddAttendancesAsync(Guid employeeId, List<AddAttendancesDTO> attendances)
        {
            _logger.LogInformation($"{nameof(AddAttendancesAsync)}: EmployeeRepository.");
            try
            {
                var employee = await _dbContext.Employees.FindAsync(employeeId);
                if (employee == null)
                {
                    _logger.LogWarning($"Employee with ID {employeeId} not found.");
                    return false;
                }

                foreach (var attendanceDto in attendances)
                {
                    if (!attendanceDto.checkIn.HasValue || !attendanceDto.checkOut.HasValue)
                    {
                        _logger.LogWarning($"Attendance DTO has null values for checkIn or checkOut.");
                        continue; // Skip this attendanceDto since it has null values
                    }

                    var attendance = new EmployeeAttendance
                    {
                        employeeId = employeeId,
                        attendanceDate = attendanceDto.attendanceDate,
                        checkIn = attendanceDto.checkIn.Value,
                        checkOut = attendanceDto.checkOut.Value,
                        status = attendanceDto.status
                    };

                    _dbContext.Attendances.Add(attendance);
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddAttendancesAsync)}: EmployeeRepository.");
                return false;
            }
        }

        public async Task<IEnumerable<StatusListResponse>> GetAllStatusesAsync()
        {
            _logger.LogInformation($"{nameof(GetAllStatusesAsync)}: EmployeeRepository.");

            try
            {
                var statuses = await _dbContext.EmployeeStatuses
                    .FromSqlRaw("SELECT * FROM get_all_statuses()")
                    .Select(u => new StatusListResponse
                    {
                        statusName = u.statusName
                    })
                    .ToListAsync();
                return statuses;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllStatusesAsync)}: EmployeeRepository.");
                throw;
            }
        }

        public async Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync()
        {
            _logger.LogInformation($"{nameof(GetAllDepartmentsAsync)}: EmployeeRepository.");

            try
            {
                var departments = await _dbContext.Departments
                    .FromSqlRaw("SELECT * FROM get_all_departments()")
                    .Select(u => new DepartmentResponse
                    {
                        departmentName = u.departmentName
                    })
                    .ToListAsync();
                return departments;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllDepartmentsAsync)}: EmployeeRepository.");
                throw;
            }
        }

        public async Task<IEnumerable<JobTitleResponse>> GetAllJobsAsync()
        {
            _logger.LogInformation($"{nameof(GetAllJobsAsync)}: EmployeeRepository.");

            try
            {
                var jobs = await _dbContext.JobTitles
                    .FromSqlRaw("SELECT * FROM get_all_jobs()")
                    .Select(u => new JobTitleResponse
                    {
                        jobTitle = u.jobName
                    })
                    .ToListAsync();
                return jobs;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllJobsAsync)}: EmployeeRepository.");
                throw;
            }
        }

        public async Task<IEnumerable<BranchResponse>> GetAllBranchesAsync()
        {
            _logger.LogInformation($"{nameof(GetAllBranchesAsync)}: EmployeeRepository.");

            try
            {
                var branches = await _dbContext.Branches
                    .FromSqlRaw("SELECT * FROM get_all_branches()")
                    .Select(u => new BranchResponse
                    {
                        branchName = u.branchName
                    })
                    .ToListAsync();
                return branches;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllBranchesAsync)}: EmployeeRepository.");
                throw;
            }
        }

        public async Task<bool?> CheckEmployeeExistsAsync(Guid employeeId)
        {
            _logger.LogInformation($"{nameof(CheckEmployeeExistsAsync)}: EmployeeRepository.");

            try
            {
                await _dbContext.Database.OpenConnectionAsync();

                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT check_employee_exists(@employeeId)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@employeeId", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = employeeId });

                using (command)
                {
                    var obj = await command.ExecuteScalarAsync();
                    if (obj != null && bool.TryParse(obj.ToString(), out bool ans))
                    {
                        return ans;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(CheckEmployeeExistsAsync)}: EmployeeRepository.");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task EditEmployee(Guid employeeId, Dictionary<string, object> updates)
        {
            _logger.LogInformation($"{nameof(EditEmployee)}: EmployeeRepository.");
            try
            {
                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "Select edit_employee_details(@employeeId, @updates)";
                command.CommandType = System.Data.CommandType.Text;

                var updatesJson = System.Text.Json.JsonSerializer.Serialize(updates);
                command.Parameters.Add(new NpgsqlParameter("@employeeId", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = employeeId });
                command.Parameters.Add(new NpgsqlParameter("@updates", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = updatesJson });

                await _dbContext.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EditEmployee)}: EmployeeRepository.");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task DeleteEmployeeAsync(Guid employeeId)
        {
            _logger.LogInformation($"{nameof(DeleteEmployeeAsync)}: EmployeeRepository.");
            try
            {
                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT delete_employee(@employeeId)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@employeeId", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = employeeId });

                await _dbContext.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(DeleteEmployeeAsync)}: EmployeeRepository.");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task<PaginatedEmployeesResponse> GetAllEmployeesAsync(int page, int pageSize, string? generalSearch)
        {
            _logger.LogInformation($"{nameof(GetAllEmployeesAsync)}: EmployeeRepository.");

            var offset = (page - 1) * pageSize;

            try
            {
                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    var query = "SELECT * FROM get_all_employees(@limit_param, @offset_param, @p_general_search);";
                    var parameters = new { limit_param = pageSize, offset_param = offset, p_general_search = generalSearch };

                    var employees = await connection.QueryAsync<EmployeeResponse>(query, parameters);

                    var totalRecords = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM \"Employees\"");
                    var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                    var response = new PaginatedEmployeesResponse
                    {
                        TotalRecords = totalRecords,
                        PageSize = pageSize,
                        CurrentPage = page,
                        TotalPages = totalPages,
                        Employees = employees

                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllEmployeesAsync)}: EmployeeRepository.");
                throw;
            }
        }


        public async Task<Guid?> getEmployeeId(string email)
        {
            _logger.LogInformation($"{nameof(getEmployeeId)}: EmployeeRepository.");
            try
            {
                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT getEmployeeId(@p_email)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@p_email", NpgsqlTypes.NpgsqlDbType.Text) { Value = email });

                await _dbContext.Database.OpenConnectionAsync();

                var result = await command.ExecuteScalarAsync();
                return result == DBNull.Value ? (Guid?)null : (Guid)result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(getEmployeeId)}: EmployeeRepository.");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task<EmployeeNameID?> GetEmployee(Guid employeeId)
        {
            _logger.LogInformation($"{nameof(GetEmployee)}: EmployeeRepository.");
            try
            {
                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT * FROM getEmployee(@p_empid)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@p_empid", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = employeeId });

                await _dbContext.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new EmployeeNameID
                        {
                            employeeId = reader.GetGuid(reader.GetOrdinal("employeeId")),
                            employeeIdentifier = reader.GetInt32(reader.GetOrdinal("employeeIdentifier")),
                            fullName = reader.GetString(reader.GetOrdinal("fullName")),
                            profilePictureUrl = reader.IsDBNull(reader.GetOrdinal("profilePictureUrl")) ? null : reader.GetString(reader.GetOrdinal("profilePictureUrl"))
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetEmployee)}: EmployeeRepository.");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task<EmployeeResponse?> GetEmployeeDetails(Guid employeeId)
        {
            _logger.LogInformation($"{nameof(GetEmployeeDetails)}: EmployeeRepository.");

            try
            {
                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT * FROM get_employee_details(@empId)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@empId", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = employeeId });

                await _dbContext.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var employeeResponse = new EmployeeResponse
                        {
                            employeeId = reader.GetGuid(reader.GetOrdinal("employeeId")),
                            fullName = reader.GetString(reader.GetOrdinal("fullName")),
                            email = reader.GetString(reader.GetOrdinal("email")),
                            phoneNumber = reader.GetString(reader.GetOrdinal("phoneNumber")),
                            jobTitle = reader.GetString(reader.GetOrdinal("jobTitle")),
                            department = reader.GetString(reader.GetOrdinal("department")),
                            status = reader.GetString(reader.GetOrdinal("status")),
                            branch = reader.GetString(reader.GetOrdinal("branch")),
                            joinedAt = reader.GetDateTime(reader.GetOrdinal("joinedAt")),
                            birthDate = reader.GetDateTime(reader.GetOrdinal("birthDate"))
                        };


                        return employeeResponse;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetEmployeeDetails)}: EmployeeRepository.");
                throw;
            }
        }

        public async Task<bool> AddEmployeeWorkingDefaultHours(DefaultWorkingHoursDTO? data)
        {
            _logger.LogInformation($"{nameof(AddEmployeeWorkingDefaultHours)}: EmployeeRepository.");
            try
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "Select add_employee_default_working_hours(@p_startTime, @p_endTime, @p_employeeId)";
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.Add(new NpgsqlParameter("@p_employeeId", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = data.employeeId });
                    command.Parameters.Add(new NpgsqlParameter("@p_startTime", NpgsqlTypes.NpgsqlDbType.Time) { Value = data.startTime });
                    command.Parameters.Add(new NpgsqlParameter("@p_endTime", NpgsqlTypes.NpgsqlDbType.Time) { Value = data.endTime });

                    await _dbContext.Database.OpenConnectionAsync();
                    await command.ExecuteNonQueryAsync();

                    return true; 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddEmployeeWorkingDefaultHours)}: EmployeeRepository.");
                return false; 
            }
        }

        public async Task EditWorkingHours(Guid employeeId, Dictionary<string, object> updates)
        {
            _logger.LogInformation($"{nameof(EditWorkingHours)}: EmployeeRepository.");
            try
            {
                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "Select update_default_working_hours(@employeeId, @updates)";
                command.CommandType = System.Data.CommandType.Text;

                var updatesJson = System.Text.Json.JsonSerializer.Serialize(updates);
                command.Parameters.Add(new NpgsqlParameter("@employeeId", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = employeeId });
                command.Parameters.Add(new NpgsqlParameter("@updates", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = updatesJson });

                await _dbContext.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EditWorkingHours)}: EmployeeRepository.");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }
    }
}
