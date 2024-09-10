using Common.DTOs.Request;
using Common.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Business
{
    public interface IWebUserService
    {
        Task<ApiResponse> AddWebUserAsync(WebUserDto webuser);
        Task<PaginatedWebUsersResponse> GetAllWebUsersAsync(int page, int pageSize, string? generalSearch);
        Task<IEnumerable<ExportWebUsersResponse>> ExportAllWebUsersAsync();
        Task DeleteWebUserAsync(Guid webUserId);
        Task<bool> EditWebUser(Guid webUserId, Dictionary<string, object> updates);
        Task<AllWebUsers?> GetWebUserDetails(Guid WebUserId);
        Task<PaginatedWebUsersResponse> GetWebUserByEmail(int page, int pageSize, string? userEmail);
    }
}
