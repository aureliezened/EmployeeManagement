using Common.DTOs.Request;
using Common.DTOs.Response;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AttendanceStatisticsRepository : IAttendanceStatisticsRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AttendanceStatisticsRepository> _logger;

        public AttendanceStatisticsRepository(AppDbContext context, ILogger<AttendanceStatisticsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddStatisticsAsync(AttendanceStatistics statistics)
        {
            _logger.LogInformation($"{nameof(AddStatisticsAsync)}: AttendanceStatisticsRepository.");
            try
            {
                await _context.AttendanceStatistics.AddAsync(statistics);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Statistics saved to database successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddStatisticsAsync)}: AttendanceStatisticsRepository.");
                throw;
            }
        }

        public async Task UpdateStatisticsAsync(AttendanceStatistics statistics)
        {
            _logger.LogInformation($"{nameof(UpdateStatisticsAsync)}: AttendanceStatisticsRepository.");
            try
            {
                _context.AttendanceStatistics.Update(statistics);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Statistics updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UpdateStatisticsAsync)}: AttendanceStatisticsRepository.");
                throw;
            }
        }

        public async Task<AttendanceStatistics?> GetStatisticsByLevelAndDateAsync(int level, DateOnly date, Guid employeeID)
        {
            _logger.LogInformation($"{nameof(GetStatisticsByLevelAndDateAsync)}: AttendanceStatisticsRepository.");

            AttendanceStatistics? attendance = null;

            try
            {
                await _context.Database.OpenConnectionAsync();

                var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT * FROM get_statistics_by_level_and_date(@level, @date, @employeeID)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@level", NpgsqlTypes.NpgsqlDbType.Integer) { Value = level });
                command.Parameters.Add(new NpgsqlParameter("@date", NpgsqlTypes.NpgsqlDbType.Date) { Value = date });
                command.Parameters.Add(new NpgsqlParameter("@employeeID", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = employeeID });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        attendance = new AttendanceStatistics
                        {
                            statisticsId = reader.GetInt32(reader.GetOrdinal("statisticsId")),
                            date = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("date")),
                            day = reader.IsDBNull(reader.GetOrdinal("day")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("day")),
                            month = reader.IsDBNull(reader.GetOrdinal("month")) ? null : reader.GetString(reader.GetOrdinal("month")),
                            year = reader.GetInt32(reader.GetOrdinal("year")),
                            level = reader.GetInt32(reader.GetOrdinal("level")),
                            workingHours = reader.GetDouble(reader.GetOrdinal("workingHours")),
                            attendancePercentage = reader.GetDouble(reader.GetOrdinal("attendancePercentage")),
                            employeeId = reader.GetGuid(reader.GetOrdinal("employeeId"))
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetStatisticsByLevelAndDateAsync)}: AttendanceStatisticsRepository.");
                throw;
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return attendance;
        }

        public async Task<AttendanceStatisticsResponse?> GetStatisticsByLevelAndEmployeeAsync(
    int level, Guid? employeeID, int? year, int? month, int? day, int? startYear,
    int? endYear, int? startMonth, int? endMonth, int? startDay, int? endDay)
        {
            _logger.LogInformation($"{nameof(GetStatisticsByLevelAndEmployeeAsync)}: Fetching data from repository.");

            try
            {
                await _context.Database.OpenConnectionAsync();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = @"
                    SELECT * FROM get_statistics_by_level_and_employee2(
                        @p_level, @p_employee_id, @p_year, @p_startYear, @p_endYear, 
                        @p_month, @p_startMonth, @p_endMonth, @p_day, @p_startDay, @p_endDay)";
                    command.CommandType = System.Data.CommandType.Text;

                    command.Parameters.Add(new NpgsqlParameter("@p_level", NpgsqlTypes.NpgsqlDbType.Integer) { Value = level });
                    command.Parameters.Add(new NpgsqlParameter("@p_employee_id", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = (object?)employeeID ?? DBNull.Value });
                    command.Parameters.Add(new NpgsqlParameter("@p_year", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object?)year ?? DBNull.Value });
                    command.Parameters.Add(new NpgsqlParameter("@p_month", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object?)month ?? DBNull.Value });
                    command.Parameters.Add(new NpgsqlParameter("@p_day", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object?)day ?? DBNull.Value });
                    command.Parameters.Add(new NpgsqlParameter("@p_startYear", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object?)startYear ?? DBNull.Value });
                    command.Parameters.Add(new NpgsqlParameter("@p_endYear", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object?)endYear ?? DBNull.Value });
                    command.Parameters.Add(new NpgsqlParameter("@p_startMonth", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object?)startMonth ?? DBNull.Value });
                    command.Parameters.Add(new NpgsqlParameter("@p_endMonth", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object?)endMonth ?? DBNull.Value });
                    command.Parameters.Add(new NpgsqlParameter("@p_startDay", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object?)startDay ?? DBNull.Value });
                    command.Parameters.Add(new NpgsqlParameter("@p_endDay", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object?)endDay ?? DBNull.Value });

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new AttendanceStatisticsResponse
                            {
                                level = reader.GetInt32(reader.GetOrdinal("level")),
                                workingHours = reader.GetDouble(reader.GetOrdinal("workingHours")),
                                attendancePercentage = reader.GetDouble(reader.GetOrdinal("attendancePercentage"))
                            };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetStatisticsByLevelAndEmployeeAsync)}: Error occurred.");
                throw;
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

        public async Task<int?> IsValidLevel(int level)
        {
            _logger.LogInformation($"{nameof(IsValidLevel)}: AttendanceStatisticsRepository.");

            try
            {
                await _context.Database.OpenConnectionAsync();
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = $"SELECT * from get_level(@level)";
                cmd.Parameters.Add(new NpgsqlParameter("@level", NpgsqlTypes.NpgsqlDbType.Integer) { Value = level });
                using (cmd)
                {
                    var obj = await cmd.ExecuteScalarAsync();
                    if (obj != null && int.TryParse(obj.ToString(), out int levelId))
                    {
                        return levelId;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(IsValidLevel)}: AttendanceStatisticsRepository.");
                throw;
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

        public async Task<List<AttendancesDTO>> GetAttendancesForDateAsync(DateOnly date, Guid employeeID)
        {
            _logger.LogInformation($"{nameof(GetAttendancesForDateAsync)}: AttendanceStatisticsRepository.");

            var attendances = new List<AttendancesDTO>();

            try
            {
                await _context.Database.OpenConnectionAsync();

                var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT * FROM get_attendances(@date, @employeeID)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@date", NpgsqlTypes.NpgsqlDbType.Date) { Value = date });
                command.Parameters.Add(new NpgsqlParameter("@employeeID", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = employeeID });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        attendances.Add(new AttendancesDTO
                        {
                            attendanceDate = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("attendanceDate")),
                            checkIn = reader.IsDBNull(reader.GetOrdinal("checkIn")) ? (TimeSpan?)null : reader.GetFieldValue<TimeSpan>(reader.GetOrdinal("checkIn")),
                            checkOut = reader.IsDBNull(reader.GetOrdinal("checkOut")) ? (TimeSpan?)null : reader.GetFieldValue<TimeSpan>(reader.GetOrdinal("checkOut"))
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAttendancesForDateAsync)}: AttendanceStatisticsRepository.");
                throw;
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return attendances;
        }

        public async Task<IEnumerable<AttendancesDTO>> GetAttendancesForDateRangeAsync(DateOnly startDate, DateOnly endDate, Guid employeeID)
        {
            _logger.LogInformation($"{nameof(GetAttendancesForDateRangeAsync)}: AttendanceStatisticsRepository.");

            var attendances = new List<AttendancesDTO>();

            try
            {
                await _context.Database.OpenConnectionAsync();

                var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT * FROM get_attendances_for_date_range(@startDate, @endDate, @employeeID)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@startDate", NpgsqlTypes.NpgsqlDbType.Date) { Value = startDate });
                command.Parameters.Add(new NpgsqlParameter("@endDate", NpgsqlTypes.NpgsqlDbType.Date) { Value = endDate });
                command.Parameters.Add(new NpgsqlParameter("@employeeID", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = employeeID });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        attendances.Add(new AttendancesDTO
                        {
                            attendanceDate = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("attendanceDate")),
                            checkIn = reader.IsDBNull(reader.GetOrdinal("checkIn")) ? (TimeSpan?)null : reader.GetFieldValue<TimeSpan>(reader.GetOrdinal("checkIn")),
                            checkOut = reader.IsDBNull(reader.GetOrdinal("checkOut")) ? (TimeSpan?)null : reader.GetFieldValue<TimeSpan>(reader.GetOrdinal("checkOut"))
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAttendancesForDateRangeAsync)}: AttendanceStatisticsRepository.");
                throw;
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return attendances;
        }

        public async Task<List<MonthlyAttendancePercentageDTO>> GetMonthlyAttendancePercentageAsync(int year)
        {
            _logger.LogInformation($"{nameof(GetMonthlyAttendancePercentageAsync)}: AttendanceStatisticsRepository.");

            try
            {
                // Call the SQL function and map results to MonthlyAttendancePercentageDTO
                var result = await _context.MonthlyAttendancePercentageDTO
                    .FromSqlRaw("SELECT * FROM get_monthly_attendance_percentage({0})", year)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetMonthlyAttendancePercentageAsync)}: AttendanceStatisticsRepository.");
                throw;
            }
        }
    }
}
