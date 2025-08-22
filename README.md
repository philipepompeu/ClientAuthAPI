# ClientAuthAPI Study Project

This repository contains a small study project built while exploring the .NET 8 ecosystem.  
It’s **not meant to be production‑ready**, but rather a sandbox to practice modern backend development in C# and learn how the pieces fit together.

## What you’ll find here

- **Minimal API**: instead of using traditional controllers, the HTTP endpoints are defined directly in `Program.cs` and extension methods. This makes the API lightweight and easy to read.

- **Clean-ish Architecture**: the code is split into `Domain`, `Application`, `Infrastructure` (with separate projects for Mongo and JWT), and `API`.

    - The **Domain** project holds the core business entities and repository interfaces.
    - The **Application** project orchestrates use cases via services and defines contracts for things like token generation.
    - The **Infrastructure.Mongo** project implements repositories with the official MongoDB driver and contains Mongo documents/mappers.
    - The **Infrastructure.Security** project implements token generation and password hashing using `System.IdentityModel.Tokens.Jwt`.
    - The **API** project wires everything up via DI and exposes endpoints.

- **MongoDB integration**: a basic repository pattern over `IMongoCollection<T>` is used. Documents and domain entities are kept separate to respect the boundaries of Clean Architecture.

- **JWT authentication**: users log in with username/password and receive a bearer token signed with HMAC‑SHA256. Token claims include the `client_id` to support multi‑tenant scenarios.

- **Configuration with `IOptions`**: settings such as `JwtSettings` and `MongoSettings` are bound from configuration and injected via `IOptions<T>`, avoiding magic strings in the code.

- **Unit tests**: the `test/ClientAuthAPI.Application.Tests` project uses xUnit, Moq and FluentAssertions to test services in isolation.

- **Dockerization**: a multi‑stage `Dockerfile` builds and publishes the API, and a `docker-compose.yml` spins up the API alongside a MongoDB instance with the right environment variables.

## Running it locally

You’ll need Docker installed. Clone the repo and run the following commands from the root:

```bash
docker compose down # stop any previous runs
docker compose up --build
```

This will:

1. Build the API image using the provided `Dockerfile`.
2. Start a `mongo` service (version 6) with a named volume for data.
3. Start the API service on port **8080** and inject the connection string via environment variables:
    - `MongoDB__ConnectionString=mongodb://mongo:27017`
    - `MongoDB__Database=client-auth-mongodb`

After a few seconds, the API will be available at `http://localhost:8080`.

If you want to run the unit tests:

```bash
dotnet test
```

## What I learned

- How to set up **Minimal APIs** in .NET and compare them against controllers.
- Applying **Clean Architecture** by keeping domain entities independent from infrastructure details like MongoDB or JWT.
- Using **repository patterns** over MongoDB and separating domain and document models.
- Generating and verifying JWTs with custom claims using `System.IdentityModel.Tokens.Jwt`.
- Managing configuration via `appsettings.json` and **environment variables** using `IOptions`.
- Writing **unit tests** in xUnit and mocking dependencies.
- Packaging everything with Docker and using **Docker Compose** to orchestrate services.