using Common.DTOs.Response;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTOs.Request;

namespace Data
{
    public interface IAttendanceStatisticsRepository
    {
        Task AddStatisticsAsync(AttendanceStatistics statistics);
        Task UpdateStatisticsAsync(AttendanceStatistics statistics);
        Task<AttendanceStatistics?> GetStatisticsByLevelAndDateAsync(int level, DateOnly date, Guid employeeID);
        Task<AttendanceStatisticsResponse?> GetStatisticsByLevelAndEmployeeAsync(int level, Guid? employeeID, int? year, int? month, int? day, int? startYear, int? endYear, int? startMonth, int? endMonth, int? startDay, int? endDay);
        Task<int?> IsValidLevel(int level);
        Task<List<AttendancesDTO>> GetAttendancesForDateAsync(DateOnly date, Guid employeeID);
        Task<IEnumerable<AttendancesDTO>> GetAttendancesForDateRangeAsync(DateOnly startDate, DateOnly endDate, Guid employeeID);
        Task<List<MonthlyAttendancePercentageDTO>> GetMonthlyAttendancePercentageAsync(int year);
    }
}
