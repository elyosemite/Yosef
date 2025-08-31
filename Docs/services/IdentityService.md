## 1. **Identity Service**

* **Description:**
  Centralized service for authentication, authorization, and user management. It manages **brokers, agents, and clients**. Ensures multi-tenant access control across insurers and brokers. Acts as the **entry point** for all user identity and permissions.
* **Actors:** Brokers (companies), Agents (sellers), Clients (policyholders), System administrators.
* **Features:**

  * User registration (brokers, agents, clients)
  * OAuth2/JWT authentication
  * Role-based access control (RBAC)
  * Multi-tenant support for multiple broker companies
  * Audit logging for compliance

* **Events emitted:**
  * `BrokerCreated`, `BrokerUpdated`
  * `AgentCreated`, `AgentUpdated`
  * `ClientRegistered`, `ClientUpdated`
  * `UserRoleAssigned`, `UserRoleRevoked`

* **Technology:** **.NET C#**
  * Rationale: Identity management benefits from strong typing, enterprise-level security libraries, and integration with tools like **IdentityServer**.
