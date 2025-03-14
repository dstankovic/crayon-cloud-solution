using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CloudSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Additional_Properties_Subscription_And_Service : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "SoftwareServices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_AccountId_SoftwareServiceId_State",
                table: "Subscriptions",
                columns: new[] { "AccountId", "SoftwareServiceId", "State" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareServices_ExternalId",
                table: "SoftwareServices",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_AccountId_SoftwareServiceId_State",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_SoftwareServices_ExternalId",
                table: "SoftwareServices");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "SoftwareServices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions",
                columns: new[] { "AccountId", "SoftwareServiceId" });
        }
    }
}
