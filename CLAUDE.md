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

---

## Implementation Rules

These rules apply to every session, every phase.

### Step-by-step execution
- Implement **one phase at a time**. Do not start the next phase until the current one builds and its commits are done.
- Every phase is broken into **logical blocks**. Each block gets its own commit before moving on.
- After each commit, verify the build passes (`dotnet build`) before proceeding.

### Commit discipline
- **One logical block per commit** — do not mix unrelated files.
- Keep commits small. Typical logical blocks and what goes in each commit:

| Block | Files in the commit |
|---|---|
| Domain event | `*Event.cs` + EventCatalog event `index.mdx` |
| Domain aggregate change | aggregate `.cs` + any new value objects |
| Application handler | `*Request.cs`, `*Response.cs`, `*Handler.cs`, `*Validator.cs` |
| Infrastructure model | `*DataModel.cs`, `*Configuration.cs`, `*Profile.cs` (AutoMapper) |
| Repository | `I*Repository.cs`, `*Repository.cs`, DI registration update |
| Presentation endpoint | `*Request.cs`, `*Response.cs`, endpoint `.cs`, route registration in `Program.cs` |
| Docs update | EventCatalog `index.mdx` files affected by the block |
| Migration | only the generated migration files |

- Docs updates may be bundled into the same commit as the block they document **or** committed immediately after — never deferred to "later".
- Never commit generated code (migrations) together with domain/application code.

### Documentation
- Every command, query, or event implemented must be reflected in EventCatalog **in the same session**.
- The EventCatalog service `index.mdx` `sends:` / `receives:` lists must stay in sync with implemented events.

---

## Development Plan — ProjectManagement MVP

Legend: `done` · `in progress` · `pending`

### Phase 0 — Foundation

| Step | Description | Status |
|---|---|---|
| 0.1 | Rename `Organization` → `Brokerage` across Domain, Application, Infrastructure (85 files). Keep DB column names via `HasColumnName`. | done |
| 0.2 | Rename Presentation layer (`CreateOrganization` → `CreateBrokerage`, `GetOrganization` → `GetBrokerage`). Update `Program.cs`. Fix routes to `/api/v1/`. Add ProblemDetails middleware. | done |

### Phase 1 — Brokerage CRUD

| Step | Description | Status |
|---|---|---|
| 1.1 | `CreateBrokerageCommand`: handler, validator, request/response + endpoint + EventCatalog | pending |
| 1.2 | `ArchiveBrokerageCommand`: handler, validator, request/response + endpoint + EventCatalog | pending |
| 1.3 | `RenameBrokerageCommand`: handler, validator, request/response + endpoint + EventCatalog | pending |
| 1.4 | `GetBrokerageQuery`: handler, request/response + endpoint + EventCatalog | pending |
| 1.5 | `CreateProjectCommand`: handler, validator, request/response + endpoint + EventCatalog | pending |

### Phase 2 — Broker

| Step | Description | Status |
|---|---|---|
| 2.1 | Domain: `Broker` entity inside `Brokerage` aggregate + `BrokerRegisteredEvent` | pending |
| 2.2 | `RegisterBrokerCommand`: handler, validator, request/response + Infrastructure (data model, config, AutoMapper) + `IBrokerRepository` + `BrokerRepository` + DI | pending |
| 2.3 | `RegisterBrokerCommand` endpoint + route in `Program.cs` | pending |
| 2.4 | `ListBrokersQuery`: handler + endpoint + EventCatalog | pending |

### Phase 3 — Insured

| Step | Description | Status |
|---|---|---|
| 3.1 | Domain: `Address` value object + `BrokerInsured` value object + `Insured` aggregate root | pending |
| 3.2 | Domain events: `InsuredRegisteredEvent` + `InsuredAssociatedToBrokerEvent` | pending |
| 3.3 | `RegisterInsuredCommand`: handler, validator, request/response + Infrastructure (data model, config, AutoMapper) + `IInsuredRepository` + `InsuredRepository` + DI | pending |
| 3.4 | `RegisterInsuredCommand` endpoint + route in `Program.cs` | pending |
| 3.5 | `AssociateInsuredToBrokerCommand`: handler, validator, request/response + endpoint + route | pending |
| 3.6 | `ListInsuredsByBrokerageQuery`: handler + endpoint | pending |
| 3.7 | `ListInsuredsByBrokerQuery`: handler + endpoint + EventCatalog (all list queries) | pending |
