﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ARM.DAL.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CabinetParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabinetParts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cabinets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Fullname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cabinets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Patronymic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Birthday = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Passport = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JwtId = table.Column<string>(type: "text", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSalaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    SalaryForOneHour = table.Column<decimal>(type: "numeric", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSalaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSalaries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Taks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CabinetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CurrentPerformerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Deadline = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EstimatedWorkHours = table.Column<int>(type: "integer", nullable: false),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taks_Cabinets_CabinetId",
                        column: x => x.CabinetId,
                        principalTable: "Cabinets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Taks_Employees_CurrentPerformerId",
                        column: x => x.CurrentPerformerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CabinetPartCounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CabinetPartId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabinetPartCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CabinetPartCounts_CabinetParts_CabinetPartId",
                        column: x => x.CabinetPartId,
                        principalTable: "CabinetParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabinetPartCounts_Taks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Taks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskEmployees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskEmployees_Taks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Taks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Taks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Taks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkedTimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Hours = table.Column<int>(type: "integer", nullable: false),
                    IsOverwork = table.Column<bool>(type: "boolean", nullable: false),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkedTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkedTimes_Taks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Taks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkedTimes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CabinetPartCounts_CabinetPartId_TaskId",
                table: "CabinetPartCounts",
                columns: new[] { "CabinetPartId", "TaskId" },
                unique: true,
                filter: "\"IsActual\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_CabinetPartCounts_TaskId",
                table: "CabinetPartCounts",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CabinetParts_Name",
                table: "CabinetParts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cabinets_Name",
                table: "Cabinets",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TaskId",
                table: "Comments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId_TaskId",
                table: "Comments",
                columns: new[] { "UserId", "TaskId" },
                filter: "\"IsActual\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_FirstName_LastName_Patronymic",
                table: "Employees",
                columns: new[] { "FirstName", "LastName", "Patronymic" },
                filter: "\"IsActual\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Passport",
                table: "Employees",
                column: "Passport",
                unique: true,
                filter: "\"IsActual\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_EmployeeId",
                table: "EmployeeSalaries",
                column: "EmployeeId",
                unique: true,
                filter: "\"IsActual\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Taks_CabinetId",
                table: "Taks",
                column: "CabinetId");

            migrationBuilder.CreateIndex(
                name: "IX_Taks_CurrentPerformerId",
                table: "Taks",
                column: "CurrentPerformerId");

            migrationBuilder.CreateIndex(
                name: "IX_Taks_Name",
                table: "Taks",
                column: "Name",
                filter: "\"IsActual\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEmployees_EmployeeId_TaskId",
                table: "TaskEmployees",
                columns: new[] { "EmployeeId", "TaskId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskEmployees_TaskId",
                table: "TaskEmployees",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                table: "Users",
                column: "EmployeeId",
                unique: true,
                filter: "\"IsActual\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true,
                filter: "\"IsActual\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login_PasswordHash",
                table: "Users",
                columns: new[] { "Login", "PasswordHash" },
                unique: true,
                filter: "\"IsActual\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_WorkedTimes_TaskId",
                table: "WorkedTimes",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkedTimes_UserId_TaskId",
                table: "WorkedTimes",
                columns: new[] { "UserId", "TaskId" },
                filter: "\"IsActual\" = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CabinetPartCounts");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "EmployeeSalaries");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "TaskEmployees");

            migrationBuilder.DropTable(
                name: "WorkedTimes");

            migrationBuilder.DropTable(
                name: "CabinetParts");

            migrationBuilder.DropTable(
                name: "Taks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cabinets");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
