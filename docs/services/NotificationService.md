## Notification Service

* **Description:**
  Sends notifications (email, SMS, WhatsApp, push). Acts as an event consumer and dispatcher.
* **Actors:** Clients, Brokers, Agents.
* **Features:**

  * Email/SMS/Push integration
  * Templating for messages
  * Retry & queueing of failed notifications
  * Multi-channel preferences per user
* **Events emitted:**

  * `NotificationSent`
  * `NotificationFailed`
* **Technology:** **TypeScript (Node.js)**

  * Rationale: Notifications require integration with many APIs (SendGrid, Twilio, WhatsApp Business). Node.js excels at async I/O.

Back to [README](/README.md)