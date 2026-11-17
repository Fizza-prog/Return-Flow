using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ReturnFlow.Data;

#nullable disable

namespace ReturnFlow.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20261117120000_AddInspectionFields")]
    public partial class AddInspectionFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Disposition",
                table: "ReturnRequests",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InspectedAt",
                table: "ReturnRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InspectionFindings",
                table: "ReturnRequests",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCondition",
                table: "ReturnRequests",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Disposition", table: "ReturnRequests");
            migrationBuilder.DropColumn(name: "InspectedAt", table: "ReturnRequests");
            migrationBuilder.DropColumn(name: "InspectionFindings", table: "ReturnRequests");
            migrationBuilder.DropColumn(name: "ProductCondition", table: "ReturnRequests");
        }
    }
}
