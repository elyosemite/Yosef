## Payment Service

* **Description:**
  Handles **payment capture and reconciliation** with payment gateways (credit card, PIX, bank slip). Ensures policy activation after successful payment.
* **Actors:** Clients, Payment gateways.
* **Features:**

  * Integration with multiple gateways
  * Payment initiation & tracking
  * Reconciliation with bank/gateway notifications
  * Refunds and chargebacks
  * Payment retries and error handling
* **Events emitted:**

  * `PaymentInitiated`
  * `PaymentConfirmed`
  * `PaymentDeclined`
  * `PaymentRefunded`
* **Technology:** **Golang**

  * Rationale: Payment requires **high throughput & reliability**; Go is well-suited for event-driven and concurrent tasks.

Back to [README](/README.md)