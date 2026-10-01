# Clean Architecture (.NET 10)

ASP.NET Core solution scaffold using the Hybrid Clean Architecture conventions from the RxiFinTrack `aspnet-clean-architecture-skill`.

This repository is also a local `dotnet new` template (`ca-hybrid`). Install it with `dotnet new install .`, then create a renamed solution with `dotnet new ca-hybrid --name MyFinanceApp --output ..\MyFinanceApp`.

## Structure

```text
src/
  CleanArchitecture.Domain/          # Entities, domain rules, audit and soft-delete contracts
  CleanArchitecture.Application/     # IApplicationDbContext, DTOs, pagination, services, CQRS
  CleanArchitecture.Infrastructure/  # EF Core DbContext, configurations, migrations, persistence DI
  CleanArchitecture.Api/              # Controllers, middleware, API contracts, OpenAPI and Scalar
```

Dependencies point inward: Domain has no project references; Application references Domain; Infrastructure references Application and Domain; Api is the composition root and references Application and Infrastructure to wire the database.

The `Banks` sample demonstrates simple master-data CRUD through an Application Service. The `Transactions` sample demonstrates CQRS for business operations using MediatR. Both use DTOs and the common `{ success, code, message, data }` response shape. List endpoints use `QueryParams` (`page` defaults to 1, `take` defaults to 20 and is capped at 100) and the shared pagination extension. Read queries use `AsNoTracking()` and project to response DTOs. Soft deletes are applied through EF Core global query filters; audit timestamps are assigned in the DbContext save pipeline.

## Requirements

- .NET 10 SDK

## Run

```powershell
dotnet restore
dotnet run --project .\src\CleanArchitecture.Api
```

Development starts with a SQLite database at `cleanarchitecture.db` in the solution root and creates the schema on first run. API docs are available at `/scalar/v1` and the OpenAPI document at `/openapi/v1.json`.

## Build

```powershell
dotnet build .\CleanArchitecture.sln
```

## Database migrations

The starter uses `EnsureCreated` for a zero-setup development database. Before production use, switch startup initialization to EF Core migrations and create the initial migration, for example:

```powershell
dotnet ef migrations add InitialCreate --project .\src\CleanArchitecture.Infrastructure --startup-project .\src\CleanArchitecture.Api --output-dir Persistence/Migrations
dotnet ef database update --project .\src\CleanArchitecture.Infrastructure --startup-project .\src\CleanArchitecture.Api
```

Install the matching EF CLI tool once with `dotnet tool install --global dotnet-ef --version 10.0.12` if it is not already available.

Do not combine `EnsureCreated` databases with migrations; recreate the development database after switching.

## Add a feature

- For straightforward reference/master data, add DTOs, an interface and implementation under `Application/Services/<Feature>`; depend on `IApplicationDbContext` and keep its controller thin.
- For workflows, state transitions, or reports, add request/handler/validator files under `Application/Features/<Feature>` and have the controller call `ISender.Send()`.
- Put entity invariants in Domain, EF configuration in Infrastructure, and keep API request/response contracts separate from EF entities.

