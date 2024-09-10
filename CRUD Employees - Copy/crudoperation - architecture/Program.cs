using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using Data;
using Business;
using EmployeeManagement.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using EmployeeManagement.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

if (Environment.OSVersion.Platform == PlatformID.Unix)
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(builder.Configuration.GetValue<int>("LinuxPort"));
    });
}
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization Header using the Bearer scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

}); builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IWebUserService, WebUserService>();
builder.Services.AddScoped<IWebUserRepository, WebUserRepository>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IAttendanceStatisticsService, AttendanceStatisticsService>();
builder.Services.AddScoped<IAttendanceStatisticsRepository, AttendanceStatisticsRepository>();

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Trace);
});

builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);

var tokenValidationParameter = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
    RequireExpirationTime = false,
    ValidateLifetime = true


};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwt =>
    {

        jwt.SaveToken = true;
        jwt.TokenValidationParameters = tokenValidationParameter;

    });

builder.Services.AddSingleton(tokenValidationParameter);



var app = builder.Build();


app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
