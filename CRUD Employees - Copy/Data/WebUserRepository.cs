using Common.DTOs.Request;
using Common.DTOs.Response;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Data
{
    public class WebUserRepository : IWebUserRepository
    {

        private readonly AppDbContext _dbContext;
        private readonly ILogger<WebUserRepository> _logger;

        public WebUserRepository(AppDbContext dbContext, ILogger<WebUserRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddWebUser(WebUserDto? webuser)
        {
            _logger.LogInformation($"{nameof(AddWebUser)} : WebUserRepository.");

            try
            {
                var webUserDataJson = System.Text.Json.JsonSerializer.Serialize(webuser);
                var webUserUuid = Guid.NewGuid();


                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "CALL add_webUser(@webUser_id, @webUser_data)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@webUser_id", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = webUserUuid });
                command.Parameters.Add(new NpgsqlParameter("@webUser_data", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = webUserDataJson ?? (object)DBNull.Value });

                await _dbContext.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddWebUser)}: WebUserRepository.");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task<PaginatedWebUsersResponse> GetAllWebUsersAsync(int page, int pageSize, string? generalSearch)
        {
            _logger.LogInformation($"{nameof(GetAllWebUsersAsync)}: WebUserRepository.");

            var offset = (page - 1) * pageSize;

            try
            {
                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    var query = "SELECT * FROM get_filtered_web_users(@limit_param, @offset_param, @p_general_search);";
                    var parameters = new { limit_param = pageSize, offset_param = offset, p_general_search = generalSearch };

                    var webUsers = await connection.QueryAsync<AllWebUsers>(query, parameters);

                    var totalRecords = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM \"WebUsers\"");
                    var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                    var response = new PaginatedWebUsersResponse
                    {
                        TotalRecords = totalRecords,
                        PageSize = pageSize,
                        CurrentPage = page,
                        TotalPages = totalPages,
                        WebUsers = webUsers

                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllWebUsersAsync)}: WebUserRepository.");
                throw;
            }
        }

        public async Task<IEnumerable<ExportWebUsersResponse>> ExportAllWebUsersAsync()
        {
            _logger.LogInformation($"{nameof(ExportAllWebUsersAsync)}: WebUserRepository.");

            try
            {
                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    var query = "SELECT * FROM export_all_webusers();";

                    var webUsers = await connection.QueryAsync<AllWebUsers>(query);

                    var exportData = webUsers.Select(u => new ExportWebUsersResponse
                {
                    CreatedAt = u.createdAt,
                    FullName = u.fullName,
                    UserName = u.userName,
                    Email = u.email,
                    Msisdn = u.msisdn,
                    WebRole = u.webRole
                }).ToList();

                return exportData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(ExportAllWebUsersAsync)}: WebUserRepository.");
                throw;
            }
        }

        public async Task DeleteWebUserAsync(Guid webUserId)
        {
            _logger.LogInformation($"{nameof(DeleteWebUserAsync)}: WebUserRepository.");
            try
            {
                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT delete_web_user(@webUser_id)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@webUser_id", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = webUserId });

                await _dbContext.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(DeleteWebUserAsync)}: WebUserRepository.");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task EditWebUser(Guid webUserId, Dictionary<string, object> updates)
        {
            _logger.LogInformation($"{nameof(EditWebUser)}: WebUserRepository.");
            try
            {
                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "Select edit_web_user(@webUserId, @updates)";
                command.CommandType = System.Data.CommandType.Text;

                var updatesJson = System.Text.Json.JsonSerializer.Serialize(updates);
                command.Parameters.Add(new NpgsqlParameter("@webUserId", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = webUserId });
                command.Parameters.Add(new NpgsqlParameter("@updates", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = updatesJson });

                await _dbContext.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EditWebUser)}: WebUserRepository.");
                throw;
            }
            finally
            {
                await _dbContext.Database.CloseConnectionAsync();
            }
        }

        public async Task<AllWebUsers?> GetWebUserDetails(Guid WebUserId)
        {
            _logger.LogInformation($"{nameof(GetWebUserDetails)}: WebUserRepository.");

            try
            {
                var command = _dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT * FROM get_webUser_details(@webUID)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new NpgsqlParameter("@webUID", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = WebUserId });

                await _dbContext.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var webUser = new AllWebUsers
                        {
                            webUserId = reader.GetGuid(reader.GetOrdinal("webUserId")),
                            createdAt = reader.GetDateTime(reader.GetOrdinal("createdAt")),
                            fullName = reader.GetString(reader.GetOrdinal("fullName")),
                            userName = reader.GetString(reader.GetOrdinal("userName")),
                            email = reader.GetString(reader.GetOrdinal("email")),
                            msisdn = reader.GetString(reader.GetOrdinal("msisdn")),
                            webRole = reader.GetString(reader.GetOrdinal("webRole"))                            
                        };


                        return webUser;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetWebUserDetails)}: WebUserRepository.");
                throw;
            }
        }

        public async Task<PaginatedWebUsersResponse> GetWebUserByEmail(int page, int pageSize, string? userEmail)
        {
            _logger.LogInformation($"{nameof(GetWebUserByEmail)}: WebUserRepository.");

            var offset = (page - 1) * pageSize;

            try
            {
                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    var query = "SELECT * FROM get_filtered_web_user_by_email(@limit_param, @offset_param, @p_email);";
                    var parameters = new { limit_param = pageSize, offset_param = offset, p_email = userEmail };

                    var webUsers = await connection.QueryAsync<AllWebUsers>(query, parameters);

                    var totalRecords = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM \"WebUsers\"");
                    var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                    var response = new PaginatedWebUsersResponse
                    {
                        TotalRecords = totalRecords,
                        PageSize = pageSize,
                        CurrentPage = page,
                        TotalPages = totalPages,
                        WebUsers = webUsers

                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetWebUserByEmail)}: WebUserRepository.");
                throw;
            }
        }

    }
}
