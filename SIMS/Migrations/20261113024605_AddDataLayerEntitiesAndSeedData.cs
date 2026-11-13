using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SIMS.Migrations
{
    /// <inheritdoc />
    public partial class AddDataLayerEntitiesAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollmentDate",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "FacultyId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "Courses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ScheduleTime",
                table: "Courses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TargetEntity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceScore = table.Column<double>(type: "float", nullable: true),
                    MidtermScore = table.Column<double>(type: "float", nullable: true),
                    AssignmentScore = table.Column<double>(type: "float", nullable: true),
                    FinalScore = table.Column<double>(type: "float", nullable: true),
                    TotalScore = table.Column<double>(type: "float", nullable: true),
                    LetterGrade = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EnrollmentId);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.FacultyId);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsUnread = table.Column<bool>(type: "bit", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SemesterGpaHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Semester = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gpa = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterGpaHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SemesterGpaHistories_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuditLogs",
                columns: new[] { "Id", "Action", "PerformedBy", "Role", "Severity", "TargetEntity", "Timestamp" },
                values: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), "Database Synchronized", "System Administrator", "Admin", "Info", "Database: SIMSDatabase", new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Faculties",
                columns: new[] { "FacultyId", "Department", "FacultyName" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "Department of Software Engineering & Architecture", "Dr. Michael Vance" });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Email", "EnrollmentDate", "FullName", "Gpa", "Program", "Status", "StudentCode" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), "ductvbh02383@fpt.edu.vn", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tran Vu Duc", 3.8199999999999998, "Software Engineering", "Active", "BH02383" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "CourseCode", "CourseName", "Credits", "FacultyId", "Room", "ScheduleTime" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222221"), "APDP301", "Applied Programming & Design Principles", 4, new Guid("11111111-1111-1111-1111-111111111111"), "Lab 402 - Tech Wing", "Mon, Wed (09:00 - 11:30)" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "CS402", "Cloud Architecture & Microservices", 3, new Guid("11111111-1111-1111-1111-111111111111"), "Auditorium 2B", "Tue, Thu (13:30 - 15:30)" }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "Category", "IsUnread", "Message", "StudentId", "Timestamp", "Title" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555551"), "Exam", true, "APDP301 design presentation is confirmed for next Monday at 09:00 AM.", new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 11, 12, 8, 0, 0, 0, DateTimeKind.Utc), "Midterm Examination Scheduled" },
                    { new Guid("55555555-5555-5555-5555-555555555552"), "Notice", false, "Your enrolled semester courses are now active in SIMS.", new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 11, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Course Schedule Published" }
                });

            migrationBuilder.InsertData(
                table: "SemesterGpaHistories",
                columns: new[] { "Id", "Gpa", "Semester", "StudentId" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444441"), 3.6499999999999999, "Sem 1", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("44444444-4444-4444-4444-444444444442"), 3.7200000000000002, "Sem 2", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("44444444-4444-4444-4444-444444444443"), 3.7999999999999998, "Sem 3", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 3.8199999999999998, "Sem 4", new Guid("33333333-3333-3333-3333-333333333333") }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "EnrollmentId", "AssignmentScore", "AttendanceScore", "CourseId", "FinalScore", "LetterGrade", "MidtermScore", "Note", "Status", "StudentId", "TotalScore" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666661"), 19.0, 10.0, new Guid("22222222-2222-2222-2222-222222222221"), 38.0, "A+", 28.5, "Outstanding architectural documentation.", "Submitted", new Guid("33333333-3333-3333-3333-333333333333"), 95.5 },
                    { new Guid("66666666-6666-6666-6666-666666666662"), 18.0, 9.5, new Guid("22222222-2222-2222-2222-222222222222"), 34.0, "A-", 25.0, "Solid cloud architecture patterns.", "Submitted", new Guid("33333333-3333-3333-3333-333333333333"), 86.5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_FacultyId",
                table: "Courses",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId_CourseId",
                table: "Enrollments",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_StudentId",
                table: "Notifications",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterGpaHistories_StudentId",
                table: "SemesterGpaHistories",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Faculties_FacultyId",
                table: "Courses",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Faculties_FacultyId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "SemesterGpaHistories");

            migrationBuilder.DropIndex(
                name: "IX_Courses_FacultyId",
                table: "Courses");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DropColumn(
                name: "EnrollmentDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ScheduleTime",
                table: "Courses");
        }
    }
}
