# Yosef — Project Context for Claude

## What this is

Yosef is an open-source microservices platform for the insurance industry built with .NET C#, Python, Gleam, TypeScript, and Go. It covers the full insurance lifecycle: organization management, quotations, policies, claims, notifications, and analytics.

---

## Services

| Service | Tech | Status | Responsibility |
|---|---|---|---|
| ProjectManagement | .NET 9 / C# | Active | Brokerages, Brokers, Insureds, Projects |
| Identity | Python / FastAPI | Minimal | Auth via KeyCloak |
| EventProcessor | .NET 9 Worker | Partial | Outbox + async event dispatch |
| Quotation | .NET 9 / C# | Skeleton | Quotation lifecycle |
| Claim | Gleam / Wisp | Bootstrap | Claim lifecycle |
| Policy | .NET 9 / C# | Planned | Policy lifecycle |
| Notification | — | Planned | SMS, gRPC, messaging |
| Analytics | — | Planned | Domain event aggregation, dashboards |

---

## Architecture Decisions

- **DDD + Clean Architecture** per service: Domain → Application → Infrastructure → Presentation.
- **CQRS** via MediatR inside each service.
- **Outbox pattern** for reliable domain event publishing (see `EventProcessor`).
- **Dual identity**: entities carry both a `Guid Identifier` (domain) and an `int Id` (database table key).
- **ProjectManagement** is the first deliverable. The immediate focus is implementing `Broker` and `Insured` entities within it, alongside the existing `Brokerage` (formerly `Organization`).
- **Rename strategy:** Rename `Organization` → `Brokerage` in-place across all 85 files. No rewrite. Rename domain events accordingly (`OrganizationCreatedEvent` → `BrokerageCreatedEvent`, etc.).
- **Identity vs ProjectManagement**: Kept separate. KeyCloak owns authentication; ProjectManagement owns the business hierarchy (Brokerage/Broker/Insured). Do not merge them.
- **Authentication (MVP):** Endpoints are open in the MVP. Auth via KeyCloak JWT is deferred — added later as a cross-cutting concern without touching CQRS handlers.
- **API Versioning (ADR-001):** All REST endpoints prefixed with `/api/v1/`. Breaking changes increment to `/api/v2/` — `v1` stays alive for one deprecation cycle. See `docs/docs/architecture-decision-records/01-rest-api-versioning.mdx`.
- **Pagination (MVP):** No pagination. List endpoints return full results. Add offset pagination post-MVP.
- **Error responses:** ProblemDetails (RFC 7807) via `AddProblemDetails()` + exception handler middleware. Returns `type`, `title`, `status`, `detail`. HTTP semantics: `404` for not found, `422` for domain rule violations, `400` for invalid input.

## MVP Scope — ProjectManagement Service

### Commands (writes)
| # | Operation | Aggregate | Event Published |
|---|---|---|---|
| 1 | Create Brokerage | Brokerage | `BrokerageCreatedEvent` |
| 2 | Register Broker | Brokerage | `BrokerRegisteredEvent` |
| 3 | Register Insured | Insured | `InsuredRegisteredEvent` |
| 4 | Associate Insured to Broker | Insured | `InsuredAssociatedToBrokerEvent` |
| 5 | Create Project | Brokerage | `ProjectCreatedEvent` |
| 6 | Archive Brokerage | Brokerage | `BrokerageArchivedEvent` |
| 7 | Rename Brokerage | Brokerage | `BrokerageNameUpdatedEvent` |

### Queries (reads)
- Get Brokerage by ID
- List Brokers of a Brokerage
- List Insureds of a Brokerage (consolidated view)
- List Insureds of a specific Broker

---

## Current Domain Model (ProjectManagement)

```
Brokerage (aggregate root)         ← was "Organization"
  - Identifier: Guid
  - Name: string
  - Active: bool
  - Projects: Project[]            ← child entity (stays inside Brokerage aggregate)
  - Brokers: Broker[]              ← child references only (Broker is its own aggregate)

Broker (entity, inside Brokerage aggregate)  ← new
  - Identifier: Guid
  - Name: string
  - Active: bool

Insured (aggregate root)           ← new
  - Identifier: Guid
  - BrokerageIdentifier: Guid      ← registered at Brokerage level
  - InsuredType: enum (Individual | Corporate)
  - Name: string
  - TaxId: string                  ← CPF when Individual, CNPJ when Corporate (no format validation)
  - Email: string
  - Phone: string?
  - Address: Address               ← value object (Brazilian model only for MVP)
  - BrokerAssociations: BrokerInsured[]  ← N:M join owned by Insured aggregate

Address (value object, inside Insured aggregate — Brazilian model)
  - Street: string (required)
  - Number: string (required)
  - Complement: string? (optional)
  - Neighborhood: string (required)
  - City: string (required)
  - State: string (required, 2-char UF)
  - ZipCode: string (required, CEP sem máscara)
  - Country: string (default "BR")

BrokerInsured (value object / join, inside Insured aggregate)
  - BrokerIdentifier: Guid
  - AssociatedAt: DateTime

Project (entity, inside Brokerage aggregate)  ← keep as-is
  - Identifier: Guid
  - BrokerageIdentifier: Guid
  - Name: string
  - Description: string
```

---

## Observability Stack

Prometheus (metrics) · Grafana Loki (logs) · Jaeger (tracing) · Grafana (dashboards) · OpenTelemetry Collector.
All .NET services instrument via `OpenTelemetry` SDK.

## Infrastructure

PostgreSQL (primary DB) · RabbitMQ (async messaging) · KeyCloak (IAM) · HashiCorp Vault (secrets).
Local dev via `docker-compose up -d`.
