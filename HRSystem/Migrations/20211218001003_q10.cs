using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRSystem.Migrations
{
    public partial class q10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnualVacations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualVacations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    att_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeAtt = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeLeave = table.Column<TimeSpan>(type: "time", nullable: false),
                    order = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.att_id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Emp_Ssn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emp_FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emp_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emp_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emp_Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emp_NationalID = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    DeptName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start_Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    End_Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Vac1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vac2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    overtime = table.Column<int>(type: "int", nullable: false),
                    discount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Emp_Ssn);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Group_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Group_Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Emp_show = table.Column<bool>(type: "bit", nullable: false),
                    Emp_Add = table.Column<bool>(type: "bit", nullable: false),
                    Emp_Edit = table.Column<bool>(type: "bit", nullable: false),
                    Emp_Delete = table.Column<bool>(type: "bit", nullable: false),
                    User_show = table.Column<bool>(type: "bit", nullable: false),
                    User_Add = table.Column<bool>(type: "bit", nullable: false),
                    User_Edit = table.Column<bool>(type: "bit", nullable: false),
                    User_Delete = table.Column<bool>(type: "bit", nullable: false),
                    Group_show = table.Column<bool>(type: "bit", nullable: false),
                    Group_Add = table.Column<bool>(type: "bit", nullable: false),
                    Group_Edit = table.Column<bool>(type: "bit", nullable: false),
                    Group_Delete = table.Column<bool>(type: "bit", nullable: false),
                    Attend_show = table.Column<bool>(type: "bit", nullable: false),
                    Attend_Add = table.Column<bool>(type: "bit", nullable: false),
                    Attend_Edit = table.Column<bool>(type: "bit", nullable: false),
                    Attend_Delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Group_Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendenceEmployees",
                columns: table => new
                {
                    Emp_Ssn = table.Column<int>(type: "int", nullable: false),
                    Att_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendenceEmployees", x => new { x.Att_id, x.Emp_Ssn });
                    table.ForeignKey(
                        name: "FK_AttendenceEmployees_Attendances_Att_id",
                        column: x => x.Att_id,
                        principalTable: "Attendances",
                        principalColumn: "att_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendenceEmployees_Employees_Emp_Ssn",
                        column: x => x.Emp_Ssn,
                        principalTable: "Employees",
                        principalColumn: "Emp_Ssn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Fname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_Id);
                    table.ForeignKey(
                        name: "FK_Users_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Group_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendenceEmployees_Emp_Ssn",
                table: "AttendenceEmployees",
                column: "Emp_Ssn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnualVacations");

            migrationBuilder.DropTable(
                name: "AttendenceEmployees");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
