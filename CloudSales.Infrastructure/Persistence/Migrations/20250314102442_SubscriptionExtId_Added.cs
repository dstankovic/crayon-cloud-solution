using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SubscriptionExtId_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "Subscriptions",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Subscriptions");
        }
    }
}
