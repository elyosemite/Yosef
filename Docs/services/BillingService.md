## Billing Service

* **Description:**
  Responsible for **generating invoices** and managing premium installments. Works closely with Policy and Payment. Tracks outstanding amounts and overdue invoices.
* **Actors:** Clients, Back-office financial team.
* **Features:**

  * Generate invoices based on policy premiums
  * Manage installment plans (monthly, yearly, custom)
  * Invoice lifecycle (issued, paid, overdue, canceled)
  * Automatic overdue detection
  * Integration with external ERP if needed
* **Events emitted:**

  * `InvoiceGenerated`
  * `InvoiceOverdue`
  * `InvoiceCanceled`
  * `InvoicePaid`
* **Technology:** **Python**

  * Rationale: Finance/billing often integrates with external APIs (banks, ERP). Python offers fast prototyping and good libraries for financial data handling.
