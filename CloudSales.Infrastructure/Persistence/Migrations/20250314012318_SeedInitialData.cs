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
                columns: ["Id", "Name", "Description", "PricePerLicense", "CreatedAt", "ExternalId"],
                values: new object[,]
                {
                    { 1, "Microsoft Office", "A suite of productivity applications", 129.99m, now, Guid.NewGuid() },
                    { 2, "Adobe Photoshop", "Image editing and design software", 239.99m, now, Guid.NewGuid() },
                    { 3, "AutoCAD", "Computer-aided design software", 1499.99m, now, Guid.NewGuid() },
                    { 4, "Salesforce CRM", "Customer relationship management software", 99.99m, now, Guid.NewGuid() },
                    { 5, "Slack", "Team collaboration software", 6.67m, now, Guid.NewGuid() },
                    { 6, "GitHub", "Version control and collaboration platform", 4.00m, now, Guid.NewGuid() },
                    { 7, "Jira", "Project management and issue tracking software", 10.00m, now, Guid.NewGuid() },
                    { 8, "Zoom", "Video conferencing and webinar software", 15.00m, now, Guid.NewGuid() },
                    { 9, "Google Drive", "Cloud storage and file sharing service", 1.99m, now, Guid.NewGuid() },
                    { 10, "AWS Cloud Storage", "Cloud storage provided by Amazon Web Services", 0.10m, now, Guid.NewGuid() },
                    { 11, "Dropbox", "Cloud-based file hosting service", 9.99m, now, Guid.NewGuid() },
                    { 12, "Trello", "Project management tool for organizing tasks", 5.00m, now, Guid.NewGuid() },
                    { 13, "Notion", "Workspace for note-taking, task management, and collaboration", 4.00m , now, Guid.NewGuid() },
                    { 14, "Bitbucket", "Git repository management solution", 3.00m , now, Guid.NewGuid() },
                    { 15, "Azure DevOps", "Cloud-based development collaboration tools", 7.00m, now, Guid.NewGuid() },
                    { 16, "Shopify", "E-commerce platform for online stores", 29.00m, now, Guid.NewGuid() },
                    { 17, "QuickBooks", "Accounting software for small businesses", 25.00m, now, Guid.NewGuid() },
                    { 18, "GitLab", "Web-based DevOps platform for software development", 7.00m, now, Guid.NewGuid() },
                    { 19, "Asana", "Work management software for teams", 10.00m, now, Guid.NewGuid() },
                    { 20, "Mailchimp", "Email marketing service and marketing automation", 14.99m, now, Guid.NewGuid()}
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

            var subscriptionId = 1;
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
                            columns: ["Id", "AccountId", "SoftwareServiceId", "Name", "Quantity", "State", "ValidTo", "CreatedAt"],
                            values:
                            [
                            subscriptionId++,
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
                keyColumn: "Id",
                keyValues: [
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                    16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
                    31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45,
                    46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60,
                    61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75
                ]);

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
