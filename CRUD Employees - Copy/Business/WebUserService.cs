using Common.DTOs.Request;
using Common.DTOs.Response;
using Common.Helpers;
using Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class WebUserService : IWebUserService
    {

        private readonly IWebUserRepository _webUserRepository;
        private readonly ILogger<WebUserService> _logger;


        public WebUserService(IWebUserRepository webUserRepository, ILogger<WebUserService> logger)
        {
            _webUserRepository = webUserRepository;
            _logger = logger;
        }
        public async Task<ApiResponse> AddWebUserAsync(WebUserDto webuser)
        {
            _logger.LogInformation($"{nameof(AddWebUserAsync)}: WebUserService.");

            try
            {
                if (
                string.IsNullOrWhiteSpace(webuser.fullName) ||
                string.IsNullOrWhiteSpace(webuser.userName) ||
                string.IsNullOrWhiteSpace(webuser.email) ||
                string.IsNullOrWhiteSpace(webuser.password) ||
                string.IsNullOrWhiteSpace(webuser.webRole) )
        
                {
                    _logger.LogError("Invalid or incomplete employee data provided");
                    var errorResponse = StatusCodeHelper.GetStatusResponseWithoutType(2);
                    return errorResponse;

                }

                await _webUserRepository.AddWebUser(webuser);
                var response = StatusCodeHelper.GetStatusResponseWithoutType(200);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddWebUserAsync)}: WebUserService.");
                throw;
            }
        }

        public async Task<PaginatedWebUsersResponse> GetAllWebUsersAsync(int page, int pageSize, string? generalSearch)
        {
            _logger.LogInformation($"{nameof(GetAllWebUsersAsync)}: WebUserService.");
            try
            {
                return await _webUserRepository.GetAllWebUsersAsync(page, pageSize,generalSearch);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllWebUsersAsync)}: WebUserService.");
                throw;

            }
        }

        public async Task<IEnumerable<ExportWebUsersResponse>> ExportAllWebUsersAsync()
        {
            _logger.LogInformation($"{nameof(ExportAllWebUsersAsync)}: WebUserService.");
            try
            {
                return await _webUserRepository.ExportAllWebUsersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(ExportAllWebUsersAsync)}: WebUserService.");
                throw;
            }
        }

        public async Task DeleteWebUserAsync(Guid webUserId)
        {
            _logger.LogInformation($"{nameof(DeleteWebUserAsync)}: WebUserService.");

            try
            {
                await _webUserRepository.DeleteWebUserAsync(webUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(DeleteWebUserAsync)}: WebUserService.");
                throw;
            }
        }

        public async Task<bool> EditWebUser(Guid webUserId, Dictionary<string, object> updates)
        {
            _logger.LogInformation($"{nameof(EditWebUser)}: WebUserService.");
            try
            {
                if (updates == null || updates.Count == 0)
                {
                    return false; 
                }

                await _webUserRepository.EditWebUser(webUserId, updates);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EditWebUser)}: WebUserService.");
                throw;
            }
        }

        public async Task<AllWebUsers?> GetWebUserDetails(Guid WebUserId)
        {
            _logger.LogInformation($"{nameof(GetWebUserDetails)}: WebUserService.");
            try
            {
                return await _webUserRepository.GetWebUserDetails(WebUserId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetWebUserDetails)}: WebUserService.");
                throw;

            }

        }

        public async Task<PaginatedWebUsersResponse> GetWebUserByEmail(int page, int pageSize, string? userEmail)
        {
            _logger.LogInformation($"{nameof(GetWebUserByEmail)}: WebUserService.");
            try
            {
                return await _webUserRepository.GetWebUserByEmail(page, pageSize, userEmail);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetWebUserByEmail)}: WebUserService.");
                throw;

            }
        }

    }
}
