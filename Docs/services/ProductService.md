## Product Service

* **Description:**
  Manages insurance products, including dynamic form builders and parameterized factors used in pricing. It allows **insurance specialists** to create and publish new products without developer intervention.
* **Actors:** Product specialists, Actuaries, Brokers.
* **Features:**

  * Product catalog management
  * Dynamic form builder (multi-choice, numeric, boolean, conditional questions)
  * Parametrization engine (weights, factors, rules)
  * Product versioning & publishing workflow
  * Sandbox testing for pricing formulas
* **Events emitted:**

  * `ProductVersionCreated`
  * `ProductPublished`
  * `ProductDeprecated`
  * `FormStructureChanged`
  * `ParameterizationChanged`
* **Technology:** **TypeScript (Node.js)**

  * Rationale: JSON-driven forms, high flexibility, good developer productivity, ideal for integrating with front-end UIs.