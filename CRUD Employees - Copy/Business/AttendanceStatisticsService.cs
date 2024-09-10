using Common.DTOs.Request;
using Common.DTOs.Response;
using Common.Models;
using Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AttendanceStatisticsService : IAttendanceStatisticsService
    {
        private readonly IAttendanceStatisticsRepository _attendanceStatisticsRepository;
        private readonly ILogger<AttendanceStatisticsService> _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public AttendanceStatisticsService(IAttendanceStatisticsRepository attendanceStatisticsRepository, ILogger<AttendanceStatisticsService> logger, IEmployeeRepository employeeRepository)
        {
            _attendanceStatisticsRepository = attendanceStatisticsRepository;
            _logger = logger;
            _employeeRepository = employeeRepository;
        }


        public async Task UpdateStatisticsAsync(Guid employeeId, DateOnly date)
        {
            await UpdateLevel1StatisticsAsync(date, employeeId);
            await UpdateLevel2StatisticsAsync(date, employeeId);
            await UpdateLevel3StatisticsAsync(date, employeeId);
        }

        public async Task UpdateLevel1StatisticsAsync(DateOnly date, Guid employeeID)
        {
            _logger.LogInformation($"{nameof(UpdateLevel1StatisticsAsync)}: AttendanceStatisticsService.");
            try
            {
                // Fetch all attendance entries for the specific date and employee
                var attendances = await _attendanceStatisticsRepository.GetAttendancesForDateAsync(date, employeeID);

                if (attendances == null || !attendances.Any())
                {
                    _logger.LogWarning($"{nameof(UpdateLevel1StatisticsAsync)}: No attendance records found for date {date} and employee {employeeID}.");
                    return;
                }

                var workingHours = CalculateWorkingHours(attendances);
                var attendancePercentage = CalculateAttendancePercentage(attendances, new DateOnly(date.Year, date.Month, 1), date);

                var statistics = await _attendanceStatisticsRepository.GetStatisticsByLevelAndDateAsync(1, date, employeeID);

                if (statistics == null)
                {
                    statistics = new AttendanceStatistics
                    {
                        date = date,
                        day = date.Day,
                        month = date.ToString("MMMM"),
                        year = date.Year,
                        level = 1,
                        workingHours = workingHours,
                        attendancePercentage = attendancePercentage,
                        employeeId = employeeID
                    };
                    await _attendanceStatisticsRepository.AddStatisticsAsync(statistics);
                }
                else
                {
                    statistics.workingHours = workingHours;
                    statistics.attendancePercentage = attendancePercentage;
                    await _attendanceStatisticsRepository.UpdateStatisticsAsync(statistics);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UpdateLevel1StatisticsAsync)}: AttendanceStatisticsService.");
                throw;
            }
        }

        public async Task UpdateLevel2StatisticsAsync(DateOnly date, Guid employeeID)
        {
            _logger.LogInformation($"{nameof(UpdateLevel2StatisticsAsync)}: AttendanceStatisticsService.");
            try
            {
                var startDate = new DateOnly(date.Year, date.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var attendances = await _attendanceStatisticsRepository.GetAttendancesForDateRangeAsync(startDate, endDate, employeeID);

                var workingHours = CalculateWorkingHours(attendances);
                var attendancePercentage = CalculateAttendancePercentage(attendances, startDate, endDate);

                var statistics = await _attendanceStatisticsRepository.GetStatisticsByLevelAndDateAsync(2, startDate, employeeID);

                if (statistics == null)
                {
                    statistics = new AttendanceStatistics
                    {
                        date = startDate,
                        month = startDate.ToString("MMMM"),
                        year = date.Year,
                        level = 2,
                        workingHours = workingHours,
                        attendancePercentage = attendancePercentage,
                        employeeId = employeeID
                    };
                    await _attendanceStatisticsRepository.AddStatisticsAsync(statistics);
                }
                else
                {
                    statistics.workingHours = workingHours;
                    statistics.attendancePercentage = attendancePercentage;
                    await _attendanceStatisticsRepository.UpdateStatisticsAsync(statistics);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UpdateLevel2StatisticsAsync)}: AttendanceStatisticsService.");
                throw;
            }
        }

        public async Task UpdateLevel3StatisticsAsync(DateOnly date, Guid employeeID)
        {
            _logger.LogInformation($"{nameof(UpdateLevel3StatisticsAsync)}: AttendanceStatisticsService.");
            try
            {
                var startDate = new DateOnly(date.Year, 1, 1);
                var endDate = new DateOnly(date.Year, 12, 31);
                var attendances = await _attendanceStatisticsRepository.GetAttendancesForDateRangeAsync(startDate, endDate, employeeID);

                var workingHours = CalculateWorkingHours(attendances);
                var attendancePercentage = CalculateAttendancePercentage(attendances, startDate, endDate);

                var statistics = await _attendanceStatisticsRepository.GetStatisticsByLevelAndDateAsync(3, startDate, employeeID);

                if (statistics == null)
                {
                    statistics = new AttendanceStatistics
                    {
                        date = startDate,
                        year = date.Year,
                        level = 3,
                        workingHours = workingHours,
                        attendancePercentage = attendancePercentage,
                        employeeId = employeeID
                    };
                    await _attendanceStatisticsRepository.AddStatisticsAsync(statistics);
                }
                else
                {
                    statistics.workingHours = workingHours;
                    statistics.attendancePercentage = attendancePercentage;
                    await _attendanceStatisticsRepository.UpdateStatisticsAsync(statistics);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UpdateLevel3StatisticsAsync)}: AttendanceStatisticsService.");
                throw;
            }
        }

        public async Task<AttendanceStatisticsResponse?> GetStatisticsByLevelAndEmployeeAsync(
            int level, Guid? employeeID, int? year, int? month, int? day, int? startYear, int? endYear,
            int? startMonth, int? endMonth, int? startDay, int? endDay
            )
        {
            _logger.LogInformation($"{nameof(GetStatisticsByLevelAndEmployeeAsync)}: AttendanceStatisticsService.");
            try
            {
                return await _attendanceStatisticsRepository.GetStatisticsByLevelAndEmployeeAsync(
                    level, employeeID, year, month, day, startYear, endYear, startMonth, endMonth, startDay, endDay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetStatisticsByLevelAndEmployeeAsync)}: AttendanceStatisticsService.");
                throw;
            }
        }

        public async Task<int?> IsValidLevel(int level)
        {
            _logger.LogInformation($"{nameof(IsValidLevel)}: AttendanceStatisticsService.");
            try
            {
                var levelId = await _attendanceStatisticsRepository.IsValidLevel(level);
                return levelId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(IsValidLevel)}: AttendanceStatisticsService.");
                throw;
            }
        }

        public double CalculateWorkingHours(IEnumerable<AttendancesDTO> attendances)
        {
            double totalHours = 0;

            foreach (var attendance in attendances)
            {
                if (attendance.checkIn.HasValue && attendance.checkOut.HasValue)
                {
                    var workDuration = attendance.checkOut.Value - attendance.checkIn.Value;
                    totalHours += workDuration.TotalHours;
                }
            }

            return totalHours;
        }

        public double CalculateAttendancePercentage(IEnumerable<AttendancesDTO> attendances, DateOnly startDate, DateOnly endDate)
        {
            var totalWorkingDays = GetTotalWorkingDays(startDate, endDate);
            if (totalWorkingDays == 0)
            {
                return 0;
            }

            var attendedDays = attendances
                .Select(a => a.attendanceDate)
                .Distinct()
                .Count();

            return (attendedDays / (double)totalWorkingDays) * 100;
        }

        private int GetTotalWorkingDays(DateOnly startDate, DateOnly endDate)
        {
            int totalDays = 0;
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    totalDays++;
                }
            }
            return totalDays;
        }

        public async Task<YearlyAttendanceTrendsDTO> GetYearlyAttendanceTrendsAsync(int year)
        {
            _logger.LogInformation($"{nameof(GetYearlyAttendanceTrendsAsync)}: AttendanceStatisticsService.");

            try
            {
                var monthlyData = await _attendanceStatisticsRepository.GetMonthlyAttendancePercentageAsync(year);

                var trends = new YearlyAttendanceTrendsDTO
                {
                    Year = year,
                    MonthlyTrends = monthlyData
                };

                return trends;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetYearlyAttendanceTrendsAsync)}: AttendanceStatisticsService.");
                throw;
            }
        }
    }
}
