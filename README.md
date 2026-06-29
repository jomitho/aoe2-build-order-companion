# AoE2 Build Order Companion

A small full-stack web application for creating, managing and following Age of Empires II build orders.

The project is built to demonstrate practical experience with:

- ASP.NET Core Web API
- React and TypeScript
- SQL / Entity Framework Core
- automated testing
- Azure deployment
- Infrastructure as Code

## Tech stack

- ASP.NET Core Web API
- C# / .NET
- Entity Framework Core
- SQL Server LocalDB
- React
- TypeScript
- Vite
- REST API

## Current features

- View a list of Age of Empires II build orders
- View detailed build order steps
- Create new build orders from the React client
- Delete build orders
- Persist build orders and steps in SQL Server using Entity Framework Core
- Seed initial build order data on first run

## Local development setup

### Prerequisites

- .NET SDK
- Node.js
- SQL Server LocalDB
- Entity Framework Core CLI

Run the API:
```bash
dotnet run --project src/Aoe2BuildOrders.Api
```

The API runs at:
```bash
http://localhost:5198
```

Swagger is available at:
```bash
http://localhost:5198/swagger
```

Run the React client:
```bash
cd client
npm install
npm run dev
```

The React client runs at:
```bash
http://localhost:5173
```

## Database

The API uses Entity Framework Core with SQL Server LocalDB for local persistence.

On startup, the API applies pending EF Core migrations and seeds the database with initial build orders if the database is empty.

The local connection string is configured in:
```bash
src/Aoe2BuildOrders.Api/appsettings.json
```

To reset the local database:
```bash
dotnet ef database drop --project src/Aoe2BuildOrders.Api
dotnet run --project src/Aoe2BuildOrders.Api
```

## Roadmap

- Add edit functionality for build orders
- Add dynamic create form steps
- Add backend validation improvements
- Add automated tests
- Add GitHub Actions CI
- Deploy API and database to Azure
- Add Azure infrastructure with Bicep
