## Policy Service

* **Description:**
  Manages the **full lifecycle of policies**: creation, issuance, suspension, cancellation, endorsement, and renewal. It is the **source of truth** for policy states.
* **Actors:** Clients, Agents, Brokers, Back-office staff.
* **Features:**

  * Policy issuance from accepted quotations
  * Policy state machine (pending, active, suspended, canceled, renewed)
  * Endorsement workflows (changes in coverage or insured objects)
  * Renewal processing
  * Integration with billing & documents
* **Events emitted:**

  * `PolicyCreated`
  * `PolicyIssued`
  * `PolicySuspended`
  * `PolicyCanceled`
  * `PolicyRenewed`
  * `PolicyEndorsed`
* **Technology:** **.NET C#**

  * Rationale: Policies involve **complex business rules** and long-term state; .NET provides robustness for enterprise workflows.
