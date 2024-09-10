using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class FileUploadRepository : IFileUploadRepository
    {
        private readonly ILogger<FileUploadRepository> _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public FileUploadRepository(ILogger<FileUploadRepository> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;   
        }

        public async Task UpdateProfilePictureUrlAsync(Guid employeeId, string profilePictureUrl)
        {
            try
            {
                var updates = new Dictionary<string, object>
                  {
                         { "ProfilePictureUrl", profilePictureUrl }
                  };

                await _employeeRepository.EditEmployee(employeeId, updates);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UpdateProfilePictureUrlAsync)}: EmployeeRepository.");
                throw;

            }

        }

    }
}
