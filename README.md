# DevOps Lab App

> A full-stack reference application for DevOps training and showcasing .NET backend skills.

---

## Table of Contents

1. [Project Overview](#project-overview)
2. [Tech Stack](#tech-stack)
3. [Features](#features)
4. [Prerequisites](#prerequisites)
5. [Installation & Setup](#installation--setup)
6. [Usage](#usage)
7. [Code Linting and Style](#code-linting--style)
8. [Testing](#testing)
9. [CI/CD & Deployment](#cicd--deployment)
10. [Architecture & Directory Structure](#architecture--directory-structure)
11. [Roadmap](#roadmap)
12. [Contributing](#contributing)
13. [License](#license)
14. [Acknowledgements & Contact](#acknowledgements--contact)

---

## Project Overview

**DevOps Lab App** is a simple e-commerce–style reference application designed primarily for DevOps training. It also doubles as a showcase of modern backend development practices with .NET 8 and EF Core. Future modules will demonstrate Infrastructure as Code (IaC), Terraform, AWS deployments, observability dashboards, and CI/CD pipelines.

- **Repo:** https://github.com/infraforge-dev/devops-lab-app
- **Public Postman Workspace:** https://web.postman.co/workspace/f3d2aeac-ae77-4cee-91f9-6cddc12df833

---

## Tech Stack

- **Backend:**
  - .NET 8 Web API
  - Entity Framework Core (SQLite provider)
- **Frontend:**
  - Angular 18
  - TypeScript
  - Tailwind CSS
- **DevOps & Tooling:**
  - Docker Compose (SQLite, API container)
  - Postman (API testing)
  - Planned: Terraform, AWS, GitHub Actions, Prometheus/Grafana

---

## Features

- **Product Catalog API**
  - CRUD endpoints for products
  - EF Core code-first migrations and seeding
- **Dockerized Local Environment**
  - One-command spin-up with `docker-compose up`
- **API Testing Suite**
  - Postman collection with automated tests
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

1. **Clone this repository**
   ```bash
   git clone https://github.com/infraforge-dev/devops-lab-app.git
   cd devops-lab-app
   ```

---

## Usage
**API Endpoints**

- GET /api/products

- GET /api/products/{id}

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

This section will evolve as pipelines are added. Planned workflows:

- GitHub Actions

    - Build & test on PR.

    - Publish Docker image on merge to main.

- Terraform

    - Provision AWS ECS cluster and RDS instance.

- Observability

    - Deploy Prometheus/Grafana to monitor API metrics and logs.

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

## Roadmap

 - Infrastructure as Code examples (Terraform)

 - AWS provisioning & deployments

 - GitHub Actions CI/CD pipeline

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
