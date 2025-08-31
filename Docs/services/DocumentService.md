## Document Service

* **Description:**
  Generates and stores official documents (policies, endorsements, cancellation letters). Provides secure document retrieval.
* **Actors:** Clients, Agents, Brokers, Back-office staff.
* **Features:**

  * PDF/Document generation (policy contracts, endorsements)
  * Secure storage with access control
  * Digital signatures and timestamping
  * API for retrieval by clients and brokers
* **Events emitted:**

  * `DocumentGenerated`
  * `DocumentUpdated`
  * `DocumentArchived`
* **Technology:** **Python**

  * Rationale: Document processing is file-oriented; Python has excellent libraries for PDF generation and manipulation.
