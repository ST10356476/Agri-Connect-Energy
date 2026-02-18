# Agri-Energy Connect - Technical README

## Table of Contents
1. [Introduction](#introduction)
2. [System Requirements](#system-requirements)
3. [Development Environment Setup](#development-environment-setup)
4. [Database Setup](#database-setup)
5. [Building and Running the Application](#building-and-running-the-application)
6. [System Architecture](#system-architecture)
7. [User Roles and Functionalities](#user-roles-and-functionalities)
8. [Testing](#testing)
9. [Troubleshooting](#troubleshooting)

## Introduction

Agri-Energy Connect is a web application built using ASP.NET Core MVC and Microsoft SQL Server, designed to connect farmers with green energy solutions in South Africa. The platform facilitates collaboration, resource sharing, and knowledge exchange between the agricultural sector and renewable energy providers.

This prototype demonstrates the core functionality of the platform, focusing on farmer and employee user roles, product management, and basic marketplace features.

## System Requirements

### Development Environment
- Visual Studio 2022 (v17.0+) or Visual Studio Code
- .NET 7.0 SDK or later
- Microsoft SQL Server 2019 or later (Express Edition is sufficient for development)
- SQL Server Management Studio (SSMS) or Azure Data Studio
- Git for version control

### Production Environment (Recommended)
- .NET 7.0 Runtime
- SQL Server 2019 or later

## Development Environment Setup

### Step 1: Install Required Software
1. Install .NET 7.0 SDK from [https://dotnet.microsoft.com/download/dotnet/7.0](https://dotnet.microsoft.com/download/dotnet/7.0)
2. Install Visual Studio 2022 (Community Edition is free) from [https://visualstudio.microsoft.com/](https://visualstudio.microsoft.com/)
   - During installation, select the "ASP.NET and web development" workload
3. Install SQL Server 2019 Express from [https://www.microsoft.com/en-us/sql-server/sql-server-downloads](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
4. Install SQL Server Management Studio from [https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

### Step 2: Clone the Repository
```bash
git clone https://github.com/your-organization/agri-energy-connect.git
cd agri-energy-connect
```

### Step 3: Install Required .NET Tools
```bash
dotnet tool install --global dotnet-ef
```

## Database Setup

### Step 1: Configure SQL Server
1. Open SQL Server Management Studio and connect to your local SQL Server instance
2. Create a new database named `AgriEnergyConnect`
3. Ensure your SQL Server authentication mode is set to "SQL Server and Windows Authentication mode"
4. Create a SQL Server login for the application if not using Windows authentication

### Step 2: Configure Connection String
1. Open the project in Visual Studio
2. Locate the `appsettings.json` file in the root directory
3. Update the connection string to match your SQL Server configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=AgriEnergyConnect;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  // Other settings...
}
```

If using SQL authentication instead of Windows authentication:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=AgriEnergyConnect;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;MultipleActiveResultSets=true"
  },
  // Other settings...
}
```

### Step 3: Apply Database Migrations
From the project root directory, run:

```bash
dotnet ef database update
```

This will create the database schema according to the entity models.

### Step 4: Seed Initial Data (Optional)
The application includes a data seeder that will populate the database with sample data for demonstration purposes. To run the seeder:

```bash
dotnet run seeddata
```

## Building and Running the Application

### Using Visual Studio
1. Open the solution file `AgriEnergyConnect.sln` in Visual Studio
2. Restore NuGet packages:
   - Right-click on the solution in Solution Explorer
   - Select "Restore NuGet Packages"
3. Build the solution:
   - Press Ctrl+Shift+B or select Build > Build Solution
4. Run the application:
   - Press F5 or click the "Start" button
   - The application will launch in your default browser

### Using Command Line
1. Navigate to the project directory
2. Restore packages:
   ```bash
   dotnet restore
   ```
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run
   ```
5. Open a browser and navigate to `https://localhost:5001` or `http://localhost:5000`

## System Architecture

Agri-Energy Connect follows the Model-View-Controller (MVC) architectural pattern:

### Model Layer
- Located in the `Models` directory
- Contains entity classes that map to database tables
- Includes data transfer objects (DTOs) for API operations
- Implements data validation using DataAnnotations

### View Layer
- Located in the `Views` directory
- Razor views (.cshtml files) for rendering UI
- Organized by controller (e.g., `Views/Farmers`, `Views/Products`)
- Uses Bootstrap 5 for responsive design

### Controller Layer
- Located in the `Controllers` directory
- Handles HTTP requests and user interactions
- Coordinates between Model and View layers
- Implements role-based access control

### Additional Components
- `Services`: Contains business logic and data processing services
- `Data`: Includes database context and migration configurations
- `Utilities`: Helper classes and extension methods
- `wwwroot`: Static files (CSS, JavaScript, images)

## User Roles and Functionalities

### Farmer Role
Users with the Farmer role can:

1. **Manage Profile**
   - Create and edit personal farm profile
   - Update contact information and farm details
   - Specify sustainability practices and energy needs

2. **Manage Products**
   - Add new products with the following details:
     - Product name
     - Category
     - Description
     - Production date
     - Quantity and unit of measure
     - Sustainability features
   - Edit existing product information
   - Upload product images
   - View their own product listings

3. **Access Resources**
   - Browse educational content
   - View green energy solutions
   - Participate in community forums (future feature)

### Employee Role
Users with the Employee role can:

1. **Manage Farmers**
   - Add new farmer profiles with:
     - Personal details
     - Farm information
     - Contact details
     - Account credentials
   - Edit farmer information
   - View farmer activity and participation

2. **Manage Products**
   - View all products from all farmers
   - Filter products by:
     - Date range
     - Product type
     - Farmer
     - Location
     - Sustainability features
   - Generate reports on product data
   - Verify product information

3. **System Administration**
   - Moderate content
   - Manage user accounts
   - Access system analytics
   - Configure platform settings

## API Documentation

The application exposes a set of RESTful APIs for integration with external systems. API endpoints follow this structure:

```
/api/[controller]/[action]
```

### Authentication API
- `POST /api/auth/login`: Authenticate user
- `POST /api/auth/logout`: End user session
- `POST /api/auth/register`: Register new user (admin only)

### Farmers API
- `GET /api/farmers`: Get all farmers
- `GET /api/farmers/{id}`: Get farmer by ID
- `POST /api/farmers`: Create new farmer
- `PUT /api/farmers/{id}`: Update farmer
- `DELETE /api/farmers/{id}`: Delete farmer (admin only)

### Products API
- `GET /api/products`: Get all products
- `GET /api/products/{id}`: Get product by ID
- `GET /api/products/farmer/{farmerId}`: Get products by farmer
- `POST /api/products`: Create new product
- `PUT /api/products/{id}`: Update product
- `DELETE /api/products/{id}`: Delete product

API documentation can be accessed through Swagger UI at `/swagger` when running in development mode.

## Testing

### Running Automated Tests
The solution includes unit and integration tests. To run all tests:

```bash
dotnet test
```

To run a specific test project:

```bash
dotnet test ./tests/AgriEnergyConnect.Tests.Unit/AgriEnergyConnect.Tests.Unit.csproj
```

## Troubleshooting

### Common Issues and Solutions

**Database Connection Errors:**
- Verify your connection string in `appsettings.json`
- Ensure SQL Server is running
- Check firewall settings if connecting to a remote database

**Missing Dependencies:**
- Run `dotnet restore` to ensure all packages are installed
- Check for package version conflicts in `.csproj` files

**Authentication Problems:**
- Clear browser cookies and cache
- Verify the user exists in the database
- Check role assignments in the AspNetUserRoles table

**404 Not Found Errors:**
- Verify route configuration in `Startup.cs`
- Check controller and action method names
- Ensure view files exist in the correct directory

**For additional assistance:**
- Review application logs in the `logs` directory
- Check the SQL Server error log
- Consult the project documentation wiki



### Running in Docker
- Make sure you have Docker Desktop
- To run:
- Build the image and the container
```bash
docker compose build --no-cache
```
- Run the image and Container up
```bash
docker compose up
```
- Then open docker desktop and click on the image that is running.
- Click on the local host port.
