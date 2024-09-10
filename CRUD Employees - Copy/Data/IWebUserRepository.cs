using Common.DTOs.Request;
using Common.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IWebUserRepository
    {
        Task AddWebUser(WebUserDto? webuser);
        Task<PaginatedWebUsersResponse> GetAllWebUsersAsync(int page, int pageSize, string? generalSearch);
        Task<IEnumerable<ExportWebUsersResponse>> ExportAllWebUsersAsync();
        Task DeleteWebUserAsync(Guid webUserId);
        Task EditWebUser(Guid webUserId, Dictionary<string, object> updates);
        Task<AllWebUsers?> GetWebUserDetails(Guid WebUserId);
        Task<PaginatedWebUsersResponse> GetWebUserByEmail(int page, int pageSize, string? userEmail);
    }
}
