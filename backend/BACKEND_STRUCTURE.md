# Backend Structure Detail - LoginBackend

This document provides a comprehensive overview of the `LoginBackend` project, an ASP.NET Core Web API designed for JWT and Session-based authentication.

## 🚀 Overview

The backend is built with **ASP.NET Core 8.0**, using **Entity Framework Core** with **PostgreSQL** for data persistence. It implements a layered architecture to separate concerns between API controllers, business logic, and data access.

---

## 📁 Directory Structure

```text
backend/LoginBackend/
├── Controllers/         # API Endpoints
├── Services/            # Business Logic (Auth, Session management)
├── Models/              # Data structures
│   ├── Entities/        # Database tables (EF Core)
│   ├── Request/         # Data Transfer Objects for incoming data
│   └── Response/        # Data Transfer Objects for outgoing data
├── Data/                # Database Context & Migrations
├── Middleware/          # Custom request handling logic
├── appsettings.json     # Configuration (DB, JWT, Session)
└── Program.cs           # Application entry point & service wiring
```

---

## 🛠️ Core Components

### 1. Entry Point: `Program.cs`
This file is the "brain" of the application. It configures:
- **Dependency Injection**: Registers `IAuthService` with `AuthService`.
- **Database Connection**: Configures Npgsql for PostgreSQL using the `DefaultConnection` string.
- **Authentication (JWT)**: Sets up `JwtBearer` authentication with validation for Issuer, Audience, and Secret Key.
- **Session Management**: Configured with a 30-minute timeout and essential cookies for cross-origin support.
- **CORS**: An "AllowAll" policy is implemented to allow communication from any origin (ideal for local development).
- **Middleware Pipeline**: Defines the order of execution: CORS -> Session -> Authentication -> Authorization.

### 2. Controllers: `Controllers/AuthController.cs`
Handles incoming HTTP requests. Key endpoints:
- `POST /api/auth/signup`: Registers a new user with hashed passwords.
- `POST /api/auth/login`: Authenticates user, generates JWT, and initializes a secure session.
- `GET /api/auth/verify`: A protected endpoint (`[Authorize]`) that verifies if both the JWT and Session cookie are still valid.
- `GET /api/auth/health`: Checks database connectivity.

### 3. Services: `Services/AuthService.cs`
Encapsulates the core business logic:
- **Password Hashing**: Uses `BCrypt.Net` for secure password storage.
- **JWT Generation**: Creates tokens containing claims (Username, JTI) with a configurable expiry.
- **Database Interaction**: Uses `ApplicationDbContext` to query and save users.

### 4. Data Layer: `Data/ApplicationDbContext.cs`
The bridge between C# and PostgreSQL.
- **Fluent API**: Maps the `User` entity to the `users` table and handle column name mapping (e.g., `Username` -> `name`).
- **DbSet**: Exposes `Users` for CRUD operations.

### 5. Models: `Models/`
- **Entities/User.cs**: Represents the user record in the database.
- **Request/LoginRequest.cs**: Schema for login data (Username, Password).
- **Response/LoginResponse.cs**: Schema for successful login, containing the JWT and session info.

---

## 🔐 Security Features

- **JWT (JSON Web Tokens)**: Used for stateless authentication.
- **Secure Sessions**: Used for stateful tracking; cookies are `HttpOnly` and `SameSite=None` to prevent CSRF and allow cross-origin requests.
- **BCrypt**: Industry-standard hashing for user passwords.
- **Protected Endpoints**: Using the `[Authorize]` attribute to enforce valid tokens.

---

## ⚙️ Configuration (`appsettings.json`)

Contains critical settings:
- **ConnectionStrings**: Database credentials.
- **Jwt**: Issuer, Audience, Secret Key, and Expiry minutes.
- **Logging**: Console and Debug log levels.
