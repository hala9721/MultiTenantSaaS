# Multi-Tenant Project Management SaaS

##  Overview
Production-ready SaaS platform for managing projects and tasks across multiple tenants.
Demonstrates enterprise architecture: Clean Architecture, Repository Pattern, 
Multi-Tenancy Isolation, RBAC, Comprehensive Testing.

##  Architecture
- **Clean Architecture**: Entities → Services → Controllers
- **Repository Pattern**: Abstraction for data access
- **Unit of Work**: Transaction management
- **Multi-Tenancy**: TenantId isolation at database level
- **Service Layer**: Business logic separation from controllers
- **DTO Pattern**: Request/Response abstraction

##  Features
-  Multi-tenant isolation (data per tenant)
-  JWT Authentication with refresh tokens
-  Role-Based Access Control (RBAC)
-  Permission-based authorization
-  Project Management (CRUD)
-  Task Management with status workflow
-  User invitations to projects
-  Activity logging
-  Comprehensive input validation
-  Global error handling
-  Structured logging
-  Unit tests (25+ tests)

##  Tech Stack
- .NET 8
- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Authentication
- FluentValidation
- xUnit, Moq

##  Project Structure

src/
├── MultiTenantSaaS.API/ # Controllers, Middleware, Program.cs
├── MultiTenantSaaS.Application/ # Services, Validators, DTOs, Interfaces
├── MultiTenantSaaS.Domain/ # Entities, Enums, Abstractions
└── MultiTenantSaaS.Infrastructure/ # Repositories, DbContext, Migrations


##  Getting Started



### Setup
1. Update connection string in appsettings.json
2. Run migrations: `dotnet ef database update`
3. Start API: `dotnet run`
4. Visit Swagger: https://localhost:5001/swagger

##  API Endpoints

### Auth
- `POST /api/auth/login` - Login user
- `POST /api/auth/refresh` - Refresh token

### Tenants
- `GET /api/tenants/{id}` - Get tenant details
- `POST /api/tenants` - Create tenant (Admin only)

### Projects
- `GET /api/projects` - List tenant projects
- `POST /api/projects` - Create project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Delete project

### Tasks
- `GET /api/tasks` - List tasks
- `POST /api/tasks` - Create task
- `PUT /api/tasks/{id}` - Update task
- `PATCH /api/tasks/{id}/status` - Change task status

### Users
- `GET /api/users` - List users in tenant
- `POST /api/users` - Invite user to tenant
- `PUT /api/users/{id}/roles` - Assign roles

##  Testing
Run tests: `dotnet test`

##  Notes
- All requests require TenantId in JWT token
- All data automatically filtered by TenantId
- Admin role required for tenant-level operations
