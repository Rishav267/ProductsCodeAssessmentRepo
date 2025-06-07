Make sure visual studio is of latest version which suport .NET 9 version
**Download zip file for the code base**

A sample .NET 9 Web API for managing products, featuring CRUD operations, stock management, and robust error handling. Includes unit tests and Swagger/OpenAPI integration for easy endpoint testing.

Features
•	Product CRUD (Create, Read, Update, Delete)
•	Stock increment/decrement with validation
•	Exception-driven error handling
•	AutoMapper for DTO/entity mapping
•	Unit tests with xUnit, Moq, and EF Core InMemory
•	Swagger UI for API exploration

Technologies
•	.NET 9
•	ASP.NET Core Web API
•	Entity Framework Core (SQL Server & InMemory)
•	AutoMapper
•	xUnit, Moq (for testing)
•	Swagger (Swashbuckle)

1. Restore Dependencies:
    dotnet restore

2.	Update the database:
         dotnet ef database update
3. Configure the connection string:
     •	Edit appsettings.json if needed. Default uses LocalDB:
   "ConnectionStrings": {
        "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ProductDb; MultipleActiveResultSets=True;TrustServerCertificate=True;"
      }

   •	Swagger UI: https://localhost:7265/swagger

   Project Structure
     ProductsCodeAssessmentRepo/
          Controllers/
          Domain/
          Models/
          Repository/
          Contract/
          Program.cs
          appsettings.json
      ProductsCodeAssessmentRepo.UnitTests/
          UnitTests/
            ProductRepositoryTests/
              ProductRepositoryTests.cs


    
