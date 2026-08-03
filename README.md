# IsoTreatment — Strangler Fig Demo

This repository demonstrates a real-world migration from a **legacy monolith** to a **microservice architecture** using the **strangler fig** pattern, with an **API gateway** routing traffic in front of both systems.

## What's in here

The repo contains three .NET applications that run side-by-side:

### `IsoTreatment.ApiGateway/` — the API gateway
A thin reverse proxy (.NET 10, [YARP](https://microsoft.github.io/reverse-proxy/)) that is the single entry point for clients. It matches incoming paths and forwards each request either to the legacy monolith or to the extracted microservice, based on routing rules in its `appsettings.json`. This is the "trunk" the strangler fig grows around.

### `IsoTreatmentProcessSupportAPI/` — the legacy monolith
The original application (.NET 8) that handles the full domain: users, authentication, treatment processes, entries, reminders, etc. It uses Entity Framework Core 6 with SQL Server, JWT cookie-based auth, FluentValidation, AutoMapper, MailKit, and NLog.

### `IsoTreatment.Users/` — the extracted microservice
A new service (.NET 10, Clean Architecture: `Api` / `Application` / `Domain` / `Infrastructure`, CQRS-style command/query handlers) that takes over responsibility for **user data and authentication** — the first capability being carved out of the monolith.

## The strangler fig approach

Rather than doing a big-bang rewrite, functionality is strangled out of the monolith one capability at a time:

1. The **API gateway** is placed in front of the monolith, so every client goes through it.
2. A new microservice (`IsoTreatment.Users`) is built that reimplements one capability — the **user module** — at parity with the monolith.
3. The gateway is reconfigured to route that capability's traffic to the new microservice, while everything else keeps flowing to the monolith.
4. Repeat for the next capability. As more routes are redirected, the monolith is gradually "strangled" until it can be retired.

Routing lives entirely in the gateway's `IsoTreatment.ApiGateway/appsettings.json`:

```jsonc
"ReverseProxy": {
  "Routes": {
    "microservice-users": {        // user traffic → microservice
      "ClusterId": "microservice-users-cluster",
      "Match": { "Path": "users-api/{**catch-all}" }
    },
    "monolith": {                  // everything else → legacy monolith
      "ClusterId": "monolith-cluster",
      "Match": { "Path": "isotreatment-api/{**catch-all}" }
    }
  }
}
```

Moving another capability to a microservice is just a matter of adding a route + cluster here — no client changes, no big-bang cut-over.

## Architecture overview

```
                         client
                           │
                           ▼
              ┌──────────────────────────────┐
              │   IsoTreatment.ApiGateway    │   ← YARP reverse proxy (.NET 10)
              │           (port 5124)        │
              │                              │
              │   users-api/**  ─────────────┼──────────────┐
              │   isotreatment-api/**  ──┐    │              │
              └─────────────────────────┼────┘              │
                                        │ HTTP              │ HTTP
                                        ▼                   ▼
        ┌────────────────────────────────┐   ┌──────────────────────────────┐
        │  IsoTreatmentProcessSupportAPI │   │   IsoTreatment.Users         │
        │      legacy monolith (.NET 8)  │   │   microservice (.NET 10)     │
        │          (port 5113)           │   │        (port 5250)           │
        └───────────────┬────────────────┘   └───────────────┬──────────────┘
                        │                                     │
                        └──────────────┬──────────────────────┘
                                       ▼
                          SQL Server (port 1433)
                          DB: IsoTreatmentProcessSupport

        All three services export traces via OpenTelemetry (OTLP) to
                          Jaeger  (UI on port 16686)
```

Path routing through the gateway:

| Gateway path | Forwarded to |
|---|---|
| `http://localhost:5124/users-api/{...}` | microservice `http://isotreatment.users:8080/api/user/{...}` |
| `http://localhost:5124/isotreatment-api/{...}` | monolith `http://isotreatment.monolith:8080/api/{...}` |

## The users module

The microservice reimplements the monolith's user endpoints at behavioral parity. All are served under `/api/user` (reachable through the gateway as `users-api/...`):

| Method & path | Description |
|---|---|
| `POST /register` | Create an account and send an email-confirmation link |
| `POST /confirmEmail` | Confirm the account from the emailed token |
| `POST /login` | Authenticate and set the `token` JWT cookie |
| `POST /logout` | Clear the auth cookie |
| `GET  /info` | Get the current user's profile *(auth required)* |
| `POST /info/update` | Update the current user's profile *(auth required)* |
| `POST /forgot-password` | Email a password-reset link |
| `POST /reset-password` | Set a new password from the reset token |

Auth is JWT stored in an HTTP-only `token` cookie. Input is validated with FluentValidation, confirmation/reset mails are sent through MailKit, and domain exceptions are mapped to HTTP status codes by an error-handling middleware.

## Configuration & secrets

Secrets are **not** committed. They live in a git-ignored `.env` file at the repository root, which Docker Compose interpolates into the services. A template is provided in `.env.example`:

```bash
cp .env.example .env
# then fill in the values
```

```dotenv
DB_PASSWORD=your-sql-server-sa-password
EMAIL_PASSWORD=your-smtp-app-password
```

`DB_PASSWORD` is used both for the SQL Server SA account and for the services' connection strings; `EMAIL_PASSWORD` is the SMTP password used to send emails.

## Running the project

Everything is orchestrated through Docker Compose.

### Prerequisites
- Docker Desktop (or Docker Engine + Docker Compose v2)
- A populated `.env` file (see above)

### Start everything
From the repository root:

```bash
docker compose up --build
```

This builds and starts:

| Service | URL / address |
|---|---|
| `isotreatment.apigateway` — API gateway (entry point) | http://localhost:5124 |
| `isotreatment.monolith` — legacy API | http://localhost:5113 (Swagger at `/swagger`) |
| `isotreatment.users` — users microservice | http://localhost:5250 (OpenAPI at `/openapi/v1.json` in Development) |
| `isotreatment.database` — SQL Server | `localhost:1433` |
| `jaeger` — Jaeger UI | http://localhost:16686 |

Send client requests to the **gateway** (port 5124) — for example `POST http://localhost:5124/users-api/login`.

### Stop everything

```bash
docker compose down
```

Add `-v` to also remove the database volume (`mssql_data`) for a clean reset.

## Observability

All three services are instrumented with OpenTelemetry and export traces over OTLP to Jaeger. Open the Jaeger UI at http://localhost:16686 to follow a request as it flows from the gateway into the monolith or the microservice.

## Tech stack

- **.NET 10** (gateway, microservice) and **.NET 8** (monolith)
- **YARP** reverse proxy
- **Entity Framework Core** (10 in the microservice, 6 in the monolith) + **SQL Server**
- **FluentValidation**, **MailKit**, **JWT** cookie auth
- **OpenTelemetry** + **Jaeger** for distributed tracing
- **Docker Compose** for orchestration
