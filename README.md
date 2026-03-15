# Employee Management System

A modern backend service demonstrating clean architecture, JWT authentication, role-based access control, and comprehensive logging. Built with .NET 9 and designed for scalability, security, and maintainability.

## Technology Stack

- **.NET 9** | **ASP.NET Core Web API**
- **Databases:** MSSQL (Main), PostgreSQL (Logging)
- **ORM:** Entity Framework Core
- **Authentication:** JWT Bearer Tokens
- **Validation:** FluentValidation
- **Architecture:** Clean Layered Architecture

---

## Project Setup

### Prerequisites

- .NET 9 SDK
- MSSQL Server
- PostgreSQL
- Git

### Installation

```bash
# Clone repository
git clone <repo-url>
cd EmployeeManagementSystem

# Restore dependencies
dotnet restore
```

---

## Database Configuration

### Step 1: Update Connection Strings

Edit `EmployeeManagement.WebAPI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER\\MSSQLSERVER05;Database=EmployeeDb;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False",
    "LogConnection": "Host=localhost;Port=5432;Database=EmployeeLogDb;Username=postgres;Password=postgres;"
  }
}
```

**MSSQL Settings:**
- `Server`: Your MSSQL instance name
- `Database`: EmployeeDb (or your preferred name)
- `Trusted_Connection`: True for Windows Auth

**PostgreSQL Settings:**
- `Host`: PostgreSQL server address
- `Port`: Default 5432
- `Database`: EmployeeLogDb
- `Username/Password`: Your PostgreSQL credentials
  > [SECURITY WARNING] Change the default PostgreSQL password from `postgres` before deploying

### Step 2: Run Migrations

```bash
# Main Database (MSSQL)
dotnet ef database update --context AppDbContext --project EmployeeManagement.Persistence

# Logging Database (PostgreSQL)
dotnet ef database update --context LogDbContext --project EmployeeManagement.Persistence
```

---

## Running the Application

```bash
cd EmployeeManagement.WebAPI
dotnet run
```

**Application starts at:** `http://localhost:5050`

**API Documentation:**
- **Swagger UI:** `http://localhost:5050/swagger`
- **Scalar UI:** `http://localhost:5050/scalar/v1` (Modern alternative)

**Note:** To change port, edit `Properties/launchSettings.json` or use: `dotnet run --urls "http://localhost:YOUR_PORT"`

---

## API Testing

### Quick Start

1. **Get JWT Token:**
   ```http
   POST http://localhost:5050/api/auth/login
   Content-Type: application/json

   {
     "username": "admin",
     "password": "admin123"
   }
   ```

2. **Use Token in Requests:**
   ```http
   Authorization: Bearer <TOKEN_FROM_LOGIN>
   ```

### Test Using REST Client

**VSCode:** Install "REST Client" extension, open `EmployeeManagement.WebAPI.http`

**JetBrains Rider:** Open `EmployeeManagement.WebAPI.http` directly in IDE

**cURL Example:**
```bash
curl -X POST http://localhost:5050/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'
```

### Key Endpoints

| Method | Endpoint | Permission |
|--------|----------|-----------|
| POST | `/api/auth/login` | Public |
| POST | `/api/auth/logout` | Authenticated |
| GET | `/api/employee` | `employees.read` |
| POST | `/api/employee` | `employees.write` |
| GET | `/api/department` | `departments.read` |
| POST | `/api/department` | `departments.write` |
| GET | `/api/salary/history/{id}` | `salary.read` |
| POST | `/api/salary/disburse` | `salary.write` |
| GET | `/api/reporting/total-disbursed` | `reports.view` |

---

## Architecture Overview

```
Domain              → Entities (Employee, Department, etc.)
   ↓
Application         → DTOs, Services, Validators, Business Logic
   ↓
Persistence         → DbContexts, Repositories, UnitOfWork
   ↓
Infrastructure      → Authentication, Logging, Caching
   ↓
WebAPI             → Controllers, Middleware, Attributes
```

---

## Authentication & Authorization

- **JWT Token:** Valid for 60 minutes (configurable)
- **RBAC:** Database-driven User → Role → Permission mapping
- **Each endpoint** protected by `[RequirePermission]` attribute

---

## Default Credentials

```
Username: admin
Password: admin123
```

> [WARNING] Change these credentials immediately in production environments.

---

## Response Format

All responses follow standard format:

```json
{
  "isSuccess": true,
  "statusCode": "200",
  "message": "Operation successful",
  "data": {},
  "errors": []
}
```

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| **Connection string error** | Verify MSSQL/PostgreSQL running & credentials correct |
| **Migration failed** | Ensure databases exist, check connection strings |
| **Port 5050 in use** | Change port in `Properties/launchSettings.json` |
| **JWT expired** | Get new token from login endpoint |

---

## Features

- JWT Authentication & Logout
- Role-Based Access Control (Database-Driven)
- Employee/Department/Designation CRUD
- Salary Disbursement & History
- 4 Report Types
- Comprehensive Audit Logging (PostgreSQL)
- Global Exception Handling
- FluentValidation

---

## Support & Contact

For issues, questions, or feedback, please check the resources below before contacting:

- Troubleshooting section above
- API documentation at `/swagger` or `/scalar/v1`
- Connection strings in `appsettings.json`
- All prerequisites installed (MSSQL, PostgreSQL, .NET 9 SDK)

### Author

**Aminul Islam**  
[a.soton7@gmail.com](mailto:a.soton7@gmail.com)

