# 📋 Task Management API

A production-ready RESTful API built with **.NET 9** and **ASP.NET Core**, following **Clean Architecture**, **Domain-Driven Design (DDD)**, **CQRS**, and the **Mediator Pattern**. Implements JWT authentication, role-based authorization, Redis caching, background processing, global exception handling, and Docker support.

---

## 📑 Table of Contents

- [Architecture Overview](#-architecture-overview)
- [Features](#-features)
- [Project Structure](#-project-structure)
- [Prerequisites](#-prerequisites)
- [Running with Docker (Recommended)](#-running-with-docker-recommended)
- [Running Locally (.NET 9)](#-running-locally-net-9)
- [Configuration](#-configuration)
- [Seeded Admin Credentials](#-seeded-admin-credentials)
- [API Endpoints](#-api-endpoints)
- [Authentication & Authorization](#-authentication--authorization)
- [Redis Caching](#-redis-caching)
- [Background Processing](#-background-processing)
- [Business Logic](#-business-logic)
- [Bonus Features](#-bonus-features)
- [Assumptions](#-assumptions)

---

## 🏛️ Architecture Overview

This project follows **Clean Architecture** — dependencies always point inward. The outer layers know about the inner layers, but never the reverse.

```
┌─────────────────────────────────────────────────┐
│         TaskMangement.API  (Presentation)        │  ← Controllers, Middleware, DI
├─────────────────────────────────────────────────┤
│         TaskMangement.Infrastructure             │  ← EF Core, JWT, Redis, Seeders
├─────────────────────────────────────────────────┤
│         TaskMangement.Application                │  ← CQRS Handlers, DTOs, Validators
├─────────────────────────────────────────────────┤
│         TaskMangement.Domain                     │  ← Entities, Enums, Interfaces
├─────────────────────────────────────────────────┤
│         TaskMangemnet.Shared                     │  ← Shared wrappers, constants
└─────────────────────────────────────────────────┘
```

> The **Domain** layer has **zero** external dependencies.

---

## 🧩 Domain-Driven Design (DDD)

The project structure follows a **DDD-style approach** where the domain is the heart of the application and everything else supports it.

**Key DDD concepts applied:**

**Entities** — core objects with a unique identity that persists over time (`User`, `TaskItem`). Business rules and state live on the entity itself, not scattered across services.

**Enums as domain concepts** — `TaskStatus` (Pending / InProgress / Done) and `TaskPriority` (Low / Medium / High) are defined in the Domain layer as first-class domain vocabulary, not magic strings or integers.

**Domain Interfaces** — repositories are defined as abstractions in the Domain/Application layer (`ITaskRepository`, `IUserRepository`). The Infrastructure layer provides the concrete implementations. The domain never knows about EF Core or SQL Server.

**Ubiquitous Language** — naming throughout the codebase (commands, queries, DTOs, endpoints) reflects the language of the business domain: `CreateTask`, `UpdateTaskStatus`, `AssignedTo`, `DueDate`.

**Separation by feature** — the Application layer is organized by feature (`Auth`, `Tasks`, `Admin`) rather than by technical concern, keeping related commands, queries, and DTOs co-located.

```
Domain/
├── Models/          ← Entities (User, TaskItem)
├── Enums/           ← Domain concepts (TaskStatus, TaskPriority)
└── Interfaces/      ← Repository contracts (no EF Core, no SQL)
```

---

## ✨ Features

- ✅ Clean Architecture (Domain → Application → Infrastructure → API)
- ✅ Domain-Driven Design (DDD) — rich domain model, entities, enums, and domain interfaces isolated in the Domain layer
- ✅ CQRS with MediatR (Commands / Queries separated)
- ✅ JWT Authentication & Authorization
- ✅ Role-Based Access Control (Admin / User)
- ✅ Redis Caching (Get Task by ID)
- ✅ Background Processing (`BackgroundService` + in-memory `Channel<T>`)
- ✅ Database Seeding (Admin user + Roles on startup)
- ✅ Global Exception Handling Middleware
- ✅ Global Response Wrapper
- ✅ FluentValidation
- ✅ Docker + Docker Compose support
- ✅ Soft Delete for Users
- ✅ Refresh Token Implementation
- ✅ Clean Logging

---

## 📁 Project Structure

```
TasksMangement/
├── TaskMangement.API/                  # Entry point — controllers, middleware, Program.cs
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── TasksController.cs
│   │   └── AdminController.cs
│   ├── Middlewares/
│   │   └── GlobalExceptionMiddleware.cs
│   ├── Dockerfile
│   └── Program.cs
│
├── TaskMangement.Application/          # Use cases — CQRS handlers, DTOs, validators
│   ├── Features/
│   │   ├── Auth/
│   │   │   ├── Commands/               # Register, Login, RefreshToken
│   │   │   └── Queries/                # GetCurrentUser
│   │   ├── Tasks/
│   │   │   ├── Commands/               # CreateTask, UpdateTaskStatus
│   │   │   └── Queries/                # GetTaskById, GetAllTasks
│   │   └── Admin/
│   │       ├── Commands/               # CreateUser, DeleteUser
│   │       └── Queries/                # GetAllUsers
│   ├── DTOs/
│   ├── Validators/
│   └── Abstractions/
│
├── TaskMangement.Domain/               # Core business entities — no dependencies
│   ├── Models/
│   │   ├── User.cs
│   │   └── TaskItem.cs
│   ├── Enums/
│   │   ├── TaskStatus.cs
│   │   └── TaskPriority.cs
│   └── Interfaces/
│
├── TaskMangement.Infrastructure/       # Frameworks & drivers
│   ├── Persistence/
│   │   ├── Contexts/
│   │   │   └── ApplicationDbContext.cs
│   │   ├── Repositories/
│   │   └── Seeders/
│   │       ├── RoleSeeder.cs
│   │       └── AdminSeeder.cs
│   ├── Authentication/
│   │   └── JwtTokenGenerator.cs
│   ├── Caching/                        # Redis cache service
│   └── BackgroundServices/             # Task processing queue
│
├── TaskMangemnet.Shared/               # Shared models
│   └── Responses/
│       └── ApiResponse.cs
│
├── docker-compose.yml
└── TasksMangement.sln
```

---

## 🔧 Prerequisites

### For Docker (recommended)

| Tool | Version |
|------|---------|
| [Docker Desktop](https://www.docker.com/products/docker-desktop) | Latest |
| [Docker Compose](https://docs.docker.com/compose/) | v2+ (included in Docker Desktop) |

### For local development

| Tool | Version |
|------|---------|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/9.0) | 9.0 |
| [SQL Server](https://www.microsoft.com/en-us/sql-server) | 2019+ or LocalDB |
| [Redis](https://redis.io/download) | 7+ |

---

## 🐳 Running with Docker (Recommended)

This is the easiest way to run the full stack with zero local setup.

### 1. Clone the repository

```bash
git clone https://github.com/mohamedshehatadev17/TasksMangement.git
cd TasksMangement
```

### 2. Start all services

```bash
docker-compose up --build
```

This spins up 3 containers:

| Container | Description | Port |
|-----------|-------------|------|
| **api** | ASP.NET Core API | `5000` |
| **sqlserver** | SQL Server 2022 | `14333` |
| **redis** | Redis 7 | `6379` |

> Database migrations and seeding run automatically on startup. The admin user is created on first boot.

### 3. Access the API

```
Base URL: http://localhost:5000
```

Test endpoints using [APIDog](https://apidog.com) — import the collection and set the base URL environment variable to `http://localhost:5000`.

### 4. Stop containers

```bash
docker-compose down
```

To also remove volumes (wipe the database):

```bash
docker-compose down -v
```

---

## 💻 Running Locally (.NET 9)

Use this when you want to debug in Visual Studio with breakpoints.

### 1. Clone the repository

```bash
git clone https://github.com/mohamedshehatadev17/TasksMangement.git
cd TasksMangement
```

### 2. Start only the infrastructure containers

```bash
docker-compose up sqlserver redis -d
```

### 3. Configure `appsettings.Development.json`

Create or update `TaskMangement.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,14333;Database=db_TaskMangement;User Id=sa;Password=TaskMng@2026!;TrustServerCertificate=True;Encrypt=False;",
    "RedisConnection": "localhost:6379"
  },
  "Jwt": {
    "Key": "your-base64-encoded-secret-key-at-least-32-chars",
    "Issuer": "TaskManagementAPI",
    "Audience": "TaskManagementClient"
  }
}
```

### 4. Apply database migrations

```bash
dotnet ef database update --project TaskMangement.Infrastructure --startup-project TaskMangement.API
```

### 5. Run the API

```bash
dotnet run --project TaskMangement.API
```

```
Base URL: https://localhost:7123
```

---

## ⚙️ Configuration

| Key | Description |
|-----|-------------|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `ConnectionStrings:RedisConnection` | Redis connection string |
| `Jwt:Key` | Base64-encoded secret key (min 32 chars) |
| `Jwt:Issuer` | JWT token issuer |
| `Jwt:Audience` | JWT token audience |

> In Docker, these are injected via environment variables in `docker-compose.yml` and override `appsettings.json` automatically.

---

## 🔑 Seeded Admin Credentials

On first startup, the application seeds a default admin user and roles automatically.

| Field | Value |
|-------|-------|
| **Email** | `admin@anyware.com` |
| **Password** | `Admin@123` |
| **Role** | `Admin` |

Use these credentials to call `POST /api/auth/login`, then use the returned JWT token in the `Authorization` header for all protected requests.

---

## 📡 API Endpoints

### Auth

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `/api/auth/register` | ❌ | Register a new user |
| `POST` | `/api/auth/login` | ❌ | Login and receive JWT token |
| `POST` | `/api/auth/refresh-token` | ❌ | Refresh an expired JWT token |
| `GET` | `/api/auth/profile` | ✅ | Get current user profile |

### Tasks

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `/api/tasks` | ✅ User | Create a new task |
| `GET` | `/api/tasks` | ✅ User | Get all tasks (own tasks, sorted by priority) |
| `GET` | `/api/tasks/{id}` | ✅ User | Get task by ID (Redis cached) |
| `PATCH` | `/api/tasks/{id}/status` | ✅ User | Update task status |

### Admin

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `/api/admin/users` | ✅ Admin | Get all users |
| `POST` | `/api/admin/users` | ✅ Admin | Create a new user |
| `DELETE` | `/api/admin/users/{id}` | ✅ Admin | Soft delete a user |

---

## 🔐 Authentication & Authorization

The API uses **JWT Bearer tokens**.

### How to authenticate with APIDog

1. Call `POST /api/auth/login` with your credentials
2. Copy the `token` from the response
3. In APIDog, set an environment variable: `token = <paste value here>`
4. On protected requests, set the Authorization header:
   ```
   Authorization: Bearer {{token}}
   ```

### Role breakdown

| Role | Capabilities |
|------|-------------|
| `Admin` | Manage users (create, soft delete, list) + all user capabilities |
| `User` | Create tasks, view own tasks, update own task status |

---

## 🗄️ Redis Caching

Redis is used to cache the **Get Task by ID** endpoint.

```
GET /api/tasks/{id}
        │
        ├── Cache HIT?  → Return from Redis ⚡
        │
        └── Cache MISS? → Query DB → Store in Redis → Return
```

When a task is updated via `PATCH /api/tasks/{id}/status`, the cache entry is automatically invalidated so the next request fetches fresh data.

- **Cache key:** `task:{id}`
- **TTL:** 5 minutes

---

## ⚙️ Background Processing

When a task is created, it is saved to the database and enqueued for background processing using a .NET `BackgroundService` with an in-memory `Channel<T>`.

```
POST /api/tasks
        │
        ├── 1. Save task to DB  →  Status: Pending
        │
        └── 2. Enqueue task ID  →  BackgroundService
                    │
                    └── Simulate processing (delay)
                            │
                            └── Status: InProgress → Done
```

No external queue (RabbitMQ, Azure Service Bus) is required — the built-in .NET `Channel<T>` handles the queue in-process.

---

## 🧠 Business Logic

Two business rules are enforced in the Application layer:

**1. Duplicate task prevention**
A user cannot create two tasks with the same title on the same calendar day. Returns `400 Bad Request` with a descriptive message if violated.

**2. Task sorting**
All tasks are returned sorted by **Priority** (High → Medium → Low), then by **CreatedAt** (newest first) within the same priority.

---

## 🎁 Bonus Features

| Bonus | Status |
|-------|--------|
| Global exception handling | ✅ `GlobalExceptionMiddleware` |
| Docker support | ✅ `Dockerfile` + `docker-compose.yml` |
| Clean logging | ✅ Structured logging via `ILogger` |
| Refresh token implementation | ✅ `POST /api/auth/refresh-token` |
| Soft delete for users | ✅ `IsDeleted` flag — data is never hard deleted |
| Unit tests | ❌ Not included |

---

## 📝 Assumptions

- Regular users can only view and update their own tasks; admins have full visibility.
- Soft delete is used for users — deleted users cannot log in, but their data and tasks are preserved.
- The JWT key must be Base64-encoded and at least 32 characters before encoding.
- Background processing uses an in-memory channel. If the API restarts, queued-but-unprocessed tasks remain in `Pending` status and are not automatically retried — acceptable for this scope.
- Refresh tokens are single-use and rotated on each refresh.
