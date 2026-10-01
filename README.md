# .NET 10 Clean Architecture Template

An ASP.NET Core Web API starter built around the Hybrid Clean Architecture conventions used by the RxiFinTrack `aspnet-clean-architecture-skill`. This repository can also be installed as a `dotnet new` template.

## Requirements

- .NET 10 SDK

## Project structure

```text
src/
  CleanArchitecture.Domain/          # Domain entities, business rules, auditing, and soft delete
  CleanArchitecture.Application/     # IApplicationDbContext, DTOs, services, CQRS, and pagination
  CleanArchitecture.Infrastructure/  # EF Core DbContext, configurations, migrations, and interceptors
  CleanArchitecture.Api/             # Controllers, middleware, API responses, OpenAPI, and Scalar
```

Dependencies point inward: `Application -> Domain` and `Infrastructure -> Application + Domain`. `Api` is the composition root and wires Application to Infrastructure. EF Core is accessed through `IApplicationDbContext`; this template does not use the Repository or Generic Repository pattern.

## Use as a template

Run these commands from the repository root:

```powershell
dotnet new install .
dotnet new ca-hybrid --name MyFinanceApp --output ..\MyFinanceApp
```

Then open the generated `MyFinanceApp` directory and run the application.

## Included examples

- **Banks** — straightforward master data CRUD implemented with an Application Service.
- **Transactions** — business operations organized as MediatR CQRS commands and queries.
- **Pagination** — list endpoints accept `page` (default `1`), `take` (default `20`, maximum `100`), and optional `search`. The shared pagination extension centralizes `Skip` and `Take`.
- **API responses** — endpoints use the `{ "success", "code", "message", "data" }` response shape.
- **EF Core** — read-only queries use `AsNoTracking()` and DTO projections. A `SaveChangesInterceptor` sets audit timestamps, and global query filters implement soft delete.

## Run

Run from the solution root:

```powershell
dotnet restore
dotnet run --project .\src\CleanArchitecture.Api
```

In Development, SQLite creates `cleanarchitecture.db` in the solution root and creates the schema on first run. API documentation is available at `/scalar/v1`; the OpenAPI document is at `/openapi/v1.json`.

## Frontend integration

This project exposes a REST API that can be consumed by a separate frontend. Any client that can send HTTP requests and handle JSON can connect. Examples include **Angular**, **React** or **Next.js**, **Vue**, **Svelte**, and **Blazor WebAssembly**. Mobile clients such as **Flutter** and **React Native** can also call the API. These frontend applications are not included in this repository.

With the API running at `http://localhost:5115`, a frontend can call an endpoint using `fetch`, `Axios`, or Angular `HttpClient`. For example:

```javascript
const response = await fetch("http://localhost:5115/api/banks?page=1&take=20");
const result = await response.json();

if (result.success) {
  console.log(result.data.items);
}
```

When the frontend and API run on different origins or ports, configure CORS on the API to allow the frontend origin. This template does not currently define a CORS policy; configure it in `Program.cs` for your development and production URLs. Use Scalar (`/scalar/v1`) or OpenAPI (`/openapi/v1.json`) to inspect the endpoints and response schemas.

## Build

```powershell
dotnet build .\CleanArchitecture.sln
```

## API examples

Create a bank by sending this JSON to `POST /api/banks`:

```json
{
  "code": "BANK01",
  "name": "Example Bank"
}
```

List banks with `GET /api/banks?page=1&take=20&search=example`. List transactions with `GET /api/transactions?page=1&take=20`.

## Add a feature

- For straightforward master data CRUD, add DTOs, a service interface, and an implementation under `Application/Services/<Feature>`. Access EF Core through `IApplicationDbContext` and keep the controller thin.
- For complex workflows, state transitions, or reports, add commands or queries, handlers, and validators under `Application/Features/<Feature>`. CQRS controllers should only call `ISender.Send()`.
- Keep domain rules in Domain and EF Core configuration and persistence in Infrastructure. Do not return EF entities directly from the API.

## Database migrations

The starter uses `EnsureCreated` for a zero-setup Development database. For production, replace the startup `EnsureCreated` call with migration-based initialization, then run the following commands. Do not use `EnsureCreated` databases together with EF Core migrations.

```powershell
dotnet tool install --global dotnet-ef --version 10.0.12
dotnet ef migrations add InitialCreate --project .\src\CleanArchitecture.Infrastructure --startup-project .\src\CleanArchitecture.Api --output-dir Persistence/Migrations
dotnet ef database update --project .\src\CleanArchitecture.Infrastructure --startup-project .\src\CleanArchitecture.Api
```
