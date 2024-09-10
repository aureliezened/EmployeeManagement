using Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IFileUploadRepository _fileUploadRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(IFileUploadRepository fileUploadRepository, ILogger<FileUploadService> logger, IEmployeeRepository employeeRepository)
        {
            _fileUploadRepository = fileUploadRepository;
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public async Task UpdateProfilePictureUrlAsync(Guid employeeId, string profilePictureUrl)
        {
            _logger.LogInformation($"{nameof(UpdateProfilePictureUrlAsync)}: EmployeeService.");

            try
            {
                var updates = new Dictionary<string, object>
                {
                    { "profilePictureUrl", profilePictureUrl }
                };

                await _employeeRepository.EditEmployee(employeeId, updates);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UpdateProfilePictureUrlAsync)}: EmployeeService.");
                throw;

            }
        }

    }
}
