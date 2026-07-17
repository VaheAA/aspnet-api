# GameStore.Api

> **Learning project.** This is a small ASP.NET Core minimal API built to practice modern C#/.NET 10 patterns — minimal APIs, EF Core with SQLite, xUnit integration testing, and Docker. It's not intended for production use.
>
> Based on the [ASP.NET Core Full Course For Beginners (.NET 10)](https://www.youtube.com/watch?v=YbRe4iIVYJk&list=PLdLcewOZX20SlVudzCzU5xyzwag2umsrF&index=71&t=12320s) tutorial by Julio Casal.

A minimal REST API for managing a catalog of games and their genres.

## Tech stack

- **.NET 10** minimal APIs
- **EF Core 10** with **SQLite**
- **Swashbuckle** (Swagger UI) for API exploration
- **xUnit** + `WebApplicationFactory` for integration testing
- **Docker** / Docker Compose

## Project structure

```
GameStore.Api.csproj      # main project
Program.cs                 # app composition root
Data/                      # DbContext, seeding, migration helpers
Models/                    # EF entities
Dtos/                      # request/response records
Endpoints/                 # minimal API endpoint groups (one file per resource)
GameStore.Api.Tests/        # xUnit integration tests
```

## Running locally

Requires the .NET 10 SDK.

```sh
dotnet run
```

The API listens on `http://localhost:5100` (see `Properties/launchSettings.json`). On startup it applies EF Core migrations and seeds a handful of genres automatically.

Swagger UI is available at `/swagger` when running in the `Development` environment (the default for `dotnet run`).

## Running with Docker

```sh
docker compose up -d
```

This builds the image, starts the API on `http://localhost:8080`, and persists the SQLite database in a named Docker volume (`gamestore-data`) so data survives container restarts/rebuilds. Swagger is reachable at `http://localhost:8080/swagger` since compose sets `ASPNETCORE_ENVIRONMENT=Development` for local convenience.

```sh
docker compose down      # stop and remove the container
docker compose down -v   # also wipe the persisted database
```

## Running tests

```sh
dotnet test
```

`GameStore.Api.Tests` spins up the real app in-memory via `WebApplicationFactory<Program>` against an isolated, throwaway SQLite file per test run — it doesn't touch your local `GameStore.db`.

## API

| Method | Route         | Description                  |
|--------|---------------|-------------------------------|
| GET    | `/games`      | List all games                |
| GET    | `/games/{id}` | Get a single game by id        |
| POST   | `/games`      | Create a game                  |
| PUT    | `/games/{id}` | Update a game                  |
| DELETE | `/games/{id}` | Delete a game                  |
| GET    | `/genres`     | List all genres                |

Full request/response schemas are available via Swagger once the app is running.
