## Event Processor Service

* **Description:**
  The **event-driven backbone**. Routes, enriches, and processes events from all other services. Handles retries, DLQ (dead letter queues), and monitoring.
* **Actors:** Internal only (no direct human interaction).
* **Features:**

  * Event bus integration (Kafka, RabbitMQ, NATS)
  * Retry logic & error handling
  * Event enrichment (adding metadata before forwarding)
  * Audit logging of events
* **Events emitted:**

  * `EventReprocessed`
  * `EventEnriched`
* **Technology:** **Golang**

  * Rationale: Go is well-suited for high-performance event streaming and concurrency.
