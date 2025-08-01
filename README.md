# Yosef: An Open-Source Insurance Microservices Platform

[](https://www.google.com/search?q=https://github.com/elyosemite/Yosef)
[](https://opensource.org/licenses/MIT)
[](https://www.google.com/search?q=./CONTRIBUTING.md)

Yosef is a comprehensive, open-source microservices platform built with .NET C\# for the financial insurance industry. It provides a robust, scalable, and observable foundation for building modern insurance applications.

This project is designed to be a community-driven effort. We welcome developers to use it as a boilerplate for their own solutions, contribute to its core features, and help it grow.

## Features

  * **Microservices Architecture:** A decoupled architecture that ensures scalability and maintainability.
  * **Identity Management:** Centralized identity and access management for brokerages, brokers, and other entities.
  * **Project-Based Organization:** Manage insurance sales initiatives as distinct projects (e.g., Bike Insurance, Ship Insurance).
  * **Full Insurance Lifecycle:** From quotation management to policy creation.
  * **Asynchronous Processing:** Dedicated services for handling asynchronously background jobs and event-driven communication.
  * **High Observability:** Pre-configured stack for monitoring, logging, and tracing. In this case, it uses Grafana.
  * **Enterprise-Grade Security:** Integrated with industry-standard tools for secrets and identity management.

## Architecture & Services

Yosef is composed of several specialized microservices that work together to provide a complete insurance platform.

  * **`Identity Service`**

      * **Description:** Manages all identity-related operations. It integrates with **KeyCloak** to handle authentication and authorization for brokerages, brokers, clients, and other roles within the system.

  * **`ProjectManagement Service`**

      * **Description:** Allows for the creation and organization of insurance projects. This enables brokerages to segment their sales efforts, for example: a project for selling Bike insurance, another for Ship insurance, or one for Aircraft insurance.

  * **`Quotation Service`**

      * **Description:** A dedicated service for creating and managing insurance quotations. It handles the initial pricing and terms before a policy is finalized.

  * **`Policy Service`**

      * **Description:** Responsible for creating and managing insurance policies. A policy is the final contract, which is generated from an approved quotation.

  * **`Notification Service`**

      * **Description:** Handles all outbound communications. It integrates with message brokers like **RabbitMQ**, **Kafka**, or **Azure Service Bus** and can deliver messages via various channels, including **SMS** and **gRPC**.

  * **`EventProcessor Service`**

      * **Description:** Acts as the asynchronous backbone of the platform. Any event or background job that needs to be processed asynchronously by the services above is handled here. This ensures that the system remains responsive and resilient.

## Technology Stack

This project leverages a modern, cloud-native technology stack to ensure performance, reliability, and security.

| Category                | Technology / Tool                                     |
| ----------------------- | ----------------------------------------------------- |
| **Core Framework** | .NET (C\#)                                             |
| **Observability** | **Grafana** (Visualization)                           |
|                         | **Grafana Loki** (Log Aggregation)                    |
|                         | **Jaeger** (Distributed Tracing)                        |
|                         | **Prometheus** (Metrics & Alerting)                     |
| **Security** | **KeyCloak** (Identity & Access Management)           |
|                         | **HashiCorp Vault** (Secrets Management)              |
| **Database & Caching** | **SQLite** (Current, for development)                 |
|                         | **PostgreSQL** (Planned, for production relational)   |
|                         | **Redis** (Planned, for caching)                      |
| **Messaging** | RabbitMQ, Kafka, Azure Service Bus (via Notification) |

## Observability

A core principle of Yosef is full observability into the system's health and performance.

  * **Metrics (Prometheus):** Each microservice exposes key performance indicators (KPIs) and health metrics, which are scraped by Prometheus.
  * **Logging (Loki):** Structured logs from all services are collected and indexed by Grafana Loki, allowing for powerful and efficient log querying.
  * **Tracing (Jaeger):** End-to-end distributed tracing provides deep insight into request flows across multiple services, making it easy to pinpoint bottlenecks and debug issues.
  * **Visualization (Grafana):** Grafana serves as the single pane of glass, bringing together metrics, logs, and traces into unified dashboards for a complete overview of the platform.

## Security

Security is a first-class citizen in the Yosef project.

  * **Identity Management (KeyCloak):** We use KeyCloak, a powerful open-source IAM solution, to secure our services. It handles user authentication, service-to-service authorization (via OAuth 2.0/OIDC), and fine-grained access control.
  * **Secrets Management (Vault):** All sensitive information—such as database connection strings, API keys, and certificates—is managed by HashiCorp's Vault. This prevents secrets from being hardcoded in configuration files or source code.

## Database

To facilitate easy setup and development, the project currently uses **SQLite3**.

### Future Roadmap

Our goal is to be production-ready. The following database support is planned for the near future:

  * **PostgreSQL:** Will be supported as the primary relational database for robust, production-grade data persistence.
  * **Redis:** Will be integrated for high-performance caching to reduce database load and improve response times.

## Getting Started

To get a local copy up and running, please follow these steps.

### Prerequisites

  * [.NET SDK](https://dotnet.microsoft.com/download)
  * [Docker Desktop](https://www.docker.com/products/docker-desktop) (to run KeyCloak, Vault, and the observability stack)

### Installation

1.  **Clone the repository:**

    ```sh
    git clone https://github.com/elyosemite/Yosef.git
    cd Yosef
    ```

2.  **Configure Environment:**

      * Set up your `appsettings.Development.json` files with the necessary connection details for Vault and KeyCloak.
      * (More detailed instructions to be added here).

3.  **Run the Infrastructure:**

      * A `docker-compose.yml` file is provided to easily spin up all necessary infrastructure services (Grafana, Loki, Prometheus, Jaeger, KeyCloak, Vault).

    <!-- end list -->

    ```sh
    docker-compose up -d
    ```

4.  **Run the Services:**

      * Navigate to each service directory and run the application:

    <!-- end list -->

    ```sh
    dotnet run
    ```

## Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".

1.  Fork the Project
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4.  Push to the Branch (`git push origin feature/AmazingFeature`)
5.  Open a Pull Request

Please read our [CONTRIBUTING.md](https://www.google.com/search?q=./CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

Distributed under the MIT License. See `LICENSE` for more information.