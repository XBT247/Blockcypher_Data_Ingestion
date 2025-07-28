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

## Quick Start
