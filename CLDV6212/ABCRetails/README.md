# ABC Retails (ASP.NET Core MVC + Azure Storage)

- Tables: Customers, Products, Orders
- Blob: productimages container for product images
- Queue: orders-queue to signal new orders
- File Share: contracts to store uploaded payment proofs

## Run locally
1. Install .NET 8 SDK.
2. Start Azurite (or set a real Azure Storage connection string in `appsettings.json`).
3. `dotnet run` from the project folder.

## Publish
Use Azure App Service for compute, and Azure Storage for data. Set `AzureStorage:ConnectionString` as App Setting.