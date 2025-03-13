using CloudSales.Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //This migration is just for the sake of presentation - not necessary for prod scenario

            var now = DateTime.UtcNow;

            migrationBuilder.InsertData(
                table: "SoftwareServices",
                columns: ["Id", "Name", "Description", "PricePerLicense", "CreatedAt"],
                values: new object[,]
                {
                    { 1, "Microsoft Office", "A suite of productivity applications", 129.99m, now },
                    { 2, "Adobe Photoshop", "Image editing and design software", 239.99m, now },
                    { 3, "AutoCAD", "Computer-aided design software", 1499.99m, now },
                    { 4, "Salesforce CRM", "Customer relationship management software", 99.99m, now },
                    { 5, "Slack", "Team collaboration software", 6.67m, now },
                    { 6, "GitHub", "Version control and collaboration platform", 4.00m, now },
                    { 7, "Jira", "Project management and issue tracking software", 10.00m, now },
                    { 8, "Zoom", "Video conferencing and webinar software", 15.00m, now },
                    { 9, "Google Drive", "Cloud storage and file sharing service", 1.99m, now },
                    { 10, "AWS Cloud Storage", "Cloud storage provided by Amazon Web Services", 0.10m, now },
                    { 11, "Dropbox", "Cloud-based file hosting service", 9.99m, now },
                    { 12, "Trello", "Project management tool for organizing tasks", 5.00m, now },
                    { 13, "Notion", "Workspace for note-taking, task management, and collaboration", 4.00m , now },
                    { 14, "Bitbucket", "Git repository management solution", 3.00m , now },
                    { 15, "Azure DevOps", "Cloud-based development collaboration tools", 7.00m, now },
                    { 16, "Shopify", "E-commerce platform for online stores", 29.00m, now },
                    { 17, "QuickBooks", "Accounting software for small businesses", 25.00m, now },
                    { 18, "GitLab", "Web-based DevOps platform for software development", 7.00m, now },
                    { 19, "Asana", "Work management software for teams", 10.00m, now },
                    { 20, "Mailchimp", "Email marketing service and marketing automation", 14.99m, now}
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: ["Id", "Name", "Email", "PhoneNumber", "CreatedAt"],
                values: new object[,]
                {
                    { 1, "Customer 1", "customer1@example.com", "123-456-7891", now },
                    { 2, "Customer 2", "customer2@example.com", "123-456-7892", now },
                    { 3, "Customer 3", "customer3@example.com", "123-456-7893", now },
                    { 4, "Customer 4", "customer4@example.com", "123-456-7894", now },
                    { 5, "Customer 5", "customer5@example.com", "123-456-7895", now }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: ["Id", "Username", "Email", "FirstName", "LastName", "Type", "CustomerId", "CreatedAt"],
                values: new object[,]
                {
                    { 1, "User1", "user1@example.com", "FirstName1", "LastName1", (int)UserType.Admin, 1, now },
                    { 2, "User2", "user2@example.com", "FirstName2", "LastName2", (int)UserType.Admin, 2, now },
                    { 3, "User3", "user3@example.com", "FirstName3", "LastName3", (int)UserType.Admin, 3, now },
                    { 4, "User4", "user4@example.com", "FirstName4", "LastName4", (int)UserType.Admin, 4, now },
                    { 5, "User5", "user5@example.com", "FirstName5", "LastName5", (int)UserType.Admin, 5, now }
                });

            for (int customerId = 1; customerId <= 5; customerId++)
            {
                for (int accountId = 1; accountId <= 5; accountId++)
                {
                    migrationBuilder.InsertData(
                        table: "Accounts",
                        columns: ["Id", "Name", "Description", "CustomerId", "CreatedAt"],
                        values: [(customerId - 1) * 5 + accountId, $"Account {accountId} for Customer {customerId}", $"Description for Account {accountId} for Customer {customerId}", customerId, now]
                    );

                    // Seeding Subscriptions data (3 subscriptions per account)
                    for (int softwareServiceId = 1; softwareServiceId <= 3; softwareServiceId++)
                    {
                        migrationBuilder.InsertData(
                            table: "Subscriptions",
                            columns: ["AccountId", "SoftwareServiceId", "Name", "Quantity", "State", "ValidTo", "CreatedAt"],
                            values:
                            [
                            (customerId - 1) * 5 + accountId,  // AccountId
                            softwareServiceId,  // SoftwareServiceId
                            $"Subscription {softwareServiceId} for Account {accountId}",
                            3,
                            (int)SubscriptionState.Active,
                            DateTime.UtcNow.AddMonths(12),
                            now
                            ]);
                    }
                }
            }

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
            table: "Subscriptions",
            keyColumns: ["AccountId", "SoftwareServiceId"],
            keyColumnTypes: ["int", "int"],
            keyValues: new object[,] {
                { 1, 1 }, { 1, 2 }, { 1, 3 },
                { 2, 1 }, { 2, 2 }, { 2, 3 },
                { 3, 1 }, { 3, 2 }, { 3, 3 },
                { 4, 1 }, { 4, 2 }, { 4, 3 },
                { 5, 1 }, { 5, 2 }, { 5, 3 },
                { 6, 1 }, { 6, 2 }, { 6, 3 },
                { 7, 1 }, { 7, 2 }, { 7, 3 },
                { 8, 1 }, { 8, 2 }, { 8, 3 },
                { 9, 1 }, { 9, 2 }, { 9, 3 },
                { 10, 1 }, { 10, 2 }, { 10, 3 },
                { 11, 1 }, { 11, 2 }, { 11, 3 },
                { 12, 1 }, { 12, 2 }, { 12, 3 },
                { 13, 1 }, { 13, 2 }, { 13, 3 },
                { 14, 1 }, { 14, 2 }, { 14, 3 },
                { 15, 1 }, { 15, 2 }, { 15, 3 },
                { 16, 1 }, { 16, 2 }, { 16, 3 },
                { 17, 1 }, { 17, 2 }, { 17, 3 },
                { 18, 1 }, { 18, 2 }, { 18, 3 },
                { 19, 1 }, { 19, 2 }, { 19, 3 },
                { 20, 1 }, { 20, 2 }, { 20, 3 },
                { 21, 1 }, { 21, 2 }, { 21, 3 },
                { 22, 1 }, { 22, 2 }, { 22, 3 },
                { 23, 1 }, { 23, 2 }, { 23, 3 },
                { 24, 1 }, { 24, 2 }, { 24, 3 },
            });

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValues: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValues: [1, 2, 3, 4, 5]);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValues: [1, 2, 3, 4, 5]);

            migrationBuilder.DeleteData(
                table: "SoftwareServices",
                keyColumn: "Id",
                keyValues: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20]);
        }
    }
}
