using Business;
using Common.DTOs.Response;
using Common.Helpers;
using Common.Models;
using EmployeeManagement.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileUploadController> _logger;
        private readonly IEmployeeService _employeeService;
        public FileUploadController(IConfiguration configuration, ILogger<FileUploadController> logger, IEmployeeService employeeService, IFileUploadService fileUploadService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _configuration = configuration;
            _fileUploadService = fileUploadService;
        }

        [HttpPost("uploadProfilePicture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] fileUploadModel model, [FromForm] Guid employeeId)
        {
            _logger.LogInformation($"{nameof(UploadProfilePicture)} : EmployeeController.");

            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            // Validate file type (e.g., JPEG, PNG)
            var fileExtension = Path.GetExtension(model.File.FileName).ToLower();
            if (fileExtension != ".jpg" && fileExtension != ".png")
            {
                return BadRequest("Invalid file type.");
            }

            // Get employee and username
            var employee = await _employeeService.GetEmployee(employeeId);
            if (employee == null)
            {
                return BadRequest ("No employee found");
            }

            string directoryPath = _configuration.GetValue<string>("FilePaths:ProfilePictures");
            string filePath = Path.Combine(directoryPath, $"{employee.fullName}-{employee.employeeIdentifier}{fileExtension}");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Save the file to the server
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                    _logger.LogInformation("File saved successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while saving the file: {ex.Message}");
                return BadRequest("Internal Server Error");
            }

            await _fileUploadService.UpdateProfilePictureUrlAsync(employeeId, filePath);

            return Ok();
        }


    }
}
