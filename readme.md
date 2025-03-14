# Cloud Sales Application (.NET 8)

This is a **Cloud Sales** application implemented in **.NET 8**, created for **interview purposes**. The application demonstrates a secure and modular API design integrated with PostgreSQL and background service synchronization.

## 🚀 Features

- Built with **.NET 8 Web API**.
- Uses **PostgreSQL** as a relational database.
- Includes **mock tokens** for UserId and CustomerId to simulate secured API access without implementing a full Identity service.
- Periodic synchronization using ServiceSyncService to keep local services up-to-date with external **CCP services**.

## ⚙️ Configuration

Before running the application, please adjust the **PostgreSQL connection string** in the appropriate environment configuration file:

`  appsettings.Development.json  `

Example snippet:

`  "ConnectionStrings": {    "DefaultConnection": "Host=localhost;Port=5432;Database=your_db;Username=your_user;Password=your_password"  }  `

Make sure your PostgreSQL instance is running and accessible using the provided connection string.

## 🔐 Security Notes

- The API is secured using **mock tokens** containing UserId and CustomerId claims.
- This approach allows you to interact with the secured endpoints **without a full-fledged Identity service**, which was out of scope for this project.
- The mock token can be used for a User who has an Id of 1, and a Customer who also has ID of 1. It's present in mockToken.txt file.

## 🔄 Service Synchronization

The ServiceSyncService is a hosted background service that runs **every 30 minutes** to synchronize service records between the **CCP system** and the local database.

- No manual intervention is required — the sync runs automatically on schedule.
- This helps ensure that the local copy of service data is consistent with external sources.

## 💻 Running the Application

1.  git clone [https://github.com/dstankovic/crayon-cloud-solution](https://github.com/dstankovic/crayon-cloud-solution)
2.  **Configure the connection string** in appsettings.Development.json.
3.  dotnet run

## ✅ Notes

- This project is intended for demonstration and interview purposes only.
- Feel free to extend or adapt the authentication mechanism for production use.
- Ensure PostgreSQL is set up and running before starting the API.
