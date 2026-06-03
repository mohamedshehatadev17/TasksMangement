# 📋 Task Management API

A production-ready RESTful API built with **ASP.NET Core** following **Clean Architecture**, **CQRS**, and the **Mediator Pattern**. Includes JWT authentication & authorization, role-based access control, API versioning, global response handling, and global error handling.

---

## 📑 Table of Contents

- [Architecture Overview](#architecture-overview)
- [Features](#features)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Running the Project](#running-the-project)
- [API Versioning](#api-versioning)
- [Authentication & Authorization](#authentication--authorization)
- [Global Response & Error Handling](#global-response--error-handling)
- [Key Patterns Explained](#key-patterns-explained)

---

## 🏛️ Architecture Overview

This project follows **Clean Architecture** — a layered approach that keeps business logic independent from frameworks, databases, and external concerns. Dependencies always point **inward**: the outer layers know about the inner layers, but never the other way around.

```
┌──────────────────────────────────────────────┐
│              Web.Api / Presentation           │  ← Controllers, Middleware, DI Registration
├──────────────────────────────────────────────┤
│                Infrastructure                 │  ← EF Core, JWT, Email, External Services
├──────────────────────────────────────────────┤
│                  Application                  │  ← CQRS Handlers, DTOs, Mapster Profiles, Validators
├──────────────────────────────────────────────┤
│                    Domain                     │  ← Entities, Enums, Domain Interfaces (no dependencies)
└──────────────────────────────────────────────┘
```

> The **Domain** layer has **zero** external dependencies. Everything else depends on it, never the reverse.

---

## ✨ Features

- ✅ Clean Architecture (Domain → Application → Infrastructure → API)
- ✅ CQRS with MediatR (Commands / Queries separated)
- ✅ JWT Authentication & Authorization
- ✅ Role-Based Authorization
- ✅ DTOs with Mapster for object mapping
- ✅ API Versioning (v1, v2, …)
- ✅ Global Response Wrapper
- ✅ Global Error Handling Middleware
- ✅ Docker support

---

## 📁 Project Structure

```
TaskMangement/
├── Domain/                         # Enterprise business rules
│   ├── Entities/                   # Core domain models (Task, User, Role…)
│   ├── Enums/
│   └── Interfaces/                 # IRepository, IGeneric Repository
│
├── Application/                    # Application business rules
│   ├── Features/
│   │   ├── Tasks/
│   │   │   ├── Commands/           # CreateTaskCommand, UpdateTaskCommand…
│   │   │   └── Queries/            # GetTaskByIdQuery, GetAllTasksQuery…
│   │   └── Auth/
│   │       ├── Commands/           # LoginCommand, RegisterCommand…
│   │       └── Queries/
│   ├── DTOs/                       # Request/Response data transfer objects
│   ├── Mapping/                    # Mapster configuration profiles
│   ├── Common/
│   │   └── Responses/              # ApiResponse<T> wrapper
│   └── Validators/                 # FluentValidation rules
│
├── Infrastructure/                 # Frameworks & drivers
│   ├── Persistence/
│   │   ├── AppDbContext.cs
│   │   └── Repositories/
│   ├── Identity/                   # ASP.NET Identity, JWT generation
│   └── DependencyInjection.cs
│
└── Web.Api / TaskMangement.API/    # Entry point
    ├── Controllers/
    ├── Middleware/                  # Global error handler, response wrapper
    └── Program.cs
```

---

## 🔧 Prerequisites

| Tool | Version |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download) | 9.0  
| [SQL Server](https://www.microsoft.com/en-us/sql-server) | 2019+ (or LocalDB for development) |
| [Git](https://git-scm.com/) | Any recent version |

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/mohamedshehatadev17/Task_Mangemnet_API.git
cd Task_Mangemnet_API
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Configure the application

Open `Web.Api/appsettings.json` (or `appsettings.Development.json`) and update the following sections:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Key": "YOUR_SUPER_SECRET_KEY_AT_LEAST_32_CHARS",
    "Issuer": "TaskManagementAPI",
    "Audience": "TaskManagementClient",
    "DurationInDays": 7
  }
}
```

> ⚠️ Never commit real secrets to source control. Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or environment variables in production.

### 4. Apply database migrations

```bash
dotnet ef database update --project Infrastructure --startup-project Web.Api
```

If migrations don't exist yet, create them first:

```bash
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Web.Api
dotnet ef database update --project Infrastructure --startup-project Web.Api
```

### 5. Run the API

```bash
dotnet run --project Web.Api
```

The API will be available at `https://localhost:5001` (or the port shown in the terminal).

Swagger UI is accessible at:

```
https://localhost:5001/swagger
```
---

## ⚙️ Configuration

| Key | Description |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `JwtSettings:Key` | Secret key used to sign JWT tokens (min 32 chars) |
| `JwtSettings:Issuer` | Token issuer name |
| `JwtSettings:Audience` | Token audience name |
| `JwtSettings:DurationInDays` | Token expiry duration |

---

## 🔢 API Versioning

The API uses URL-segment versioning. All routes are prefixed with the version number:

```
/api/v1/projects
/api/v2/projects
```

Controllers declare their version using the `[ApiVersion]` attribute:

```csharp
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProjectsController : ControllerBase { ... }
```

When consuming the API, always include the version in the URL to avoid breaking changes.

---

## 🔐 Authentication & Authorization

### Register a new user

```http
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "Mohamed",
  "lastName": "Shehata",
  "email": "user@example.com",
  "password": "P@ssw0rd!"
}
```

### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "P@ssw0rd!"
}
```

Response returns a **JWT Bearer token**. Use it in all subsequent requests:

```http
Authorization: Bearer <your_token_here>
```

### Role-Based Authorization

Endpoints are protected by roles using the `[Authorize(Roles = "...")]` attribute:

| Role | Description |
|---|---|
| `Admin` | Full access to all resources |
| `User` | Can manage their own tasks |

---

## 🌐 Global Response & Error Handling

### Unified Response Wrapper

Every successful response is wrapped in a standard envelope:

```json
{
  "isSuccess": true,
  "statusCode": 200,
  "message": "Tasks retrieved successfully.",
  "data": [ ... ]
}
```

### Global Error Handling

Unhandled exceptions are caught by a middleware pipeline and returned as structured error responses — no stack traces leak to clients in production:

```json
{
  "isSuccess": false,
  "statusCode": 400,
  "message": "Validation failed.",
  "errors": [
    "Title is required.",
    "Due date cannot be in the past."
  ]
}
```

---

## 🧠 Key Patterns Explained

### Clean Architecture

Clean Architecture divides the codebase into concentric layers. The rule is simple: **source code dependencies can only point inward**.

- **Domain** — your pure business entities and rules. No NuGet packages, no frameworks.
- **Application** — orchestrates the domain to fulfill use cases. Depends only on Domain.
- **Infrastructure** — implements interfaces defined in Application (database, email, JWT). Depends on Application and Domain.
- **Web.Api** — the delivery mechanism. Depends on everything but is known by nothing.

This means you can swap SQL Server for PostgreSQL, or replace JWT with OAuth, by only touching the Infrastructure layer.

---

### CQRS (Command Query Responsibility Segregation)

CQRS separates operations that **change state** (Commands) from operations that **read state** (Queries).

```
User Request
     │
     ├── Write? → Command → CommandHandler → Repository (writes to DB)
     │
     └── Read?  → Query  → QueryHandler  → Repository (reads from DB)
```

**Example Command:**
```csharp
// Carries the intent + data to create a task
public record CreateTaskCommand(string Title, string Description, DateTime DueDate) 
    : IRequest<ApiResponse<TaskDto>>;

// Handles the command — all business logic lives here
public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, ApiResponse<TaskDto>>
{
    public async Task<ApiResponse<TaskDto>> Handle(CreateTaskCommand request, CancellationToken ct)
    {
        // validate → create entity → save → map → return
    }
}
```

**Example Query:**
```csharp
public record GetTaskByIdQuery(Guid Id) : IRequest<ApiResponse<TaskDto>>;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, ApiResponse<TaskDto>>
{
    public async Task<ApiResponse<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken ct)
    {
        // fetch → map → return
    }
}
```

Benefits: handlers are small and focused, easy to unit test, and the read/write models can evolve independently.

---

### Mediator Pattern (MediatR)

The Mediator pattern decouples the sender of a request from its handler. Controllers don't call services directly — they send a message to MediatR, which routes it to the correct handler.

```
Controller  →  _mediator.Send(new GetTaskByIdQuery(id))  →  MediatR  →  GetTaskByIdQueryHandler
```

This keeps controllers thin and removes direct coupling between layers:

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetById(Guid id)
{
    var result = await _mediator.Send(new GetTaskByIdQuery(id));
    return Ok(result);
}
```

---

### Mapster (Object Mapping)

Mapster maps domain entities to DTOs (and back) without manual property assignment. Mapping configuration is centralized in profiles inside the Application layer:

```csharp
// Mapster profile
TypeAdapterConfig<TaskEntity, TaskDto>
    .NewConfig()
    .Map(dest => dest.AssignedTo, src => src.AssignedUser.FullName);

// Usage inside a handler
var dto = entity.Adapt<TaskDto>();
```

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).
