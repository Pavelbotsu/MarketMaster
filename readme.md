MarketMaker .NET
Multi-Tenant SaaS Marketplace Infrastructure
MarketMaker is a high-performance Marketplace-as-a-Service (MaaS) platform. Built on .NET 10, it enables users to launch independent, white-labeled marketplaces with custom domains. It serves as a sophisticated architectural blueprint for building scalable, multi-tenant software systems.

The project demonstrates advanced implementation of Microservices, Clean Architecture, and a strict Database-per-Tenant isolation model for enterprise-grade security.

Architectural Design
The system is designed as a decoupled microservices ecosystem where each business capability is isolated and independently scalable.

Core Pillars
Database-per-Tenant Isolation: Every vendor is provisioned with a physically separate PostgreSQL database. This ensures total data privacy, prevents cross-tenant leakage, and allows for per-tenant maintenance or scaling.

Clean Architecture (Onion): Each service is partitioned into Domain, Application, Infrastructure, and API layers. This ensures that business logic remains independent of frameworks, databases, and external UI concerns.

Dynamic Tenant Resolution: Custom middleware identifies the tenant context at runtime using the Host header (supporting custom domains). It resolves connection strings dynamically from a central Master Database.

Event-Driven Communication: Asynchronous inter-service communication is handled via RabbitMQ and MassTransit, ensuring high system resilience and eventual consistency.

Key Capabilities
Platform Administration (Super Admin)
Automated Provisioning: Programmatic creation of tenant databases and infrastructure.

Domain Orchestration: Management of custom DNS mappings and SSL termination logic.

Global Monitoring: Platform-wide health checks and SaaS subscription tracking.

Store Management (Tenant Admin)
Isolated Catalog: Full CRUD operations for products and categories within the tenant's private database.

Fulfillment Portal: Dedicated order management and customer data handling for specific vendors.

Role-Based Access: Secure authentication ensuring store owners only access their authorized environment.