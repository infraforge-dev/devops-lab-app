# DevOps Lab App

[![Build & Test](https://github.com/infraforge-dev/devops-lab-app/actions/workflows/dotnet.yml/badge.svg)](https://github.com/infraforge-dev/devops-lab-app/actions/workflows/dotnet.yml)
[![Lint](https://github.com/infraforge-dev/devops-lab-app/actions/workflows/lint.yml/badge.svg)](https://github.com/infraforge-dev/devops-lab-app/actions/workflows/lint.yml)
[![MIT License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

> A full-stack reference application for DevOps training and showcasing .NET backend skills.

---

## Table of Contents

1. [Project Overview](#project-overview)
2. [Tech Stack](#tech-stack)
3. [Features](#features)
4. [Prerequisites](#prerequisites)
5. [Installation & Setup](#installation--setup)
6. [Usage](#usage)
7. [Code Linting & Style](#code-linting--style)
8. [Testing](#testing)
9. [CI/CD & Deployment](#cicd--deployment)
10. [Architecture & Directory Structure](#architecture--directory-structure)
11. [Query Specification Pattern](#query-specification-pattern)
11. [Roadmap](#roadmap)
12. [Contributing](#contributing)
13. [License](#license)
14. [Acknowledgements & Contact](#acknowledgements--contact)

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
  - Planned: Terraform, AWS, GitHub Actions, Prometheus/Grafana

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

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+ & npm](https://nodejs.org/)
- [Docker Engine & Docker Compose](https://docs.docker.com/)
- (Optional) [Postman](https://www.postman.com/) for manual testing

---

## Installation & Setup

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

### Future additions (Roadmap)

- Automated deployments to AWS using Terraform.

- Observability tooling (metrics, logging, tracing) setup with Prometheus/Grafana.

- Kubernetes deployments.

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
└── README.md
```

- **API/** — .NET 8 Web API project, contains controllers, configuration, and entry point.
- **Core/** — Domain layer with entity definitions and repository interfaces.
- **Infrastructure/** — EF Core implementation: configurations, DbContext & seeding, migrations, and repository classes.
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

 - Infrastructure as Code examples (Terraform)

 - AWS provisioning & deployments

 - GitHub Actions CI/CD pipeline (build/test/lint, Docker publishing)

 - Observability training (metrics, logs, tracing)

 - Kubernetes deployment demo

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
