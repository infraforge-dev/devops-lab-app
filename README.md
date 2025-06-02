# DevOps Lab App

[![Build & Test](https://github.com/infraforge-dev/devops-lab-app/actions/workflows/dotnet.yml/badge.svg)](https://github.com/infraforge-dev/devops-lab-app/actions/workflows/dotnet.yml)
[![Lint](https://github.com/infraforge-dev/devops-lab-app/actions/workflows/lint.yml/badge.svg)](https://github.com/infraforge-dev/devops-lab-app/actions/workflows/lint.yml)
[![MIT License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

> A full-stack reference application for DevOps training and showcasing .NET backend skills.

---

## Table of Contents

1. [Project Overview](#project-overview)
2. [Tech Stack](#tech-stack)
3. [Prerequisites](#prerequisites)
4. [Features](#features)
5. [Cloud Deployment (AWS & OpenTofu)](#cloud-deployment-aws--opentofu)
6. [Local Installation & Setup](#local-installation--setup)
7. [Usage](#usage)
8. [Code Linting & Style](#code-linting--style)
9. [Testing](#testing)
10. [CI/CD & Deployment](#cicd--deployment)
11. [Architecture & Directory Structure](#architecture--directory-structure)
12. [Query Specification Pattern](#query-specification-pattern)
13. [Roadmap](#roadmap)
14. [Contributing](#contributing)
15. [License](#license)
16. [Acknowledgements & Contact](#acknowledgements--contact)

---

## Project Overview

**DevOps Lab App** is a simple e-commerce–style reference application designed primarily for DevOps training. It also doubles as a showcase of modern backend development practices with .NET 8 and EF Core.
Future modules will demonstrate Infrastructure as Code (IaC), Terraform, AWS deployments, observability dashboards, and CI/CD pipelines.

- **Repo:** [https://github.com/infraforge-dev/devops-lab-app](https://github.com/infraforge-dev/devops-lab-app)
- **Public Postman Workspace:** [https://web.postman.co/workspace/f3d2aeac-ae77-4cee-91f9-6cddc12df833](https://web.postman.co/workspace/f3d2aeac-ae77-4cee-91f9-6cddc12df833)

---

## Tech Stack

- **Backend:**
  - .NET 8 Web API
  - Entity Framework Core (SQL Server/Azure SQL Edge provider)
- **Frontend:**
  - Angular 18
  - TypeScript
  - Tailwind CSS
- **DevOps & Tooling:**
  - Docker Compose (Azure SQL Edge container)
  - Postman (API testing)
  - OpenTofu (Terraform-compatible IaC)
  - AWS ECS (Fargate), ECR, RDS (PostgreSQL), ALB
  - CloudWatch Logs
  - GitHub Actions (CI/CD)
  - Planned: Prometheus/Grafana

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+ & npm](https://nodejs.org/)
- [Docker Engine & Docker Compose](https://docs.docker.com/)
- (Optional) [Postman](https://www.postman.com/) for manual testing

---

## Features

- **Product Catalog API**
  - CRUD endpoints for products
  - EF Core code-first migrations and database seeding on startup
- **Dockerized Local Environment**
  - One-command spin-up for Azure SQL Edge database
- **API Testing Suite**
  - Postman collection with automated tests
- **CI/CD Integration**
  - Automated build, test, and lint workflows via GitHub Actions
- **Future Modules**
  - Infrastructure as Code examples (Terraform)
  - AWS provisioning and deployments
  - CI/CD pipeline definitions
  - Observability (metrics, logs, tracing)

---

## Cloud Deployment (AWS & OpenTofu)

The project now includes a production-ready AWS deployment setup using [OpenTofu](https://opentofu.org/) (a Terraform-compatible, open-source IaC tool).
All cloud infrastructure is defined and managed in the /infra directory.

**Provisioned resources include:**

- ECR (Elastic Container Registry): Stores Docker images for your API.

- ECS (Fargate): Runs your containerized API securely, managed by AWS.

- RDS (PostgreSQL): Free-tier eligible managed database.

- Application Load Balancer (ALB): Publicly exposes your API endpoints.

- IAM Roles & Security Groups: Follows AWS best practices for least privilege and safe networking.

- CloudWatch Logs: Real-time centralized logging for your containers.

### Cloud Deployment Workflow

1. **Build Docker Image**
```bash
docker build -t infraforge-api:latest .
```

2. **Push Image to ECR (Elastic Container Registry in AWS)**
```bash
docker tag infraforge-api:latest <your-account-id>.dkr.ecr.<region>.amazonaws.com/infraforge-api:latest
docker push <your-account-id>.dkr.ecr.<region>.amazonaws.com/infraforge-api:latest
```

3. **Provision AWS Infrastructure with OpenTofu**
```bash
cd infra
tofu init
tofu apply
```
- Enter AWS/DB credentials and Docker tag when prompted.

- ALB DNS and RDS endpoints are output after apply.

4. **Test your API**
- Access your API using the ALB DNS output, e.g.:
```bash
http://<alb-dns-name>/api/v1/products
```

5. **Tear Down Infra (save costs)**
```bash
tofu destroy
```

**Secrets and credentials are passed as variables—never committed to source.**
All infrastructure changes are reproducible and version-controlled.

---

## local Installation & Setup

**Spin up the backend API and Azure SQL Edge database in minutes:**

1. **Clone this repository**
    ```bash
    git clone https://github.com/infraforge-dev/devops-lab-app.git
    cd devops-lab-app
    ```

2. **Start the Azure SQL Edge database using Docker Compose**
    ```bash
    docker compose up -d
    ```
    This uses the [`docker-compose.yaml`](./docker-compose.yaml) file to spin up an [Azure SQL Edge](https://learn.microsoft.com/en-us/azure/azure-sql-edge/) container.

3. **Run the .NET API**
    ```bash
    cd API
    dotnet run
    ```
    - The API will connect to the database, run migrations, and **seed initial data automatically** on first run.
    - By default, the API runs at `https://localhost:5001` or `http://localhost:5000`.

4. **(Optional) Start the Angular frontend**
    - Frontend setup instructions will be added once the Angular app is scaffolded.

5. **(Optional) Test the API with Postman**
    - Import the collection from the [public Postman workspace](https://web.postman.co/workspace/f3d2aeac-ae77-4cee-91f9-6cddc12df833) and run the provided tests.

> **Tip:**
> If you need to customize connection strings, see `API/appsettings.json`. No other configuration is required for local development out of the box.

---

## Usage
**API Endpoints**

- GET /api/v1/products

- GET /api/v1/products/{id}

- GET /api/v1/products/?brand={brand}

- GET /api/v1/products/?type={type}

- GET /api/v1/products/brands

- GET /api/v1/products/types

- POST /api/products

- PUT /api/products/{id}

- DELETE /api/products/{id}

**Postman Testing**

Import the collection from the public workspace.

Run the “DevOps Lab App” collection to exercise all endpoints and view example requests/responses.

---

## Code Linting & Style

Before pushing or opening a PR, run:

    dotnet format

This auto-formats code and fixes most style issues. For analyzer errors or warnings, see your IDE or run:

    dotnet build

All rules are enforced in CI. If the lint workflow fails, check the logs, run `dotnet format`, fix any remaining issues, and push again.

---
## Testing

**Unit and Integration Tests**

Currently, only integration tests are available and can be run:

```bash
cd Tests\API.IntegrationTests
dotnet test
```
---

## CI/CD & Deployment

CI/CD is powered by GitHub Actions.

### Current workflows

- Build & Integration Test Workflow
    - Runs on every push and pull request to main and develop.
    - Restores dependencies, builds the solution, and executes integration tests.
- Lint Workflow
    - Runs on every push and PR.
    - Runs ```dotnet format``` to enforce code style and formatting rules.

### How it works

- All pushes and pull requests are validated via these workflows before merging.

- Failing tests or lint errors must be resolved before PRs can be merged.

- (Planned) On merge to main, a Docker image will be built and published to a container registry.

---

## Architecture & Directory Structure

```text
.
├── DevOpsLabApp.sln
├── .gitignore
├── docker-compose.yml
├── API/
│   ├── Controllers/
│   ├── appsettings.json
│   └── Program.cs
├── Core/
│   ├── Entities/
│   │   └── Product.cs
│   └── Interfaces/
│       └── IProductRepository.cs
├── Infrastructure/
│   ├── Config/
│   │   └── ProductConfiguration.cs
│   ├── Data/
│   │   ├── Context.cs
│   │   └── Seed/
│   │       └── SeedData.cs
│   ├── Migrations/
│   └── Repositories/
│       └── ProductRepository.cs
├── infra/
│   ├── main.tf
│   ├── variables.tf
│   ├── outputs.tf
│   └── provider.tf
└── README.md
```

- **API/** — .NET 8 Web API project, contains controllers, configuration, and entry point.
- **Core/** — Domain layer with entity definitions and repository interfaces.
- **Infrastructure/** — EF Core implementation: configurations, DbContext & seeding, migrations, and repository classes.
- **Tests/** - XUnit Integration Tests
- **Infra/** - Cloud infrastructure definition and management files using OpenTofu
- **docker-compose.yml** & **.sln** — root-level orchestration and solution file.

---

## Query Specification Pattern

This project implements the Specification Pattern to encapsulate filtering, sorting, and projection logic in a clean, reusable way. The pattern is used across the `ProductRepository`, `GenericRepository<T>`, and controller layers.

### Benefits:
- Promotes single-responsibility and composability of query logic.
- Separates LINQ expressions for criteria, ordering, and selection.
- Simplifies API controller actions — specs handle all query logic.
- Supports projection with automatic `.Distinct()` application where needed.

### Example Use Cases:
- `/api/v1/products?brand=Logitech&type=Peripheral&sort=PriceAsc`
- `/api/v1/products/brands` – returns distinct brand values
- `/api/v1/products/types` – returns distinct type values

### Relevant Files:
- `Core/Specifications/BaseSpecification.cs`
- `Core/Specifications/BaseSpecification.TResult.cs`
- `Core/Specifications/ProductSpecification.cs`
- `Core/Specifications/BrandListSpecification.cs`
- `Infrastructure/Data/SpecificationEvaluator.cs`

---

## Roadmap

- [x] Infrastructure as Code: AWS infra using OpenTofu (ECR, ECS Fargate, RDS, ALB, IAM, CloudWatch)

- [x] Dockerized API image and registry setup

- [x] ECS/ALB public deployment with secrets passed via environment variables

- [ ] Automated CI/CD: GitHub Actions build/test/lint + ECR deploy (in progress)

- [ ] Custom domain + HTTPS (in progress)

- [ ] Cloud observability dashboards (future)

- [ ] Frontend deployment (future)

- [ ] Kubernetes demo (future)

---

## Contributing

1. Fork the repo

2. Create a feature branch (git checkout -b feature/XYZ)

3. Commit your changes (git commit -m "Add feature XYZ")

4. Push to your branch (git push origin feature/XYZ)

5. Open a Pull Request against main

6. Ensure all tests pass on CI before merging (Coming soon!)

---

## License

This project is licensed under the MIT License. See LICENSE for details.

---

## Acknowledgements & Contact

- Author: Jose Rodriguez

- Issues & Feedback: Create an issue

- Email: jose@infraforge.dev
