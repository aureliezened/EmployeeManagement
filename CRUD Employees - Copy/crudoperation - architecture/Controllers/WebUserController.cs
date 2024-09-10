using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
using Dapper;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace EmployeeManagement.Controllers
{

    [Route("API/[controller]")]
    [ApiController]
    public class WebUserController : ControllerBase
    {

        private readonly ILogger<WebUserController> _logger;
        private readonly IWebUserService _webUserService;

        public WebUserController(ILogger<WebUserController> logger, IWebUserService webUserService)
        {
            _logger = logger;
            _webUserService = webUserService;

        }


        [HttpPost("Add-Web-User")]
        public async Task<ApiResponse> AddWebUser([FromBody] WebUserDto webuser)
        {
            _logger.LogInformation($"{nameof(AddWebUser)} : WebUserController.");
            try
            {
                if (webuser == null)
                {
                    _logger.LogError("Invalid web user data provided");
                    var errorResponse = StatusCodeHelper.GetStatusResponseWithoutType(3);
                    return errorResponse;
                }

                var result = await _webUserService.AddWebUserAsync(webuser);
                if (result.statusCode == 200)
                {
                    return result;
                }
                else return StatusCodeHelper.GetStatusResponseWithoutType(1);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddWebUser)}: WebUserController.");
                return StatusCodeHelper.GetStatusResponseWithoutType(1);
                throw;
            }
        }

        [HttpGet("View-Web-Users")]
        public async Task<ApiResponseType<PaginatedWebUsersResponse?>> GetAllWebUsers(int page = 1, int pageSize = 10, string? generalSearch = null )
        {
            _logger.LogInformation($"{nameof(GetAllWebUsers)} : WebUserController.");

            try
            {
                var webUsers = await _webUserService.GetAllWebUsersAsync(page, pageSize, generalSearch);

                if (webUsers == null)
                {
                    _logger.LogError("No web users found.");
                    return StatusCodeHelper.GetStatusResponse(6, (PaginatedWebUsersResponse?)null);
                }

                return StatusCodeHelper.GetStatusResponse(200, webUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllWebUsers)}: WebUserController.");
                return StatusCodeHelper.GetStatusResponse(1, (PaginatedWebUsersResponse?)null);
            }
        }

        [HttpDelete("Delete-Web-User")]
        public async Task<ApiResponse> DeleteWebUser(Guid webUserId)
        {
            _logger.LogInformation($"{nameof(DeleteWebUser)} : WebUserController.");

            try
            {
                await _webUserService.DeleteWebUserAsync(webUserId);
                return StatusCodeHelper.GetStatusResponseWithoutType(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(DeleteWebUser)}: WebUserController.");

                return StatusCodeHelper.GetStatusResponseWithoutType(1);
            }
        }

        [HttpPost("Edit-Web-User")]
        public async Task<ApiResponse> EditWebUser(Guid employeeId, Dictionary<string, object> updates)
        {
            _logger.LogInformation($"{nameof(EditWebUser)} : WebUserController.");
            try
            {
                var result = await _webUserService.EditWebUser(employeeId, updates);
                if (!result)
                {
                    return StatusCodeHelper.GetStatusResponseWithoutType(400);
                }

                else return StatusCodeHelper.GetStatusResponseWithoutType(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EditWebUser)}: WebUserController.");
                return StatusCodeHelper.GetStatusResponseWithoutType(1);
            }
        }

        [HttpGet("Export-Web-Users")]
        public async Task<ApiResponseType<FileResponse?>> ExportAllWebUsers()
        {
            _logger.LogInformation($"{nameof(ExportAllWebUsers)} : WebUserController.");

            try
            {
                var exportData = await _webUserService.ExportAllWebUsersAsync();

                if (exportData == null || !exportData.Any())
                {
                    return StatusCodeHelper.GetStatusResponse<FileResponse?>(6, null);
                }

                // Convert to CSV
                var csv = ExportHelper.ConvertToCsv(exportData);
                var fileBytes = System.Text.Encoding.UTF8.GetBytes(csv);

                var fileResponse = new FileResponse
                {
                    FileContent = fileBytes,
                    FileName = "WebUsers.csv",
                    MimeType = "text/csv"
                };

                return StatusCodeHelper.GetStatusResponse<FileResponse?>(200, fileResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(ExportAllWebUsers)}: WebUserController.");
                return StatusCodeHelper.GetStatusResponse<FileResponse?>(1, null);
            }
        }

        //single web user
        [HttpGet("Get-WebUser-details")]
        public async Task<ApiResponseType<AllWebUsers?>> GetWebUserDetails(Guid WebUserId)
        {
            _logger.LogInformation($"{nameof(GetWebUserDetails)} : WebUserController.");

            try
            {
                var webUser = await _webUserService.GetWebUserDetails(WebUserId);
                return StatusCodeHelper.GetStatusResponse(200, webUser);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetWebUserDetails)}: WebUserController.");
                return StatusCodeHelper.GetStatusResponse(1, (AllWebUsers?)null);

            }

        }

        [HttpGet("View-Web-User-By-Email")]
        public async Task<ApiResponseType<PaginatedWebUsersResponse?>> GetWebUserByEmail(int page = 1, int pageSize = 10, string? userEmail = null )
        {
            _logger.LogInformation($"{nameof(GetWebUserByEmail)} : WebUserController.");

            try
            {
                var webUsers = await _webUserService.GetWebUserByEmail(page, pageSize, userEmail);

                if (webUsers == null)
                {
                    _logger.LogError("No web user found.");
                    return StatusCodeHelper.GetStatusResponse(6, (PaginatedWebUsersResponse?)null);
                }

                return StatusCodeHelper.GetStatusResponse(200, webUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetWebUserByEmail)}: WebUserController.");
                return StatusCodeHelper.GetStatusResponse(1, (PaginatedWebUsersResponse?)null);
            }
        }


    }
}
