## Quotation Service

* **Description:**
  Generates insurance quotations by combining **customer-provided answers** with **product parameters** from the Product Service. Supports real-time premium calculation and quote lifecycle.
* **Actors:** Clients, Agents, Brokers.
* **Features:**

  * Start and manage quotations
  * Real-time premium calculation using product factors
  * Support for quote acceptance and expiration
  * Cache integration with Product Service for fast evaluations
  * Audit of all quotations for analytics
* **Events emitted:**

  * `QuoteStarted`
  * `QuoteCalculated`
  * `QuoteAccepted`
  * `QuoteExpired`
* **Technology:** **Golang**

  * Rationale: High-performance, low-latency service ideal for **real-time premium calculations**.

Back to [README](/README.md)