# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Project Is

BunkerMVP is a full-stack bunker fuel procurement application. It lets shipping operators create fuel requests, collect supplier quotes, and place orders. The backend is a .NET 8 Web API and the frontend is an Angular 21 SPA.

---

## Running the Application

### Backend

```bash
# Run API on port 5005 (what the frontend services point to)
dotnet run --project src/BunkerMVP.API --urls "http://localhost:5005"

# Default launchSettings runs on 5197 — override with --urls to match frontend config
```

**Connection string** (hardcoded in `DesignTimeDbContextFactory` and `appsettings` via environment):
```
Host=localhost;Database=BunkerMVP;Username=postgres;Password=postgres
```

The database is **auto-migrated on startup** (`db.Database.Migrate()` in `Program.cs`). Seed data is applied via `OnModelCreating` in `BunkerDbContext`.

### EF Core Migrations

```bash
# Add a new migration
dotnet ef migrations add <Name> --project src/BunkerMVP.Infrastructure --startup-project src/BunkerMVP.API --output-dir Data/Migrations

# Apply migrations manually
dotnet ef database update --project src/BunkerMVP.Infrastructure --startup-project src/BunkerMVP.API
```

### Frontend

```bash
cd bunker-frontend
npm install
ng serve          # dev server on http://localhost:4300
ng build          # production build
ng test           # run tests (vitest)
```

---

## Architecture

### Backend Layer Dependency Rules

```
Domain  ←  Application  ←  Infrastructure  ←  API
```

- **Domain** (`BunkerMVP.Domain`): Entities and enums only. No EF Core, no external deps.
- **Application** (`BunkerMVP.Application`): DTOs only. References Domain.
- **Infrastructure** (`BunkerMVP.Infrastructure`): `BunkerDbContext`, all services (`*Service.cs`). References Application. Services live here (not in Application) to avoid a circular dependency with EF Core.
- **API** (`BunkerMVP.API`): Controllers, `Program.cs`, `AuthMiddleware`. References Application and Infrastructure.

### Authentication Flow

Session-based auth (no JWT). `AuthMiddleware` checks `context.Session.GetString("UserId")` on every `/api/*` request, except `/api/auth/*` and `/swagger/*`. The Angular `authInterceptor` catches 401 responses and redirects to `/login`.

Admin credentials: username=`admin`, password=`Admin@123`.

### Database / ORM Notes

- Enums (`RequestStatus`, `OrderStatus`, `ProductUnit`) are stored as **strings** in PostgreSQL via `HasConversion<string>()`.
- `BunkerRequest` has a one-to-one with `BunkerOrder` and one-to-many with `SupplierQuote`.
- `SupplierQuote` → `BunkerRequest` cascades on delete.
- All other foreign keys use `DeleteBehavior.Restrict`.
- The `ETA` property on `BunkerRequestDto` (C# `DateTime ETA`) serializes to JSON as `"eTA"` (not `"eta"`) due to camelCase policy only lowercasing the first character. The Angular model uses `eta` (lowercase). Keep this in mind if adding date display/binding logic.

### Request Status Lifecycle

`Draft` → `Open` → `Quoted` (auto-set when first quote is added) → `Ordered` (auto-set when order is created) → `Cancelled`

### Frontend Structure

```
bunker-frontend/src/app/
  app.config.ts          # provideHttpClient, provideRouter, authInterceptor
  app.routes.ts          # lazy-loaded routes; Shell wraps all protected routes
  core/
    guards/auth.guard.ts
    interceptors/auth.interceptor.ts   # adds withCredentials, handles 401
    models/index.ts                    # all TypeScript interfaces (single file)
    services/                          # one service per domain entity
  features/
    dashboard/dashboard.ts
    requests/
      requests-list/requests-list.ts   # MatTableDataSource with sort/paginator
      request-detail/request-detail.ts # loads request + quotes independently
      create-request-dialog/           # used for both create and edit
      add-quote-dialog/
    auth/login/login.ts
    admin/                             # CRUD pages for vessels/products/locations/suppliers
  layout/
    shell/shell.ts        # wraps authenticated pages (sidebar + router-outlet)
    sidebar/sidebar.ts
    header/header.ts
```

**Component conventions:**
- All component files use `.ts` extension (not `.component.ts`)
- Standalone components throughout — no NgModules
- Use `inject()` for DI in class field initializers when reactive forms are involved (avoids constructor ordering issues)
- Auth state: Angular signals + `localStorage` key `bunker_user`
- All API services hardcode `API_BASE = 'http://localhost:5005/api'` and set `withCredentials: true` (the interceptor also adds it, so it's idempotent)

### CORS

The backend allows `http://localhost:4300` and `http://localhost:52716`. If running the Angular dev server on a different port, add it to the `WithOrigins(...)` call in `Program.cs`.
