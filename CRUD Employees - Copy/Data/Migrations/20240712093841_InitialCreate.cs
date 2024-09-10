using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceStatuses",
                columns: table => new
                {
                    statusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusName = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceStatuses", x => x.statusId);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    branchId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    branchName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.branchId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    departmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    departmentName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.departmentId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeStatuses",
                columns: table => new
                {
                    statusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    statusName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeStatuses", x => x.statusId);
                });

            migrationBuilder.CreateTable(
                name: "JobTitles",
                columns: table => new
                {
                    jobId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    jobName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.jobId);
                });

            migrationBuilder.CreateTable(
                name: "WebRoles",
                columns: table => new
                {
                    webRoleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    webRoleName = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebRoles", x => x.webRoleId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    employeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    employeeIdentifier = table.Column<int>(type: "integer", nullable: false),
                    fullName = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    birthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    phoneNumber = table.Column<string>(type: "text", nullable: true),
                    jobTitle = table.Column<int>(type: "integer", nullable: false),
                    department = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    branch = table.Column<int>(type: "integer", nullable: false),
                    profilePicture = table.Column<string>(type: "text", nullable: false),
                    joinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.employeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Branches_branch",
                        column: x => x.branch,
                        principalTable: "Branches",
                        principalColumn: "branchId");
                    table.ForeignKey(
                        name: "FK_Employees_Departments_department",
                        column: x => x.department,
                        principalTable: "Departments",
                        principalColumn: "departmentId");
                    table.ForeignKey(
                        name: "FK_Employees_EmployeeStatuses_status",
                        column: x => x.status,
                        principalTable: "EmployeeStatuses",
                        principalColumn: "statusId");
                    table.ForeignKey(
                        name: "FK_Employees_JobTitles_jobTitle",
                        column: x => x.jobTitle,
                        principalTable: "JobTitles",
                        principalColumn: "jobId");
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    attendanceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    attendanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    checkIn = table.Column<TimeSpan>(type: "interval", nullable: false),
                    checkOut = table.Column<TimeSpan>(type: "interval", nullable: false),
                    employeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.attendanceId);
                    table.ForeignKey(
                        name: "FK_Attendances_AttendanceStatuses_status",
                        column: x => x.status,
                        principalTable: "AttendanceStatuses",
                        principalColumn: "statusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Employees_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employees",
                        principalColumn: "employeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebUsers",
                columns: table => new
                {
                    webUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    webUserIdentifier = table.Column<int>(type: "integer", nullable: false),
                    employeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fullName = table.Column<string>(type: "text", nullable: false),
                    userName = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    msisdn = table.Column<string>(type: "text", nullable: true),
                    webRole = table.Column<int>(type: "integer", nullable: false),
                    isPassChanged = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebUsers", x => x.webUserId);
                    table.ForeignKey(
                        name: "FK_WebUsers_Employees_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employees",
                        principalColumn: "employeeId");
                    table.ForeignKey(
                        name: "FK_WebUsers_WebRoles_webRole",
                        column: x => x.webRole,
                        principalTable: "WebRoles",
                        principalColumn: "webRoleId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_employeeId",
                table: "Attendances",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_status",
                table: "Attendances",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_branch",
                table: "Employees",
                column: "branch");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_department",
                table: "Employees",
                column: "department");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_jobTitle",
                table: "Employees",
                column: "jobTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_status",
                table: "Employees",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_WebUsers_employeeId",
                table: "WebUsers",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WebUsers_webRole",
                table: "WebUsers",
                column: "webRole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "WebUsers");

            migrationBuilder.DropTable(
                name: "AttendanceStatuses");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "WebRoles");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "EmployeeStatuses");

            migrationBuilder.DropTable(
                name: "JobTitles");
        }
    }
}
