## Project Management Service

* **Description:**
  Provides **logical spaces of work (projects)** inside a broker/organization. Each project groups **products, users, and permissions** to ensure that sellers and agents only see the relevant scope of operations. For example, CloudBeholder Insurance can create projects such as *Life Insurance*, *Bike Insurance*, *Aircraft Insurance*, or *Marine Insurance*. Within each project, brokers can configure who has **read/write access** to products, quotations, and policies. This service enforces **data segregation** and **multi-project visibility** for complex organizations.

* **Actors:**

  * **Brokers/Organization Admins:** create and manage projects, assign products and users.
  * **Agents/Sellers:** operate inside projects, seeing only their allocated scope.
  * **Clients:** indirectly benefit, since their data is linked to projects managed by brokers.

* **Features:**

  * Project creation and lifecycle management (active, archived).
  * Project–Product association (which products are enabled per project).
  * Project–User association with role-based permissions (read, write, admin).
  * Visibility controls: agents can only access data inside their project(s).
  * Project audit trail (changes in scope, users, products).
  * Multi-project support per organization (brokers can run multiple lines of business separately).
  * API for integration with Identity & Product services to enforce project scoping.

* **Events emitted:**

  * `ProjectCreated`
  * `ProjectUpdated`
  * `ProjectArchived`
  * `ProjectUserAssigned`
  * `ProjectUserRemoved`
  * `ProjectProductAssigned`
  * `ProjectProductRemoved`

* **Technology:** **.NET C#**

  * **Rationale:** Project and organizational management involves complex relational data (projects ↔ users ↔ products), strong role enforcement, and long-term consistency. .NET with Entity Framework (EF Core) is well-suited for this domain because of its mature support for complex **multi-tenant and RBAC** systems.

Back to [README](/README.md)