
# Product Catalog API

## Overview
This project is a **Product Catalog API** developed using .NET Core. The API provides CRUD operations for managing a catalog of products, including adding, updating, retrieving, and deleting products. It uses Entity Framework Core to interact with a relational database.

## Task Details
**Task Title:** Develop a Basic Product Catalog API  
**Deadline:** 31st December 2024  

### Assigned Developers
- **Developer 1 (Backend API Developer):** Rushikesh Katkar  
  Responsibilities:
  - API Endpoints Implementation
  - Controller Logic
  - Unit Testing
  - API Documentation  

- **Developer 2 (Database & Integration Developer):** Abhishek Yadav  
  Responsibilities:
  - Database Design and Setup
  - Data Validation
  - Database Operations
  - Testing Database Integration  

## API Endpoints
### Base URL: `/api/products`

| Method | Endpoint          | Description                       |
|--------|-------------------|-----------------------------------|
| POST   | `/`               | Add a new product                |
| GET    | `/`               | Retrieve all products            |
| GET    | `/{id}`           | Retrieve a specific product       |
| PUT    | `/{id}`           | Update product details           |
| DELETE | `/{id}`           | Delete a product                 |

## Features
1. CRUD operations for managing products.
2. Validation for input data:
   - Price must be positive.
   - Stock quantity must be non-negative.
3. Detailed API documentation included.
4. Unit tests for API endpoints and database interactions.
5. SQLite or SQL Server for data storage.

## Technologies Used
- .NET Core Web API
- Entity Framework Core
- SQLite / SQL Server
- xUnit for unit testing

## Project Structure
```
ProductCatalogAPI/
├── Controllers/
│   └── ProductController.cs
├── Models/
│   └── Product.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Tests/
│   └── ProductApiTests.cs
├── Program.cs
├── appsettings.json
└── README.md
```

## How to Run
1. **Clone the Repository:**
   ```bash
   git clone https://github.com/YourGitHubUsername/ProductCatalogAPI.git
   cd ProductCatalogAPI
   ```

2. **Set Up Database:**
   - Update the `appsettings.json` file with your database connection string.
   - Run migrations to create the database:
     ```bash
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```

3. **Run the API:**
   ```bash
   dotnet run
   ```

4. **Access the API:**
   - Use Postman or a similar tool to test the endpoints.

## Sample JSON Payloads
### POST /api/products
```json
{
  "name": "Sample Product",
  "category": "Electronics",
  "description": "A sample product description.",
  "price": 100.0,
  "stockQuantity": 50
}
```

### PUT /api/products/{id}
```json
{
  "name": "Updated Product Name",
  "category": "Updated Category",
  "description": "Updated description.",
  "price": 150.0,
  "stockQuantity": 30
}
```

## Testing
- Unit tests are included in the `/Tests` folder.
- Run tests with:
  ```bash
  dotnet test
  ```

## Contributors
- **Rushikesh Katkar:** Backend API Developer
- **Abhishek Yadav:** Database & Integration Developer
