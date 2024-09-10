using Common.DTOs.Request;
using Common.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public interface IAttendanceStatisticsService
    {
        Task UpdateStatisticsAsync(Guid employeeId, DateOnly date);
        Task UpdateLevel1StatisticsAsync(DateOnly date, Guid employeeID);
        Task UpdateLevel2StatisticsAsync(DateOnly date, Guid employeeID);
        Task UpdateLevel3StatisticsAsync(DateOnly date, Guid employeeID);
        Task<AttendanceStatisticsResponse?> GetStatisticsByLevelAndEmployeeAsync(int level, Guid? employeeID, int? year, int? month,
        int? day, int? startYear, int? endYear, int? startMonth, int? endMonth, int? startDay, int? endDay);
        Task<int?> IsValidLevel(int level);
        Task<YearlyAttendanceTrendsDTO> GetYearlyAttendanceTrendsAsync(int year);
    }
}
