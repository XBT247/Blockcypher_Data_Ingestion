# BlockCypher.DataIngestion

.NET 9 Clean Architecture API for polling BlockCypher blockchain endpoints, storing data in a Dockerized SQLite DB, and exposing REST endpoints.

## Features
- Polls BlockCypher APIs (ETH, BTC, LTC, DASH) at configurable interval
- Saves JSON + CreatedAt timestamp to DB
- Exposes API for history retrieval (per coin, descending)
- Swagger UI and HealthChecks
- Clean Arch: CQRS, UoW, Repository, DI, AutoMapper, Validation, Logging
- Docker Compose: API and persistent SQLite
- Unit/integration tests

## EF Migrations
in case sqlite throws error, run the below migration in terminal:
- dotnet ef migrations add InitialCreate --project BlockCypher.Persistence --startup-project src/BlockCypher.Api
- dotnet ef database update --project BlockCypher.Persistence --startup-project src/BlockCypher.Api
- dotnet ef migrations list --project BlockCypher.Persistence

## Run
- in project root, run the command: docker-compose up
- swagger url: http://localhost:8080/swagger/index.html

## License

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This project is open source and available under the [MIT License](LICENSE).
