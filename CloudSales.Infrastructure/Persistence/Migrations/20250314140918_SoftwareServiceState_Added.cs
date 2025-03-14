using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SoftwareServiceState_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "SoftwareServices",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "SoftwareServices");
        }
    }
}
