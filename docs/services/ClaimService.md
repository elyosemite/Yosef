## Claims Service

* **Description:**
  The **Claims Service** manages the full lifecycle of insurance claims, from first notice of loss (FNOL) through investigation, assessment, approval/rejection, and settlement. It ensures that policyholders have a structured way to file claims, and brokers/insurers can process them efficiently while maintaining regulatory compliance. This service is **mission-critical** in an insurance platform since claims are the "moment of truth" for clients.

* **Actors:**

  * **Clients (Policyholders):** submit claims, provide documents and updates.
  * **Agents/Brokers:** assist clients in filing claims, track progress.
  * **Claims Adjusters:** investigate, assess, and decide on claims.
  * **Back-office/Insurer Staff:** manage settlements, fraud detection, and compliance.

* **Features:**

  * **FNOL (First Notice of Loss):** allow clients or agents to register a claim quickly.
  * **Claim Lifecycle Management:**

    * Registered → Under Investigation → Awaiting Documents → Approved/Rejected → Settled.
  * **Integration with Policy Service:** validate active coverage and terms at the moment of claim submission.
  * **Evidence & Document Handling:** upload and manage documents, images, medical reports, repair estimates.
  * **Fraud Detection Hooks:** configurable rules or integrations with external fraud detection systems.
  * **Settlement Management:** manage payouts, link with **Payment Service** for disbursements.
  * **Audit & Compliance:** full history of all actions on claims for regulatory oversight.
  * **Notifications:** integrate with Notification Service to inform clients about status updates.

* **Events emitted:**

  * `ClaimRegistered`
  * `ClaimValidated`
  * `ClaimUnderInvestigation`
  * `ClaimDocumentsRequested`
  * `ClaimApproved`
  * `ClaimRejected`
  * `ClaimSettled`
  * `ClaimFraudSuspected`

* **Technology:** **Gleam Language**

  * **Rationale:** Gleam provides strong **type safety**, **fault-tolerance** (compiles to Erlang/BEAM), and excellent concurrency — all critical for a claims engine that must handle complex workflows reliably.
  * **Packages used:**

    * **Squirrel** → for SQL query building.
    * **Pog** → PostgreSQL driver for persistence.
    * **Wisp** → for building HTTP APIs.
  * This ensures a **type-safe, highly concurrent, fault-tolerant service** that integrates seamlessly into the event-driven ecosystem.

Back to [README](/README.md)