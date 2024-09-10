using Business;
using Common.DTOs.Response;
using Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceStatisticsController : ControllerBase
    {
        private readonly IAttendanceStatisticsService _attendanceStatisticsService;
        private readonly ILogger<AttendanceStatisticsController> _logger;
        private readonly IEmployeeService _employeeService;


        public AttendanceStatisticsController(IAttendanceStatisticsService attendanceStatisticsService, ILogger<AttendanceStatisticsController> logger, IEmployeeService employeeService)
        {
            _attendanceStatisticsService = attendanceStatisticsService;
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("Statistics-Values")]
        public async Task<ApiResponseType<AttendanceStatisticsResponse?>> GetStatisticsByLevelAndEmployee(
            int level,
            Guid? employeeID,
            int? year = null,
            int? month = null,
            int? day = null,
            int? startYear = null,
            int? endYear = null,
            int? startMonth = null,
            int? endMonth = null,
            int? startDay = null,
            int? endDay = null)
        {
            _logger.LogInformation($"{nameof(GetStatisticsByLevelAndEmployee)} : AttendanceStatisticsController.");

            try
            {
                if (employeeID.HasValue)
                {
                    bool? employeeExists = await _employeeService.CheckEmployeeExistsAsync(employeeID.Value);
                    if ((bool)!employeeExists)
                    {
                        var notFoundResponse = StatusCodeHelper.GetStatusResponse(8, (AttendanceStatisticsResponse?)null);
                        return notFoundResponse;
                    }
                }

                int? levelId = await _attendanceStatisticsService.IsValidLevel(level);
                if (levelId == null)
                {
                    var errorResponse = StatusCodeHelper.GetStatusResponse(6, (AttendanceStatisticsResponse?)null);
                    return errorResponse;
                }

                var statistics = await _attendanceStatisticsService.GetStatisticsByLevelAndEmployeeAsync(level, employeeID, year, month, day, startYear, endYear, startMonth, endMonth, startDay, endDay);

                var response = StatusCodeHelper.GetStatusResponse(200, statistics);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetStatisticsByLevelAndEmployee)}: AttendanceStatisticsController.");
                var errorResponse = StatusCodeHelper.GetStatusResponseNotNull(1, (AttendanceStatisticsResponse?)null);
                return errorResponse;
            }
        }

        [HttpGet("Attendance-Trends")]
        public async Task<ApiResponseType<YearlyAttendanceTrendsDTO?>> GetYearlyAttendanceTrends(int year)
        {
            _logger.LogInformation($"{nameof(GetYearlyAttendanceTrends)} : AttendanceStatisticsController.");

            try
            {
                var trends = await _attendanceStatisticsService.GetYearlyAttendanceTrendsAsync(year);
                var response = StatusCodeHelper.GetStatusResponse(200, trends);
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetYearlyAttendanceTrends)}: AttendanceStatisticsController.");
                var errorResponse = StatusCodeHelper.GetStatusResponseNotNull(1, (YearlyAttendanceTrendsDTO?)null);
                return errorResponse;
            }
        }
    }
}

