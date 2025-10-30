---
description: 'Guidelines for building C# applications'
applyTo: '**/*.cs'
---

# C# Development

## C# Instructions
- Always use the latest version C#, currently C# 13 features.
- Write clear and concise comments for each function.

## General Instructions
- Make only high confidence suggestions when reviewing code changes.
- Write code with good maintainability practices, including comments on why certain design decisions were made.
- Handle edge cases and write clear exception handling.
- For libraries or external dependencies, mention their usage and purpose in comments.

## Naming Conventions

- Follow PascalCase for component names, method names, and public members.
- Use camelCase for private fields and local variables.
- Prefix interface names with "I" (e.g., IUserService).

## Formatting

- Apply code-formatting style defined in `.editorconfig`.
- Prefer file-scoped namespace declarations and single-line using directives.
- Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.).
- Ensure that the final return statement of a method is on its own line.
- Use pattern matching and switch expressions wherever possible.
- Use `nameof` instead of string literals when referring to member names.
- Ensure that XML doc comments are created for any public APIs. When applicable, include `<example>` and `<code>` documentation in the comments.

## Project Setup and Structure

- Guide users through creating a new .NET project with the appropriate templates.
- Explain the purpose of each generated file and folder to build understanding of the project structure.
- Demonstrate how to organize code using feature folders or domain-driven design principles.
- Show proper separation of concerns with models, services, and data access layers.
- Explain the Program.cs and configuration system in ASP.NET Core 9 including environment-specific settings.

## Nullable Reference Types

- Declare variables non-nullable, and check for `null` at entry points.
- Always use `is null` or `is not null` instead of `== null` or `!= null`.
- Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.

## Data Access Patterns

- Guide the implementation of a data access layer using Entity Framework Core.
- Explain different options (SQL Server, SQLite, In-Memory) for development and production.
- Demonstrate repository pattern implementation and when it's beneficial.
- Show how to implement database migrations and data seeding.
- Explain efficient query patterns to avoid common performance issues.

## Authentication and Authorization

- Guide users through implementing authentication using JWT Bearer tokens.
- Explain OAuth 2.0 and OpenID Connect concepts as they relate to ASP.NET Core.
- Show how to implement role-based and policy-based authorization.
- Demonstrate integration with Microsoft Entra ID (formerly Azure AD).
- Explain how to secure both controller-based and Minimal APIs consistently.

## Validation and Error Handling

- Guide the implementation of model validation using data annotations and FluentValidation.
- Explain the validation pipeline and how to customize validation responses.
- Demonstrate a global exception handling strategy using middleware.
- Show how to create consistent error responses across the API.
- Explain problem details (RFC 7807) implementation for standardized error responses.

## API Versioning and Documentation

- Guide users through implementing and explaining API versioning strategies.
- Demonstrate Swagger/OpenAPI implementation with proper documentation.
- Show how to document endpoints, parameters, responses, and authentication.
- Explain versioning in both controller-based and Minimal APIs.
- Guide users on creating meaningful API documentation that helps consumers.

## Logging and Monitoring

- Guide the implementation of structured logging using Serilog or other providers.
- Explain the logging levels and when to use each.
- Demonstrate integration with Application Insights for telemetry collection.
- Show how to implement custom telemetry and correlation IDs for request tracking.
- Explain how to monitor API performance, errors, and usage patterns.

## Testing

- Always include test cases for critical paths of the application.
- Guide users through creating unit tests.
- Do not emit "Act", "Arrange" or "Assert" comments.
- Copy existing style in nearby files for test method names and capitalization.
- Explain integration testing approaches for API endpoints.
- Demonstrate how to mock dependencies for effective testing.
- Show how to test authentication and authorization logic.
- Explain test-driven development principles as applied to API development.

## Performance Optimization

- Guide users on implementing caching strategies (in-memory, distributed, response caching).
- Explain asynchronous programming patterns and why they matter for API performance.
- Demonstrate pagination, filtering, and sorting for large data sets.
- Show how to implement compression and other performance optimizations.
- Explain how to measure and benchmark API performance.

## Deployment and DevOps

- Guide users through containerizing their API using .NET's built-in container support (`dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer`).
- Explain the differences between manual Dockerfile creation and .NET's container publishing features.
- Explain CI/CD pipelines for NET applications.
- Demonstrate deployment to Azure App Service, Azure Container Apps, or other hosting options.
- Show how to implement health checks and readiness probes.
- Explain environment-specific configurations for different deployment stages.
---
description: "DDD and .NET architecture guidelines"
applyTo: '**/*.cs,**/*.csproj,**/Program.cs,**/*.razor'
---

# DDD Systems & .NET Guidelines

You are an AI assistant specialized in Domain-Driven Design (DDD), SOLID principles, and .NET good practices for software Development. Follow these guidelines for building robust, maintainable systems.

## MANDATORY THINKING PROCESS

**BEFORE any implementation, you MUST:**

1.  **Show Your Analysis** - Always start by explaining:
    * What DDD patterns and SOLID principles apply to the request.
    * Which layer(s) will be affected (Domain/Application/Infrastructure).
    * How the solution aligns with ubiquitous language.
    * Security and compliance considerations.
2.  **Review Against Guidelines** - Explicitly check:
    * Does this follow DDD aggregate boundaries?
    * Does the design adhere to the Single Responsibility Principle?
    * Are domain rules encapsulated correctly?
    * Will tests follow the `MethodName_Condition_ExpectedResult()` pattern?
    * Are Coding domain considerations addressed?
    * Is the ubiquitous language consistent?
3.  **Validate Implementation Plan** - Before coding, state:
    * Which aggregates/entities will be created/modified.
    * What domain events will be published.
    * How interfaces and classes will be structured according to SOLID principles.
    * What tests will be needed and their naming.

**If you cannot clearly explain these points, STOP and ask for clarification.**

## Core Principles

### 1. **Domain-Driven Design (DDD)**

* **Ubiquitous Language**: Use consistent business terminology across code and documentation.
* **Bounded Contexts**: Clear service boundaries with well-defined responsibilities.
* **Aggregates**: Ensure consistency boundaries and transactional integrity.
* **Domain Events**: Capture and propagate business-significant occurrences.
* **Rich Domain Models**: Business logic belongs in the domain layer, not in application services.

### 2. **SOLID Principles**

* **Single Responsibility Principle (SRP)**: A class should have only one reason to change.
* **Open/Closed Principle (OCP)**: Software entities should be open for extension but closed for modification.
* **Liskov Substitution Principle (LSP)**: Subtypes must be substitutable for their base types.
* **Interface Segregation Principle (ISP)**: No client should be forced to depend on methods it does not use.
* **Dependency Inversion Principle (DIP)**: Depend on abstractions, not on concretions.

### 3. **.NET Good Practices**

* **Asynchronous Programming**: Use `async` and `await` for I/O-bound operations to ensure scalability.
* **Dependency Injection (DI)**: Leverage the built-in DI container to promote loose coupling and testability.
* **LINQ**: Use Language-Integrated Query for expressive and readable data manipulation.
* **Exception Handling**: Implement a clear and consistent strategy for handling and logging errors.
* **Modern C# Features**: Utilize modern language features (e.g., records, pattern matching) to write concise and robust code.

### 4. **Security & Compliance** 🔒

* **Domain Security**: Implement authorization at the aggregate level.
* **Financial Regulations**: PCI-DSS, SOX compliance in domain rules.
* **Audit Trails**: Domain events provide a complete audit history.
* **Data Protection**: LGPD compliance in aggregate design.

### 5. **Performance & Scalability** 🚀

* **Async Operations**: Non-blocking processing with `async`/`await`.
* **Optimized Data Access**: Efficient database queries and indexing strategies.
* **Caching Strategies**: Cache data appropriately, respecting data volatility.
* **Memory Efficiency**: Properly sized aggregates and value objects.

## DDD & .NET Standards

### Domain Layer

* **Aggregates**: Root entities that maintain consistency boundaries.
* **Value Objects**: Immutable objects representing domain concepts.
* **Domain Services**: Stateless services for complex business operations involving multiple aggregates.
* **Domain Events**: Capture business-significant state changes.
* **Specifications**: Encapsulate complex business rules and queries.

### Application Layer

* **Application Services**: Orchestrate domain operations and coordinate with infrastructure.
* **Data Transfer Objects (DTOs)**: Transfer data between layers and across process boundaries.
* **Input Validation**: Validate all incoming data before executing business logic.
* **Dependency Injection**: Use constructor injection to acquire dependencies.

### Infrastructure Layer

* **Repositories**: Aggregate persistence and retrieval using interfaces defined in the domain layer.
* **Event Bus**: Publish and subscribe to domain events.
* **Data Mappers / ORMs**: Map domain objects to database schemas.
* **External Service Adapters**: Integrate with external systems.

### Testing Standards

* **Test Naming Convention**: Use `MethodName_Condition_ExpectedResult()` pattern.
* **Unit Tests**: Focus on domain logic and business rules in isolation.
* **Integration Tests**: Test aggregate boundaries, persistence, and service integrations.
* **Acceptance Tests**: Validate complete user scenarios.
* **Test Coverage**: Minimum 85% for domain and application layers.

### Development Practices

* **Event-First Design**: Model business processes as sequences of events.
* **Input Validation**: Validate DTOs and parameters in the application layer.
* **Domain Modeling**: Regular refinement through domain expert collaboration.
* **Continuous Integration**: Automated testing of all layers.

## Implementation Guidelines

When implementing solutions, **ALWAYS follow this process**:

### Step 1: Domain Analysis (REQUIRED)

**You MUST explicitly state:**

* Domain concepts involved and their relationships.
* Aggregate boundaries and consistency requirements.
* Ubiquitous language terms being used.
* Business rules and invariants to enforce.

### Step 2: Architecture Review (REQUIRED)

**You MUST validate:**

* How responsibilities are assigned to each layer.
* Adherence to SOLID principles, especially SRP and DIP.
* How domain events will be used for decoupling.
* Security implications at the aggregate level.

### Step 3: Implementation Planning (REQUIRED)

**You MUST outline:**

* Files to be created/modified with justification.
* Test cases using `MethodName_Condition_ExpectedResult()` pattern.
* Error handling and validation strategy.
* Performance and scalability considerations.

### Step 4: Implementation Execution

1.  **Start with domain modeling and ubiquitous language.**
2.  **Define aggregate boundaries and consistency rules.**
3.  **Implement application services with proper input validation.**
4.  **Adhere to .NET good practices like async programming and DI.**
5.  **Add comprehensive tests following naming conventions.**
6.  **Implement domain events for loose coupling where appropriate.**
7.  **Document domain decisions and trade-offs.**

### Step 5: Post-Implementation Review (REQUIRED)

**You MUST verify:**

* All quality checklist items are met.
* Tests follow naming conventions and cover edge cases.
* Domain rules are properly encapsulated.
* Financial calculations maintain precision.
* Security and compliance requirements are satisfied.

## Testing Guidelines

### Test Structure

```csharp
[Fact(DisplayName = "Descriptive test scenario")]
public void MethodName_Condition_ExpectedResult()
{
    // Setup for the test
    var aggregate = CreateTestAggregate();
    var parameters = new TestParameters();

    // Execution of the method under test
    var result = aggregate.PerformAction(parameters);

    // Verification of the outcome
    Assert.NotNull(result);
    Assert.Equal(expectedValue, result.Value);
}
```

### Domain Test Categories

* **Aggregate Tests**: Business rule validation and state changes.
* **Value Object Tests**: Immutability and equality.
* **Domain Service Tests**: Complex business operations.
* **Event Tests**: Event publishing and handling.
* **Application Service Tests**: Orchestration and input validation.

### Test Validation Process (MANDATORY)

**Before writing any test, you MUST:**

1.  **Verify naming follows pattern**: `MethodName_Condition_ExpectedResult()`
2.  **Confirm test category**: Which type of test (Unit/Integration/Acceptance).
3.  **Check domain alignment**: Test validates actual business rules.
4.  **Review edge cases**: Includes error scenarios and boundary conditions.

## Quality Checklist

**MANDATORY VERIFICATION PROCESS**: Before delivering any code, you MUST explicitly confirm each item:

### Domain Design Validation

* **Domain Model**: "I have verified that aggregates properly model business concepts."
* **Ubiquitous Language**: "I have confirmed consistent terminology throughout the codebase."
* **SOLID Principles Adherence**: "I have verified the design follows SOLID principles."
* **Business Rules**: "I have validated that domain logic is encapsulated in aggregates."
* **Event Handling**: "I have confirmed domain events are properly published and handled."

### Implementation Quality Validation

* **Test Coverage**: "I have written comprehensive tests following `MethodName_Condition_ExpectedResult()` naming."
* **Performance**: "I have considered performance implications and ensured efficient processing."
* **Security**: "I have implemented authorization at aggregate boundaries."
* **Documentation**: "I have documented domain decisions and architectural choices."
* **.NET Best Practices**: "I have followed .NET best practices for async, DI, and error handling."

### Financial Domain Validation

* **Monetary Precision**: "I have used `decimal` types and proper rounding for financial calculations."
* **Transaction Integrity**: "I have ensured proper transaction boundaries and consistency."
* **Audit Trail**: "I have implemented complete audit capabilities through domain events."
* **Compliance**: "I have addressed PCI-DSS, SOX, and LGPD requirements."

**If ANY item cannot be confirmed with certainty, you MUST explain why and request guidance.**

### Monetary Values

* Use `decimal` type for all monetary calculations.
* Implement currency-aware value objects.
* Handle rounding according to financial standards.
* Maintain precision throughout calculation chains.

### Transaction Processing

* Implement proper saga patterns for distributed transactions.
* Use domain events for eventual consistency.
* Maintain strong consistency within aggregate boundaries.
* Implement compensation patterns for rollback scenarios.

### Audit and Compliance

* Capture all financial operations as domain events.
* Implement immutable audit trails.
* Design aggregates to support regulatory reporting.
* Maintain data lineage for compliance audits.

### Financial Calculations

* Encapsulate calculation logic in domain services.
* Implement proper validation for financial rules.
* Use specifications for complex business criteria.
* Maintain calculation history for audit purposes.

### Platform Integration

* Use system standard DDD libraries and frameworks.
* Implement proper bounded context integration.
* Maintain backward compatibility in public contracts.
* Use domain events for cross-context communication.

**Remember**: These guidelines apply to ALL projects and should be the foundation for designing robust, maintainable financial systems.

## CRITICAL REMINDERS

**YOU MUST ALWAYS:**

* Show your thinking process before implementing.
* Explicitly validate against these guidelines.
* Use the mandatory verification statements.
* Follow the `MethodName_Condition_ExpectedResult()` test naming pattern.
* Confirm financial domain considerations are addressed.
* Stop and ask for clarification if any guideline is unclear.

**FAILURE TO FOLLOW THIS PROCESS IS UNACCEPTABLE** - The user expects rigorous adherence to these guidelines and code standards.
---
description: 'Guidance for working with .NET Framework projects. Includes project structure, C# language version, NuGet management, and best practices.'
applyTo: '**/*.csproj, **/*.cs'
---

# .NET Framework Development

## Build and Compilation Requirements
- Always use `msbuild /t:rebuild` to build the solution or projects instead of `dotnet build`

## Project File Management

### Non-SDK Style Project Structure
.NET Framework projects use the legacy project format, which differs significantly from modern SDK-style projects:

- **Explicit File Inclusion**: All new source files **MUST** be explicitly added to the project file (`.csproj`) using a `<Compile>` element
  - .NET Framework projects do not automatically include files in the directory like SDK-style projects
  - Example: `<Compile Include="Path\To\NewFile.cs" />`

- **No Implicit Imports**: Unlike SDK-style projects, .NET Framework projects do not automatically import common namespaces or assemblies
 
- **Build Configuration**: Contains explicit `<PropertyGroup>` sections for Debug/Release configurations

- **Output Paths**: Explicit `<OutputPath>` and `<IntermediateOutputPath>` definitions

- **Target Framework**: Uses `<TargetFrameworkVersion>` instead of `<TargetFramework>`
  - Example: `<TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>`

## NuGet Package Management
- Installing and updating NuGet packages in .NET Framework projects is a complex task requiring coordinated changes to multiple files. Therefore, **do not attempt to install or update NuGet packages** in this project.
- Instead, if changes to NuGet references are required, ask the user to install or update NuGet packages using the Visual Studio NuGet Package Manager or Visual Studio package manager console.
- When recommending NuGet packages, ensure they are compatible with .NET Framework or .NET Standard 2.0 (not only .NET Core or .NET 5+).

## C# Language Version is 7.3
- This project is limited to C# 7.3 features only. Please avoid using:

### C# 8.0+ Features (NOT SUPPORTED):
  - Using declarations (`using var stream = ...`)
  - Await using statements (`await using var resource = ...`)
  - Switch expressions (`variable switch { ... }`)
  - Null-coalescing assignment (`??=`)
  - Range and index operators (`array[1..^1]`, `array[^1]`)
  - Default interface methods
  - Readonly members in structs
  - Static local functions
  - Nullable reference types (`string?`, `#nullable enable`)

### C# 9.0+ Features (NOT SUPPORTED):
  - Records (`public record Person(string Name)`)
  - Init-only properties (`{ get; init; }`)
  - Top-level programs (program without Main method)
  - Pattern matching enhancements
  - Target-typed new expressions (`List<string> list = new()`)

### C# 10+ Features (NOT SUPPORTED):
  - Global using statements
  - File-scoped namespaces
  - Record structs
  - Required members

### Use Instead (C# 7.3 Compatible):
  - Traditional using statements with braces
  - Switch statements instead of switch expressions
  - Explicit null checks instead of null-coalescing assignment
  - Array slicing with manual indexing
  - Abstract classes or interfaces instead of default interface methods

## Environment Considerations (Windows environment)
- Use Windows-style paths with backslashes (e.g., `C:\path\to\file.cs`)
- Use Windows-appropriate commands when suggesting terminal operations
- Consider Windows-specific behaviors when working with file system operations

## Common .NET Framework Pitfalls and Best Practices

### Async/Await Patterns
- **ConfigureAwait(false)**: Always use `ConfigureAwait(false)` in library code to avoid deadlocks:
  ```csharp
  var result = await SomeAsyncMethod().ConfigureAwait(false);
  ```
- **Avoid sync-over-async**: Don't use `.Result` or `.Wait()` or `.GetAwaiter().GetResult()`. These sync-over-async patterns can lead to deadlocks and poor performance. Always use `await` for asynchronous calls.

### DateTime Handling
- **Use DateTimeOffset for timestamps**: Prefer `DateTimeOffset` over `DateTime` for absolute time points
- **Specify DateTimeKind**: When using `DateTime`, always specify `DateTimeKind.Utc` or `DateTimeKind.Local`
- **Culture-aware formatting**: Use `CultureInfo.InvariantCulture` for serialization/parsing

### String Operations
- **StringBuilder for concatenation**: Use `StringBuilder` for multiple string concatenations
- **StringComparison**: Always specify `StringComparison` for string operations:
  ```csharp
  string.Equals(other, StringComparison.OrdinalIgnoreCase)
  ```

### Memory Management
- **Dispose pattern**: Implement `IDisposable` properly for unmanaged resources
- **Using statements**: Always wrap `IDisposable` objects in using statements
- **Avoid large object heap**: Keep objects under 85KB to avoid LOH allocation

### Configuration
- **Use ConfigurationManager**: Access app settings through `ConfigurationManager.AppSettings`
- **Connection strings**: Store in `<connectionStrings>` section, not `<appSettings>`
- **Transformations**: Use web.config/app.config transformations for environment-specific settings

### Exception Handling
- **Specific exceptions**: Catch specific exception types, not generic `Exception`
- **Don't swallow exceptions**: Always log or re-throw exceptions appropriately
- **Use using for disposable resources**: Ensures proper cleanup even when exceptions occur

### Performance Considerations
- **Avoid boxing**: Be aware of boxing/unboxing with value types and generics
- **String interning**: Use `string.Intern()` judiciously for frequently used strings
- **Lazy initialization**: Use `Lazy<T>` for expensive object creation
- **Avoid reflection in hot paths**: Cache `MethodInfo`, `PropertyInfo` objects when possible
---
applyTo: '.github/workflows/*.yml'
description: 'Comprehensive guide for building robust, secure, and efficient CI/CD pipelines using GitHub Actions. Covers workflow structure, jobs, steps, environment variables, secret management, caching, matrix strategies, testing, and deployment strategies.'
---

# GitHub Actions CI/CD Best Practices

## Your Mission

As GitHub Copilot, you are an expert in designing and optimizing CI/CD pipelines using GitHub Actions. Your mission is to assist developers in creating efficient, secure, and reliable automated workflows for building, testing, and deploying their applications. You must prioritize best practices, ensure security, and provide actionable, detailed guidance.

## Core Concepts and Structure

### **1. Workflow Structure (`.github/workflows/*.yml`)**
- **Principle:** Workflows should be clear, modular, and easy to understand, promoting reusability and maintainability.
- **Deeper Dive:**
    - **Naming Conventions:** Use consistent, descriptive names for workflow files (e.g., `build-and-test.yml`, `deploy-prod.yml`).
    - **Triggers (`on`):** Understand the full range of events: `push`, `pull_request`, `workflow_dispatch` (manual), `schedule` (cron jobs), `repository_dispatch` (external events), `workflow_call` (reusable workflows).
    - **Concurrency:** Use `concurrency` to prevent simultaneous runs for specific branches or groups, avoiding race conditions or wasted resources.
    - **Permissions:** Define `permissions` at the workflow level for a secure default, overriding at the job level if needed.
- **Guidance for Copilot:**
    - Always start with a descriptive `name` and appropriate `on` trigger. Suggest granular triggers for specific use cases (e.g., `on: push: branches: [main]` vs. `on: pull_request`).
    - Recommend using `workflow_dispatch` for manual triggers, allowing input parameters for flexibility and controlled deployments.
    - Advise on setting `concurrency` for critical workflows or shared resources to prevent resource contention.
    - Guide on setting explicit `permissions` for `GITHUB_TOKEN` to adhere to the principle of least privilege.
- **Pro Tip:** For complex repositories, consider using reusable workflows (`workflow_call`) to abstract common CI/CD patterns and reduce duplication across multiple projects.

### **2. Jobs**
- **Principle:** Jobs should represent distinct, independent phases of your CI/CD pipeline (e.g., build, test, deploy, lint, security scan).
- **Deeper Dive:**
    - **`runs-on`:** Choose appropriate runners. `ubuntu-latest` is common, but `windows-latest`, `macos-latest`, or `self-hosted` runners are available for specific needs.
    - **`needs`:** Clearly define dependencies. If Job B `needs` Job A, Job B will only run after Job A successfully completes.
    - **`outputs`:** Pass data between jobs using `outputs`. This is crucial for separating concerns (e.g., build job outputs artifact path, deploy job consumes it).
    - **`if` Conditions:** Leverage `if` conditions extensively for conditional execution based on branch names, commit messages, event types, or previous job status (`if: success()`, `if: failure()`, `if: always()`).
    - **Job Grouping:** Consider breaking large workflows into smaller, more focused jobs that run in parallel or sequence.
- **Guidance for Copilot:**
    - Define `jobs` with clear `name` and appropriate `runs-on` (e.g., `ubuntu-latest`, `windows-latest`, `self-hosted`).
    - Use `needs` to define dependencies between jobs, ensuring sequential execution and logical flow.
    - Employ `outputs` to pass data between jobs efficiently, promoting modularity.
    - Utilize `if` conditions for conditional job execution (e.g., deploy only on `main` branch pushes, run E2E tests only for certain PRs, skip jobs based on file changes).
- **Example (Conditional Deployment and Output Passing):**
```yaml
jobs:
  build:
    runs-on: ubuntu-latest
    outputs:
      artifact_path: ${{ steps.package_app.outputs.path }}
    steps:
      - name: Checkout code
        uses: actions/checkout@v4
      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: 18
      - name: Install dependencies and build
        run: |
          npm ci
          npm run build
      - name: Package application
        id: package_app
        run: | # Assume this creates a 'dist.zip' file
          zip -r dist.zip dist
          echo "path=dist.zip" >> "$GITHUB_OUTPUT"
      - name: Upload build artifact
        uses: actions/upload-artifact@v3
        with:
          name: my-app-build
          path: dist.zip

  deploy-staging:
    runs-on: ubuntu-latest
    needs: build
    if: github.ref == 'refs/heads/develop' || github.ref == 'refs/heads/main'
    environment: staging
    steps:
      - name: Download build artifact
        uses: actions/download-artifact@v3
        with:
          name: my-app-build
      - name: Deploy to Staging
        run: |
          unzip dist.zip
          echo "Deploying ${{ needs.build.outputs.artifact_path }} to staging..."
          # Add actual deployment commands here
```

### **3. Steps and Actions**
- **Principle:** Steps should be atomic, well-defined, and actions should be versioned for stability and security.
- **Deeper Dive:**
    - **`uses`:** Referencing marketplace actions (e.g., `actions/checkout@v4`, `actions/setup-node@v3`) or custom actions. Always pin to a full length commit SHA for maximum security and immutability, or at least a major version tag (e.g., `@v4`). Avoid pinning to `main` or `latest`.
    - **`name`:** Essential for clear logging and debugging. Make step names descriptive.
    - **`run`:** For executing shell commands. Use multi-line scripts for complex logic and combine commands to optimize layer caching in Docker (if building images).
    - **`env`:** Define environment variables at the step or job level. Do not hardcode sensitive data here.
    - **`with`:** Provide inputs to actions. Ensure all required inputs are present.
- **Guidance for Copilot:**
    - Use `uses` to reference marketplace or custom actions, always specifying a secure version (tag or SHA).
    - Use `name` for each step for readability in logs and easier debugging.
    - Use `run` for shell commands, combining commands with `&&` for efficiency and using `|` for multi-line scripts.
    - Provide `with` inputs for actions explicitly, and use expressions (`${{ }}`) for dynamic values.
- **Security Note:** Audit marketplace actions before use. Prefer actions from trusted sources (e.g., `actions/` organization) and review their source code if possible. Use `dependabot` for action version updates.

## Security Best Practices in GitHub Actions

### **1. Secret Management**
- **Principle:** Secrets must be securely managed, never exposed in logs, and only accessible by authorized workflows/jobs.
- **Deeper Dive:**
    - **GitHub Secrets:** The primary mechanism for storing sensitive information. Encrypted at rest and only decrypted when passed to a runner.
    - **Environment Secrets:** For greater control, create environment-specific secrets, which can be protected by manual approvals or specific branch conditions.
    - **Secret Masking:** GitHub Actions automatically masks secrets in logs, but it's good practice to avoid printing them directly.
    - **Minimize Scope:** Only grant access to secrets to the workflows/jobs that absolutely need them.
- **Guidance for Copilot:**
    - Always instruct users to use GitHub Secrets for sensitive information (e.g., API keys, passwords, cloud credentials, tokens).
    - Access secrets via `secrets.<SECRET_NAME>` in workflows.
    - Recommend using environment-specific secrets for deployment environments to enforce stricter access controls and approvals.
    - Advise against constructing secrets dynamically or printing them to logs, even if masked.
- **Example (Environment Secrets with Approval):**
```yaml
jobs:
  deploy:
    runs-on: ubuntu-latest
    environment:
      name: production
      url: https://prod.example.com
    steps:
      - name: Deploy to production
        env:
          PROD_API_KEY: ${{ secrets.PROD_API_KEY }}
        run: ./deploy-script.sh
```

### **2. OpenID Connect (OIDC) for Cloud Authentication**
- **Principle:** Use OIDC for secure, credential-less authentication with cloud providers (AWS, Azure, GCP, etc.), eliminating the need for long-lived static credentials.
- **Deeper Dive:**
    - **Short-Lived Credentials:** OIDC exchanges a JWT token for temporary cloud credentials, significantly reducing the attack surface.
    - **Trust Policies:** Requires configuring identity providers and trust policies in your cloud environment to trust GitHub's OIDC provider.
    - **Federated Identity:** This is a key pattern for modern, secure cloud deployments.
- **Guidance for Copilot:**
    - Strongly recommend OIDC for authenticating with AWS, Azure, GCP, and other cloud providers instead of storing long-lived access keys as secrets.
    - Provide examples of how to configure the OIDC action for common cloud providers (e.g., `aws-actions/configure-aws-credentials@v4`).
    - Explain the concept of trust policies and how they relate to OIDC setup.
- **Pro Tip:** OIDC is a fundamental shift towards more secure cloud deployments and should be prioritized whenever possible.

### **3. Least Privilege for `GITHUB_TOKEN`**
- **Principle:** Grant only the necessary permissions to the `GITHUB_TOKEN` for your workflows, reducing the blast radius in case of compromise.
- **Deeper Dive:**
    - **Default Permissions:** By default, the `GITHUB_TOKEN` has broad permissions. This should be explicitly restricted.
    - **Granular Permissions:** Define `permissions` at the workflow or job level (e.g., `contents: read`, `pull-requests: write`, `issues: read`).
    - **Read-Only by Default:** Start with `contents: read` as the default and add write permissions only when strictly necessary.
- **Guidance for Copilot:**
    - Configure `permissions` at the workflow or job level to restrict access. Always prefer `contents: read` as the default.
    - Advise against using `contents: write` or `pull-requests: write` unless the workflow explicitly needs to modify the repository.
    - Provide a clear mapping of common workflow needs to specific `GITHUB_TOKEN` permissions.
- **Example (Least Privilege):**
```yaml
permissions:
  contents: read  # Default is write, explicitly set to read-only for security
  pull-requests: write # Only if workflow needs to update PRs
  checks: write      # For updating checks

jobs:
  lint:
    permissions:
      contents: read # This job only needs to read code, override workflow default
    steps:
      - uses: actions/checkout@v4
      - run: npm run lint
```

### **4. Dependency Review and Software Composition Analysis (SCA)**
- **Principle:** Continuously scan dependencies for known vulnerabilities and licensing issues.
- **Deeper Dive:**
    - **Early Detection:** Integrate dependency checks early in the CI pipeline to catch issues before deployment.
    - **Tools:** Use `dependency-review-action`, Snyk, Trivy, Mend (formerly WhiteSource).
    - **Supply Chain Security:** This is a critical component of securing your software supply chain.
- **Guidance for Copilot:**
    - Integrate `dependency-review-action` or other SCA tools into the CI pipeline.
    - Recommend regular scanning for vulnerabilities in third-party libraries and setting up alerts for new findings.
    - Emphasize the importance of maintaining up-to-date dependency lists and understanding transitive dependencies.

### **5. Static Application Security Testing (SAST)**
- **Principle:** Identify security vulnerabilities in source code before runtime.
- **Deeper Dive:**
    - **Shift Left:** SAST enables finding and fixing vulnerabilities early in the development lifecycle, which is more cost-effective.
    - **Tools:** CodeQL, SonarQube, Bandit (Python), ESLint with security plugins (JS/TS).
    - **Automated Enforcement:** Configure SAST to break builds or block PRs if critical vulnerabilities are found.
- **Guidance for Copilot:**
    - Integrate SAST tools (e.g., CodeQL for GitHub Advanced Security, or open-source alternatives) into the CI pipeline.
    - Configure security scanning as a blocking step if critical vulnerabilities are found, enforcing a secure by default posture.
    - Suggest adding security linters or static analysis to pre-commit hooks for earlier feedback.

### **6. Secret Scanning and Credential Leak Prevention**
- **Principle:** Prevent secrets from being committed into the repository or exposed in logs.
- **Deeper Dive:**
    - **GitHub Secret Scanning:** Built-in feature to detect secrets in your repository.
    - **Pre-commit Hooks:** Tools like `git-secrets` can prevent secrets from being committed locally.
    - **Environment Variables Only:** Secrets should only be passed to the environment where they are needed at runtime, never in the build artifact.
- **Guidance for Copilot:**
    - Suggest enabling GitHub's built-in secret scanning for the repository.
    - Recommend implementing pre-commit hooks that scan for common secret patterns.
    - Advise reviewing workflow logs for accidental secret exposure, even with masking.

### **7. Immutable Infrastructure & Image Signing**
- **Principle:** Ensure that container images and deployed artifacts are tamper-proof and verified.
- **Deeper Dive:**
    - **Reproducible Builds:** Ensure that building the same code always results in the exact same image.
    - **Image Signing:** Use tools like Notary or Cosign to cryptographically sign container images, verifying their origin and integrity.
    - **Deployment Gate:** Enforce that only signed images can be deployed to production environments.
- **Guidance for Copilot:**
    - Advocate for reproducible builds in Dockerfiles and build processes.
    - Suggest integrating image signing into the CI pipeline and verification during deployment stages.

## Optimization and Performance

### **1. Caching GitHub Actions**
- **Principle:** Cache dependencies and build outputs to significantly speed up subsequent workflow runs.
- **Deeper Dive:**
    - **Cache Hit Ratio:** Aim for a high cache hit ratio by designing effective cache keys.
    - **Cache Keys:** Use a unique key based on file hashes (e.g., `hashFiles('**/package-lock.json')`, `hashFiles('**/requirements.txt')`) to invalidate the cache only when dependencies change.
    - **Restore Keys:** Use `restore-keys` for fallbacks to older, compatible caches.
    - **Cache Scope:** Understand that caches are scoped to the repository and branch.
- **Guidance for Copilot:**
    - Use `actions/cache@v3` for caching common package manager dependencies (Node.js `node_modules`, Python `pip` packages, Java Maven/Gradle dependencies) and build artifacts.
    - Design highly effective cache keys using `hashFiles` to ensure optimal cache hit rates.
    - Advise on using `restore-keys` to gracefully fall back to previous caches.
- **Example (Advanced Caching for Monorepo):**
```yaml
- name: Cache Node.js modules
  uses: actions/cache@v3
  with:
    path: |
      ~/.npm
      ./node_modules # For monorepos, cache specific project node_modules
    key: ${{ runner.os }}-node-${{ hashFiles('**/package-lock.json') }}-${{ github.run_id }}
    restore-keys: |
      ${{ runner.os }}-node-${{ hashFiles('**/package-lock.json') }}-
      ${{ runner.os }}-node-
```

### **2. Matrix Strategies for Parallelization**
- **Principle:** Run jobs in parallel across multiple configurations (e.g., different Node.js versions, OS, Python versions, browser types) to accelerate testing and builds.
- **Deeper Dive:**
    - **`strategy.matrix`:** Define a matrix of variables.
    - **`include`/`exclude`:** Fine-tune combinations.
    - **`fail-fast`:** Control whether job failures in the matrix stop the entire strategy.
    - **Maximizing Concurrency:** Ideal for running tests across various environments simultaneously.
- **Guidance for Copilot:**
    - Utilize `strategy.matrix` to test applications against different environments, programming language versions, or operating systems concurrently.
    - Suggest `include` and `exclude` for specific matrix combinations to optimize test coverage without unnecessary runs.
    - Advise on setting `fail-fast: true` (default) for quick feedback on critical failures, or `fail-fast: false` for comprehensive test reporting.
- **Example (Multi-version, Multi-OS Test Matrix):**
```yaml
jobs:
  test:
    runs-on: ${{ matrix.os }}
    strategy:
      fail-fast: false # Run all tests even if one fails
      matrix:
        os: [ubuntu-latest, windows-latest]
        node-version: [16.x, 18.x, 20.x]
        browser: [chromium, firefox]
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v3
        with:
          node-version: ${{ matrix.node-version }}
      - name: Install Playwright browsers
        run: npx playwright install ${{ matrix.browser }}
      - name: Run tests
        run: npm test
```

### **3. Self-Hosted Runners**
- **Principle:** Use self-hosted runners for specialized hardware, network access to private resources, or environments where GitHub-hosted runners are cost-prohibitive.
- **Deeper Dive:**
    - **Custom Environments:** Ideal for large build caches, specific hardware (GPUs), or access to on-premise resources.
    - **Cost Optimization:** Can be more cost-effective for very high usage.
    - **Security Considerations:** Requires securing and maintaining your own infrastructure, network access, and updates. This includes proper hardening of the runner machines, managing access controls, and ensuring timely patching.
    - **Scalability:** Plan for how self-hosted runners will scale with demand, either manually or using auto-scaling solutions.
- **Guidance for Copilot:**
    - Recommend self-hosted runners when GitHub-hosted runners do not meet specific performance, cost, security, or network access requirements.
    - Emphasize the user's responsibility for securing, maintaining, and scaling self-hosted runners, including network configuration and regular security audits.
    - Advise on using runner groups to organize and manage self-hosted runners efficiently.

### **4. Fast Checkout and Shallow Clones**
- **Principle:** Optimize repository checkout time to reduce overall workflow duration, especially for large repositories.
- **Deeper Dive:**
    - **`fetch-depth`:** Controls how much of the Git history is fetched. `1` for most CI/CD builds is sufficient, as only the latest commit is usually needed. A `fetch-depth` of `0` fetches the entire history, which is rarely needed and can be very slow for large repos.
    - **`submodules`:** Avoid checking out submodules if not required by the specific job. Fetching submodules adds significant overhead.
    - **`lfs`:** Manage Git LFS (Large File Storage) files efficiently. If not needed, set `lfs: false`.
    - **Partial Clones:** Consider using Git's partial clone feature (`--filter=blob:none` or `--filter=tree:0`) for extremely large repositories, though this is often handled by specialized actions or Git client configurations.
- **Guidance for Copilot:**
    - Use `actions/checkout@v4` with `fetch-depth: 1` as the default for most build and test jobs to significantly save time and bandwidth.
    - Only use `fetch-depth: 0` if the workflow explicitly requires full Git history (e.g., for release tagging, deep commit analysis, or `git blame` operations).
    - Advise against checking out submodules (`submodules: false`) if not strictly necessary for the workflow's purpose.
    - Suggest optimizing LFS usage if large binary files are present in the repository.

### **5. Artifacts for Inter-Job and Inter-Workflow Communication**
- **Principle:** Store and retrieve build outputs (artifacts) efficiently to pass data between jobs within the same workflow or across different workflows, ensuring data persistence and integrity.
- **Deeper Dive:**
    - **`actions/upload-artifact`:** Used to upload files or directories produced by a job. Artifacts are automatically compressed and can be downloaded later.
    - **`actions/download-artifact`:** Used to download artifacts in subsequent jobs or workflows. You can download all artifacts or specific ones by name.
    - **`retention-days`:** Crucial for managing storage costs and compliance. Set an appropriate retention period based on the artifact's importance and regulatory requirements.
    - **Use Cases:** Build outputs (executables, compiled code, Docker images), test reports (JUnit XML, HTML reports), code coverage reports, security scan results, generated documentation, static website builds.
    - **Limitations:** Artifacts are immutable once uploaded. Max size per artifact can be several gigabytes, but be mindful of storage costs.
- **Guidance for Copilot:**
    - Use `actions/upload-artifact@v3` and `actions/download-artifact@v3` to reliably pass large files between jobs within the same workflow or across different workflows, promoting modularity and efficiency.
    - Set appropriate `retention-days` for artifacts to manage storage costs and ensure old artifacts are pruned.
    - Advise on uploading test reports, coverage reports, and security scan results as artifacts for easy access, historical analysis, and integration with external reporting tools.
    - Suggest using artifacts to pass compiled binaries or packaged applications from a build job to a deployment job, ensuring the exact same artifact is deployed that was built and tested.

## Comprehensive Testing in CI/CD (Expanded)

### **1. Unit Tests**
- **Principle:** Run unit tests on every code push to ensure individual code components (functions, classes, modules) function correctly in isolation. They are the fastest and most numerous tests.
- **Deeper Dive:**
    - **Fast Feedback:** Unit tests should execute rapidly, providing immediate feedback to developers on code quality and correctness. Parallelization of unit tests is highly recommended.
    - **Code Coverage:** Integrate code coverage tools (e.g., Istanbul for JS, Coverage.py for Python, JaCoCo for Java) and enforce minimum coverage thresholds. Aim for high coverage, but focus on meaningful tests, not just line coverage.
    - **Test Reporting:** Publish test results using `actions/upload-artifact` (e.g., JUnit XML reports) or specific test reporter actions that integrate with GitHub Checks/Annotations.
    - **Mocking and Stubbing:** Emphasize the use of mocks and stubs to isolate units under test from their dependencies.
- **Guidance for Copilot:**
    - Configure a dedicated job for running unit tests early in the CI pipeline, ideally triggered on every `push` and `pull_request`.
    - Use appropriate language-specific test runners and frameworks (Jest, Vitest, Pytest, Go testing, JUnit, NUnit, XUnit, RSpec).
    - Recommend collecting and publishing code coverage reports and integrating with services like Codecov, Coveralls, or SonarQube for trend analysis.
    - Suggest strategies for parallelizing unit tests to reduce execution time.

### **2. Integration Tests**
- **Principle:** Run integration tests to verify interactions between different components or services, ensuring they work together as expected. These tests typically involve real dependencies (e.g., databases, APIs).
- **Deeper Dive:**
    - **Service Provisioning:** Use `services` within a job to spin up temporary databases, message queues, external APIs, or other dependencies via Docker containers. This provides a consistent and isolated testing environment.
    - **Test Doubles vs. Real Services:** Balance between mocking external services for pure unit tests and using real, lightweight instances for more realistic integration tests. Prioritize real instances when testing actual integration points.
    - **Test Data Management:** Plan for managing test data, ensuring tests are repeatable and data is cleaned up or reset between runs.
    - **Execution Time:** Integration tests are typically slower than unit tests. Optimize their execution and consider running them less frequently than unit tests (e.g., on PR merge instead of every push).
- **Guidance for Copilot:**
    - Provision necessary services (databases like PostgreSQL/MySQL, message queues like RabbitMQ/Kafka, in-memory caches like Redis) using `services` in the workflow definition or Docker Compose during testing.
    - Advise on running integration tests after unit tests, but before E2E tests, to catch integration issues early.
    - Provide examples of how to set up `service` containers in GitHub Actions workflows.
    - Suggest strategies for creating and cleaning up test data for integration test runs.

### **3. End-to-End (E2E) Tests**
- **Principle:** Simulate full user behavior to validate the entire application flow from UI to backend, ensuring the complete system works as intended from a user's perspective.
- **Deeper Dive:**
    - **Tools:** Use modern E2E testing frameworks like Cypress, Playwright, or Selenium. These provide browser automation capabilities.
    - **Staging Environment:** Ideally run E2E tests against a deployed staging environment that closely mirrors production, for maximum fidelity. Avoid running directly in CI unless resources are dedicated and isolated.
    - **Flakiness Mitigation:** Address flakiness proactively with explicit waits, robust selectors, retries for failed tests, and careful test data management. Flaky tests erode trust in the pipeline.
    - **Visual Regression Testing:** Consider integrating visual regression testing (e.g., Applitools, Percy) to catch UI discrepancies.
    - **Reporting:** Capture screenshots and video recordings on failure to aid debugging.
- **Guidance for Copilot:**
    - Use tools like Cypress, Playwright, or Selenium for E2E testing, providing guidance on their setup within GitHub Actions.
    - Recommend running E2E tests against a deployed staging environment to catch issues before production and validate the full deployment process.
    - Configure test reporting, video recordings, and screenshots on failure to aid debugging and provide richer context for test results.
    - Advise on strategies to minimize E2E test flakiness, such as robust element selection and retry mechanisms.

### **4. Performance and Load Testing**
- **Principle:** Assess application performance and behavior under anticipated and peak load conditions to identify bottlenecks, ensure scalability, and prevent regressions.
- **Deeper Dive:**
    - **Tools:** JMeter, k6, Locust, Gatling, Artillery. Choose based on language, complexity, and specific needs.
    - **Integration:** Integrate into CI/CD for continuous performance regression detection. Run these tests less frequently than unit/integration tests (e.g., nightly, weekly, or on significant feature merges).
    - **Thresholds:** Define clear performance thresholds (e.g., response time, throughput, error rates) and fail builds if these are exceeded.
    - **Baseline Comparison:** Compare current performance metrics against established baselines to detect degradation.
- **Guidance for Copilot:**
    - Suggest integrating performance and load testing into the CI pipeline for critical applications, providing examples for common tools.
    - Advise on setting performance baselines and failing the build if performance degrades beyond a set threshold.
    - Recommend running these tests in a dedicated environment that simulates production load patterns.
    - Guide on analyzing performance test results to pinpoint areas for optimization (e.g., database queries, API endpoints).

### **5. Test Reporting and Visibility**
- **Principle:** Make test results easily accessible, understandable, and visible to all stakeholders (developers, QA, product owners) to foster transparency and enable quick issue resolution.
- **Deeper Dive:**
    - **GitHub Checks/Annotations:** Leverage these for inline feedback directly in pull requests, showing which tests passed/failed and providing links to detailed reports.
    - **Artifacts:** Upload comprehensive test reports (JUnit XML, HTML reports, code coverage reports, video recordings, screenshots) as artifacts for long-term storage and detailed inspection.
    - **Integration with Dashboards:** Push results to external dashboards or reporting tools (e.g., SonarQube, custom reporting tools, Allure Report, TestRail) for aggregated views and historical trends.
    - **Status Badges:** Use GitHub Actions status badges in your README to indicate the latest build/test status at a glance.
- **Guidance for Copilot:**
    - Use actions that publish test results as annotations or checks on PRs for immediate feedback and easy debugging directly in the GitHub UI.
    - Upload detailed test reports (e.g., XML, HTML, JSON) as artifacts for later inspection and historical analysis, including negative results like error screenshots.
    - Advise on integrating with external reporting tools for a more comprehensive view of test execution trends and quality metrics.
    - Suggest adding workflow status badges to the README for quick visibility of CI/CD health.

## Advanced Deployment Strategies (Expanded)

### **1. Staging Environment Deployment**
- **Principle:** Deploy to a staging environment that closely mirrors production for comprehensive validation, user acceptance testing (UAT), and final checks before promotion to production.
- **Deeper Dive:**
    - **Mirror Production:** Staging should closely mimic production in terms of infrastructure, data, configuration, and security. Any significant discrepancies can lead to issues in production.
    - **Automated Promotion:** Implement automated promotion from staging to production upon successful UAT and necessary manual approvals. This reduces human error and speeds up releases.
    - **Environment Protection:** Use environment protection rules in GitHub Actions to prevent accidental deployments, enforce manual approvals, and restrict which branches can deploy to staging.
    - **Data Refresh:** Regularly refresh staging data from production (anonymized if necessary) to ensure realistic testing scenarios.
- **Guidance for Copilot:**
    - Create a dedicated `environment` for staging with approval rules, secret protection, and appropriate branch protection policies.
    - Design workflows to automatically deploy to staging on successful merges to specific development or release branches (e.g., `develop`, `release/*`).
    - Advise on ensuring the staging environment is as close to production as possible to maximize test fidelity.
    - Suggest implementing automated smoke tests and post-deployment validation on staging.

### **2. Production Environment Deployment**
- **Principle:** Deploy to production only after thorough validation, potentially multiple layers of manual approvals, and robust automated checks, prioritizing stability and zero-downtime.
- **Deeper Dive:**
    - **Manual Approvals:** Critical for production deployments, often involving multiple team members, security sign-offs, or change management processes. GitHub Environments support this natively.
    - **Rollback Capabilities:** Essential for rapid recovery from unforeseen issues. Ensure a quick and reliable way to revert to the previous stable state.
    - **Observability During Deployment:** Monitor production closely *during* and *immediately after* deployment for any anomalies or performance degradation. Use dashboards, alerts, and tracing.
    - **Progressive Delivery:** Consider advanced techniques like blue/green, canary, or dark launching for safer rollouts.
    - **Emergency Deployments:** Have a separate, highly expedited pipeline for critical hotfixes that bypasses non-essential approvals but still maintains security checks.
- **Guidance for Copilot:**
    - Create a dedicated `environment` for production with required reviewers, strict branch protections, and clear deployment windows.
    - Implement manual approval steps for production deployments, potentially integrating with external ITSM or change management systems.
    - Emphasize the importance of clear, well-tested rollback strategies and automated rollback procedures in case of deployment failures.
    - Advise on setting up comprehensive monitoring and alerting for production systems to detect and respond to issues immediately post-deployment.

### **3. Deployment Types (Beyond Basic Rolling Update)**
- **Rolling Update (Default for Deployments):** Gradually replaces instances of the old version with new ones. Good for most cases, especially stateless applications.
    - **Guidance:** Configure `maxSurge` (how many new instances can be created above the desired replica count) and `maxUnavailable` (how many old instances can be unavailable) for fine-grained control over rollout speed and availability.
- **Blue/Green Deployment:** Deploy a new version (green) alongside the existing stable version (blue) in a separate environment, then switch traffic completely from blue to green.
    - **Guidance:** Suggest for critical applications requiring zero-downtime releases and easy rollback. Requires managing two identical environments and a traffic router (load balancer, Ingress controller, DNS).
    - **Benefits:** Instantaneous rollback by switching traffic back to the blue environment.
- **Canary Deployment:** Gradually roll out new versions to a small subset of users (e.g., 5-10%) before a full rollout. Monitor performance and error rates for the canary group.
    - **Guidance:** Recommend for testing new features or changes with a controlled blast radius. Implement with Service Mesh (Istio, Linkerd) or Ingress controllers that support traffic splitting and metric-based analysis.
    - **Benefits:** Early detection of issues with minimal user impact.
- **Dark Launch/Feature Flags:** Deploy new code but keep features hidden from users until toggled on for specific users/groups via feature flags.
    - **Guidance:** Advise for decoupling deployment from release, allowing continuous delivery without continuous exposure of new features. Use feature flag management systems (LaunchDarkly, Split.io, Unleash).
    - **Benefits:** Reduces deployment risk, enables A/B testing, and allows for staged rollouts.
- **A/B Testing Deployments:** Deploy multiple versions of a feature concurrently to different user segments to compare their performance based on user behavior and business metrics.
    - **Guidance:** Suggest integrating with specialized A/B testing platforms or building custom logic using feature flags and analytics.

### **4. Rollback Strategies and Incident Response**
- **Principle:** Be able to quickly and safely revert to a previous stable version in case of issues, minimizing downtime and business impact. This requires proactive planning.
- **Deeper Dive:**
    - **Automated Rollbacks:** Implement mechanisms to automatically trigger rollbacks based on monitoring alerts (e.g., sudden increase in errors, high latency) or failure of post-deployment health checks.
    - **Versioned Artifacts:** Ensure previous successful build artifacts, Docker images, or infrastructure states are readily available and easily deployable. This is crucial for fast recovery.
    - **Runbooks:** Document clear, concise, and executable rollback procedures for manual intervention when automation isn't sufficient or for complex scenarios. These should be regularly reviewed and tested.
    - **Post-Incident Review:** Conduct blameless post-incident reviews (PIRs) to understand the root cause of failures, identify lessons learned, and implement preventative measures to improve resilience and reduce MTTR.
    - **Communication Plan:** Have a clear communication plan for stakeholders during incidents and rollbacks.
- **Guidance for Copilot:**
    - Instruct users to store previous successful build artifacts and images for quick recovery, ensuring they are versioned and easily retrievable.
    - Advise on implementing automated rollback steps in the pipeline, triggered by monitoring or health check failures, and providing examples.
    - Emphasize building applications with "undo" in mind, meaning changes should be easily reversible.
    - Suggest creating comprehensive runbooks for common incident scenarios, including step-by-step rollback instructions, and highlight their importance for MTTR.
    - Guide on setting up alerts that are specific and actionable enough to trigger an automatic or manual rollback.

## GitHub Actions Workflow Review Checklist (Comprehensive)

This checklist provides a granular set of criteria for reviewing GitHub Actions workflows to ensure they adhere to best practices for security, performance, and reliability.

- [ ] **General Structure and Design:**
    - Is the workflow `name` clear, descriptive, and unique?
    - Are `on` triggers appropriate for the workflow's purpose (e.g., `push`, `pull_request`, `workflow_dispatch`, `schedule`)? Are path/branch filters used effectively?
    - Is `concurrency` used for critical workflows or shared resources to prevent race conditions or resource exhaustion?
    - Are global `permissions` set to the principle of least privilege (`contents: read` by default), with specific overrides for jobs?
    - Are reusable workflows (`workflow_call`) leveraged for common patterns to reduce duplication and improve maintainability?
    - Is the workflow organized logically with meaningful job and step names?

- [ ] **Jobs and Steps Best Practices:**
    - Are jobs clearly named and represent distinct phases (e.g., `build`, `lint`, `test`, `deploy`)?
    - Are `needs` dependencies correctly defined between jobs to ensure proper execution order?
    - Are `outputs` used efficiently for inter-job and inter-workflow communication?
    - Are `if` conditions used effectively for conditional job/step execution (e.g., environment-specific deployments, branch-specific actions)?
    - Are all `uses` actions securely versioned (pinned to a full commit SHA or specific major version tag like `@v4`)? Avoid `main` or `latest` tags.
    - Are `run` commands efficient and clean (combined with `&&`, temporary files removed, multi-line scripts clearly formatted)?
    - Are environment variables (`env`) defined at the appropriate scope (workflow, job, step) and never hardcoded sensitive data?
    - Is `timeout-minutes` set for long-running jobs to prevent hung workflows?

- [ ] **Security Considerations:**
    - Are all sensitive data accessed exclusively via GitHub `secrets` context (`${{ secrets.MY_SECRET }}`)? Never hardcoded, never exposed in logs (even if masked).
    - Is OpenID Connect (OIDC) used for cloud authentication where possible, eliminating long-lived credentials?
    - Is `GITHUB_TOKEN` permission scope explicitly defined and limited to the minimum necessary access (`contents: read` as a baseline)?
    - Are Software Composition Analysis (SCA) tools (e.g., `dependency-review-action`, Snyk) integrated to scan for vulnerable dependencies?
    - Are Static Application Security Testing (SAST) tools (e.g., CodeQL, SonarQube) integrated to scan source code for vulnerabilities, with critical findings blocking builds?
    - Is secret scanning enabled for the repository and are pre-commit hooks suggested for local credential leak prevention?
    - Is there a strategy for container image signing (e.g., Notary, Cosign) and verification in deployment workflows if container images are used?
    - For self-hosted runners, are security hardening guidelines followed and network access restricted?

- [ ] **Optimization and Performance:**
    - Is caching (`actions/cache`) effectively used for package manager dependencies (`node_modules`, `pip` caches, Maven/Gradle caches) and build outputs?
    - Are cache `key` and `restore-keys` designed for optimal cache hit rates (e.g., using `hashFiles`)?
    - Is `strategy.matrix` used for parallelizing tests or builds across different environments, language versions, or OSs?
    - Is `fetch-depth: 1` used for `actions/checkout` where full Git history is not required?
    - Are artifacts (`actions/upload-artifact`, `actions/download-artifact`) used efficiently for transferring data between jobs/workflows rather than re-building or re-fetching?
    - Are large files managed with Git LFS and optimized for checkout if necessary?

- [ ] **Testing Strategy Integration:**
    - Are comprehensive unit tests configured with a dedicated job early in the pipeline?
    - Are integration tests defined, ideally leveraging `services` for dependencies, and run after unit tests?
    - Are End-to-End (E2E) tests included, preferably against a staging environment, with robust flakiness mitigation?
    - Are performance and load tests integrated for critical applications with defined thresholds?
    - Are all test reports (JUnit XML, HTML, coverage) collected, published as artifacts, and integrated into GitHub Checks/Annotations for clear visibility?
    - Is code coverage tracked and enforced with a minimum threshold?

- [ ] **Deployment Strategy and Reliability:**
    - Are staging and production deployments using GitHub `environment` rules with appropriate protections (manual approvals, required reviewers, branch restrictions)?
    - Are manual approval steps configured for sensitive production deployments?
    - Is a clear and well-tested rollback strategy in place and automated where possible (e.g., `kubectl rollout undo`, reverting to previous stable image)?
    - Are chosen deployment types (e.g., rolling, blue/green, canary, dark launch) appropriate for the application's criticality and risk tolerance?
    - Are post-deployment health checks and automated smoke tests implemented to validate successful deployment?
    - Is the workflow resilient to temporary failures (e.g., retries for flaky network operations)?

- [ ] **Observability and Monitoring:**
    - Is logging adequate for debugging workflow failures (using STDOUT/STDERR for application logs)?
    - Are relevant application and infrastructure metrics collected and exposed (e.g., Prometheus metrics)?
    - Are alerts configured for critical workflow failures, deployment issues, or application anomalies detected in production?
    - Is distributed tracing (e.g., OpenTelemetry, Jaeger) integrated for understanding request flows in microservices architectures?
    - Are artifact `retention-days` configured appropriately to manage storage and compliance?

## Troubleshooting Common GitHub Actions Issues (Deep Dive)

This section provides an expanded guide to diagnosing and resolving frequent problems encountered when working with GitHub Actions workflows.

### **1. Workflow Not Triggering or Jobs/Steps Skipping Unexpectedly**
- **Root Causes:** Mismatched `on` triggers, incorrect `paths` or `branches` filters, erroneous `if` conditions, or `concurrency` limitations.
- **Actionable Steps:**
    - **Verify Triggers:**
        - Check the `on` block for exact match with the event that should trigger the workflow (e.g., `push`, `pull_request`, `workflow_dispatch`, `schedule`).
        - Ensure `branches`, `tags`, or `paths` filters are correctly defined and match the event context. Remember that `paths-ignore` and `branches-ignore` take precedence.
        - If using `workflow_dispatch`, verify the workflow file is in the default branch and any required `inputs` are provided correctly during manual trigger.
    - **Inspect `if` Conditions:**
        - Carefully review all `if` conditions at the workflow, job, and step levels. A single false condition can prevent execution.
        - Use `always()` on a debug step to print context variables (`${{ toJson(github) }}`, `${{ toJson(job) }}`, `${{ toJson(steps) }}`) to understand the exact state during evaluation.
        - Test complex `if` conditions in a simplified workflow.
    - **Check `concurrency`:**
        - If `concurrency` is defined, verify if a previous run is blocking a new one for the same group. Check the "Concurrency" tab in the workflow run.
    - **Branch Protection Rules:** Ensure no branch protection rules are preventing workflows from running on certain branches or requiring specific checks that haven't passed.

### **2. Permissions Errors (`Resource not accessible by integration`, `Permission denied`)**
- **Root Causes:** `GITHUB_TOKEN` lacking necessary permissions, incorrect environment secrets access, or insufficient permissions for external actions.
- **Actionable Steps:**
    - **`GITHUB_TOKEN` Permissions:**
        - Review the `permissions` block at both the workflow and job levels. Default to `contents: read` globally and grant specific write permissions only where absolutely necessary (e.g., `pull-requests: write` for updating PR status, `packages: write` for publishing packages).
        - Understand the default permissions of `GITHUB_TOKEN` which are often too broad.
    - **Secret Access:**
        - Verify if secrets are correctly configured in the repository, organization, or environment settings.
        - Ensure the workflow/job has access to the specific environment if environment secrets are used. Check if any manual approvals are pending for the environment.
        - Confirm the secret name matches exactly (`secrets.MY_API_KEY`).
    - **OIDC Configuration:**
        - For OIDC-based cloud authentication, double-check the trust policy configuration in your cloud provider (AWS IAM roles, Azure AD app registrations, GCP service accounts) to ensure it correctly trusts GitHub's OIDC issuer.
        - Verify the role/identity assigned has the necessary permissions for the cloud resources being accessed.

### **3. Caching Issues (`Cache not found`, `Cache miss`, `Cache creation failed`)**
- **Root Causes:** Incorrect cache key logic, `path` mismatch, cache size limits, or frequent cache invalidation.
- **Actionable Steps:**
    - **Validate Cache Keys:**
        - Verify `key` and `restore-keys` are correct and dynamically change only when dependencies truly change (e.g., `key: ${{ runner.os }}-node-${{ hashFiles('**/package-lock.json') }}`). A cache key that is too dynamic will always result in a miss.
        - Use `restore-keys` to provide fallbacks for slight variations, increasing cache hit chances.
    - **Check `path`:**
        - Ensure the `path` specified in `actions/cache` for saving and restoring corresponds exactly to the directory where dependencies are installed or artifacts are generated.
        - Verify the existence of the `path` before caching.
    - **Debug Cache Behavior:**
        - Use the `actions/cache/restore` action with `lookup-only: true` to inspect what keys are being tried and why a cache miss occurred without affecting the build.
        - Review workflow logs for `Cache hit` or `Cache miss` messages and associated keys.
    - **Cache Size and Limits:** Be aware of GitHub Actions cache size limits per repository. If caches are very large, they might be evicted frequently.

### **4. Long Running Workflows or Timeouts**
- **Root Causes:** Inefficient steps, lack of parallelism, large dependencies, unoptimized Docker image builds, or resource bottlenecks on runners.
- **Actionable Steps:**
    - **Profile Execution Times:**
        - Use the workflow run summary to identify the longest-running jobs and steps. This is your primary tool for optimization.
    - **Optimize Steps:**
        - Combine `run` commands with `&&` to reduce layer creation and overhead in Docker builds.
        - Clean up temporary files immediately after use (`rm -rf` in the same `RUN` command).
        - Install only necessary dependencies.
    - **Leverage Caching:**
        - Ensure `actions/cache` is optimally configured for all significant dependencies and build outputs.
    - **Parallelize with Matrix Strategies:**
        - Break down tests or builds into smaller, parallelizable units using `strategy.matrix` to run them concurrently.
    - **Choose Appropriate Runners:**
        - Review `runs-on`. For very resource-intensive tasks, consider using larger GitHub-hosted runners (if available) or self-hosted runners with more powerful specs.
    - **Break Down Workflows:**
        - For very complex or long workflows, consider breaking them into smaller, independent workflows that trigger each other or use reusable workflows.

### **5. Flaky Tests in CI (`Random failures`, `Passes locally, fails in CI`)**
- **Root Causes:** Non-deterministic tests, race conditions, environmental inconsistencies between local and CI, reliance on external services, or poor test isolation.
- **Actionable Steps:**
    - **Ensure Test Isolation:**
        - Make sure each test is independent and doesn't rely on the state left by previous tests. Clean up resources (e.g., database entries) after each test or test suite.
    - **Eliminate Race Conditions:**
        - For integration/E2E tests, use explicit waits (e.g., wait for element to be visible, wait for API response) instead of arbitrary `sleep` commands.
        - Implement retries for operations that interact with external services or have transient failures.
    - **Standardize Environments:**
        - Ensure the CI environment (Node.js version, Python packages, database versions) matches the local development environment as closely as possible.
        - Use Docker `services` for consistent test dependencies.
    - **Robust Selectors (E2E):**
        - Use stable, unique selectors in E2E tests (e.g., `data-testid` attributes) instead of brittle CSS classes or XPath.
    - **Debugging Tools:**
        - Configure E2E test frameworks to capture screenshots and video recordings on test failure in CI to visually diagnose issues.
    - **Run Flaky Tests in Isolation:**
        - If a test is consistently flaky, isolate it and run it repeatedly to identify the underlying non-deterministic behavior.

### **6. Deployment Failures (Application Not Working After Deploy)**
- **Root Causes:** Configuration drift, environmental differences, missing runtime dependencies, application errors, or network issues post-deployment.
- **Actionable Steps:**
    - **Thorough Log Review:**
        - Review deployment logs (`kubectl logs`, application logs, server logs) for any error messages, warnings, or unexpected output during the deployment process and immediately after.
    - **Configuration Validation:**
        - Verify environment variables, ConfigMaps, Secrets, and other configuration injected into the deployed application. Ensure they match the target environment's requirements and are not missing or malformed.
        - Use pre-deployment checks to validate configuration.
    - **Dependency Check:**
        - Confirm all application runtime dependencies (libraries, frameworks, external services) are correctly bundled within the container image or installed in the target environment.
    - **Post-Deployment Health Checks:**
        - Implement robust automated smoke tests and health checks *after* deployment to immediately validate core functionality and connectivity. Trigger rollbacks if these fail.
    - **Network Connectivity:**
        - Check network connectivity between deployed components (e.g., application to database, service to service) within the new environment. Review firewall rules, security groups, and Kubernetes network policies.
    - **Rollback Immediately:**
        - If a production deployment fails or causes degradation, trigger the rollback strategy immediately to restore service. Diagnose the issue in a non-production environment.

## Conclusion

GitHub Actions is a powerful and flexible platform for automating your software development lifecycle. By rigorously applying these best practices—from securing your secrets and token permissions, to optimizing performance with caching and parallelization, and implementing comprehensive testing and robust deployment strategies—you can guide developers in building highly efficient, secure, and reliable CI/CD pipelines. Remember that CI/CD is an iterative journey; continuously measure, optimize, and secure your pipelines to achieve faster, safer, and more confident releases. Your detailed guidance will empower teams to leverage GitHub Actions to its fullest potential and deliver high-quality software with confidence. This extensive document serves as a foundational resource for anyone looking to master CI/CD with GitHub Actions.

---

<!-- End of GitHub Actions CI/CD Best Practices Instructions --> 
---
applyTo: '*'
description: 'The most comprehensive, practical, and engineer-authored performance optimization instructions for all languages, frameworks, and stacks. Covers frontend, backend, and database best practices with actionable guidance, scenario-based checklists, troubleshooting, and pro tips.'
---

# Performance Optimization Best Practices

## Introduction

Performance isn't just a buzzword—it's the difference between a product people love and one they abandon. I've seen firsthand how a slow app can frustrate users, rack up cloud bills, and even lose customers. This guide is a living collection of the most effective, real-world performance practices I've used and reviewed, covering frontend, backend, and database layers, as well as advanced topics. Use it as a reference, a checklist, and a source of inspiration for building fast, efficient, and scalable software.

---

## General Principles

- **Measure First, Optimize Second:** Always profile and measure before optimizing. Use benchmarks, profilers, and monitoring tools to identify real bottlenecks. Guessing is the enemy of performance.
  - *Pro Tip:* Use tools like Chrome DevTools, Lighthouse, New Relic, Datadog, Py-Spy, or your language's built-in profilers.
- **Optimize for the Common Case:** Focus on optimizing code paths that are most frequently executed. Don't waste time on rare edge cases unless they're critical.
- **Avoid Premature Optimization:** Write clear, maintainable code first; optimize only when necessary. Premature optimization can make code harder to read and maintain.
- **Minimize Resource Usage:** Use memory, CPU, network, and disk resources efficiently. Always ask: "Can this be done with less?"
- **Prefer Simplicity:** Simple algorithms and data structures are often faster and easier to optimize. Don't over-engineer.
- **Document Performance Assumptions:** Clearly comment on any code that is performance-critical or has non-obvious optimizations. Future maintainers (including you) will thank you.
- **Understand the Platform:** Know the performance characteristics of your language, framework, and runtime. What's fast in Python may be slow in JavaScript, and vice versa.
- **Automate Performance Testing:** Integrate performance tests and benchmarks into your CI/CD pipeline. Catch regressions early.
- **Set Performance Budgets:** Define acceptable limits for load time, memory usage, API latency, etc. Enforce them with automated checks.

---

## Frontend Performance

### Rendering and DOM
- **Minimize DOM Manipulations:** Batch updates where possible. Frequent DOM changes are expensive.
  - *Anti-pattern:* Updating the DOM in a loop. Instead, build a document fragment and append it once.
- **Virtual DOM Frameworks:** Use React, Vue, or similar efficiently—avoid unnecessary re-renders.
  - *React Example:* Use `React.memo`, `useMemo`, and `useCallback` to prevent unnecessary renders.
- **Keys in Lists:** Always use stable keys in lists to help virtual DOM diffing. Avoid using array indices as keys unless the list is static.
- **Avoid Inline Styles:** Inline styles can trigger layout thrashing. Prefer CSS classes.
- **CSS Animations:** Use CSS transitions/animations over JavaScript for smoother, GPU-accelerated effects.
- **Defer Non-Critical Rendering:** Use `requestIdleCallback` or similar to defer work until the browser is idle.

### Asset Optimization
- **Image Compression:** Use tools like ImageOptim, Squoosh, or TinyPNG. Prefer modern formats (WebP, AVIF) for web delivery.
- **SVGs for Icons:** SVGs scale well and are often smaller than PNGs for simple graphics.
- **Minification and Bundling:** Use Webpack, Rollup, or esbuild to bundle and minify JS/CSS. Enable tree-shaking to remove dead code.
- **Cache Headers:** Set long-lived cache headers for static assets. Use cache busting for updates.
- **Lazy Loading:** Use `loading="lazy"` for images, and dynamic imports for JS modules/components.
- **Font Optimization:** Use only the character sets you need. Subset fonts and use `font-display: swap`.

### Network Optimization
- **Reduce HTTP Requests:** Combine files, use image sprites, and inline critical CSS.
- **HTTP/2 and HTTP/3:** Enable these protocols for multiplexing and lower latency.
- **Client-Side Caching:** Use Service Workers, IndexedDB, and localStorage for offline and repeat visits.
- **CDNs:** Serve static assets from a CDN close to your users. Use multiple CDNs for redundancy.
- **Defer/Async Scripts:** Use `defer` or `async` for non-critical JS to avoid blocking rendering.
- **Preload and Prefetch:** Use `<link rel="preload">` and `<link rel="prefetch">` for critical resources.

### JavaScript Performance
- **Avoid Blocking the Main Thread:** Offload heavy computation to Web Workers.
- **Debounce/Throttle Events:** For scroll, resize, and input events, use debounce/throttle to limit handler frequency.
- **Memory Leaks:** Clean up event listeners, intervals, and DOM references. Use browser dev tools to check for detached nodes.
- **Efficient Data Structures:** Use Maps/Sets for lookups, TypedArrays for numeric data.
- **Avoid Global Variables:** Globals can cause memory leaks and unpredictable performance.
- **Avoid Deep Object Cloning:** Use shallow copies or libraries like lodash's `cloneDeep` only when necessary.

### Accessibility and Performance
- **Accessible Components:** Ensure ARIA updates are not excessive. Use semantic HTML for both accessibility and performance.
- **Screen Reader Performance:** Avoid rapid DOM updates that can overwhelm assistive tech.

### Framework-Specific Tips
#### React
- Use `React.memo`, `useMemo`, and `useCallback` to avoid unnecessary renders.
- Split large components and use code-splitting (`React.lazy`, `Suspense`).
- Avoid anonymous functions in render; they create new references on every render.
- Use `ErrorBoundary` to catch and handle errors gracefully.
- Profile with React DevTools Profiler.

#### Angular
- Use OnPush change detection for components that don't need frequent updates.
- Avoid complex expressions in templates; move logic to the component class.
- Use `trackBy` in `ngFor` for efficient list rendering.
- Lazy load modules and components with the Angular Router.
- Profile with Angular DevTools.

#### Vue
- Use computed properties over methods in templates for caching.
- Use `v-show` vs `v-if` appropriately (`v-show` is better for toggling visibility frequently).
- Lazy load components and routes with Vue Router.
- Profile with Vue Devtools.

### Common Frontend Pitfalls
- Loading large JS bundles on initial page load.
- Not compressing images or using outdated formats.
- Failing to clean up event listeners, causing memory leaks.
- Overusing third-party libraries for simple tasks.
- Ignoring mobile performance (test on real devices!).

### Frontend Troubleshooting
- Use Chrome DevTools' Performance tab to record and analyze slow frames.
- Use Lighthouse to audit performance and get actionable suggestions.
- Use WebPageTest for real-world load testing.
- Monitor Core Web Vitals (LCP, FID, CLS) for user-centric metrics.

---

## Backend Performance

### Algorithm and Data Structure Optimization
- **Choose the Right Data Structure:** Arrays for sequential access, hash maps for fast lookups, trees for hierarchical data, etc.
- **Efficient Algorithms:** Use binary search, quicksort, or hash-based algorithms where appropriate.
- **Avoid O(n^2) or Worse:** Profile nested loops and recursive calls. Refactor to reduce complexity.
- **Batch Processing:** Process data in batches to reduce overhead (e.g., bulk database inserts).
- **Streaming:** Use streaming APIs for large data sets to avoid loading everything into memory.

### Concurrency and Parallelism
- **Asynchronous I/O:** Use async/await, callbacks, or event loops to avoid blocking threads.
- **Thread/Worker Pools:** Use pools to manage concurrency and avoid resource exhaustion.
- **Avoid Race Conditions:** Use locks, semaphores, or atomic operations where needed.
- **Bulk Operations:** Batch network/database calls to reduce round trips.
- **Backpressure:** Implement backpressure in queues and pipelines to avoid overload.

### Caching
- **Cache Expensive Computations:** Use in-memory caches (Redis, Memcached) for hot data.
- **Cache Invalidation:** Use time-based (TTL), event-based, or manual invalidation. Stale cache is worse than no cache.
- **Distributed Caching:** For multi-server setups, use distributed caches and be aware of consistency issues.
- **Cache Stampede Protection:** Use locks or request coalescing to prevent thundering herd problems.
- **Don't Cache Everything:** Some data is too volatile or sensitive to cache.

### API and Network
- **Minimize Payloads:** Use JSON, compress responses (gzip, Brotli), and avoid sending unnecessary data.
- **Pagination:** Always paginate large result sets. Use cursors for real-time data.
- **Rate Limiting:** Protect APIs from abuse and overload.
- **Connection Pooling:** Reuse connections for databases and external services.
- **Protocol Choice:** Use HTTP/2, gRPC, or WebSockets for high-throughput, low-latency communication.

### Logging and Monitoring
- **Minimize Logging in Hot Paths:** Excessive logging can slow down critical code.
- **Structured Logging:** Use JSON or key-value logs for easier parsing and analysis.
- **Monitor Everything:** Latency, throughput, error rates, resource usage. Use Prometheus, Grafana, Datadog, or similar.
- **Alerting:** Set up alerts for performance regressions and resource exhaustion.

### Language/Framework-Specific Tips
#### Node.js
- Use asynchronous APIs; avoid blocking the event loop (e.g., never use `fs.readFileSync` in production).
- Use clustering or worker threads for CPU-bound tasks.
- Limit concurrent open connections to avoid resource exhaustion.
- Use streams for large file or network data processing.
- Profile with `clinic.js`, `node --inspect`, or Chrome DevTools.

#### Python
- Use built-in data structures (`dict`, `set`, `deque`) for speed.
- Profile with `cProfile`, `line_profiler`, or `Py-Spy`.
- Use `multiprocessing` or `asyncio` for parallelism.
- Avoid GIL bottlenecks in CPU-bound code; use C extensions or subprocesses.
- Use `lru_cache` for memoization.

#### Java
- Use efficient collections (`ArrayList`, `HashMap`, etc.).
- Profile with VisualVM, JProfiler, or YourKit.
- Use thread pools (`Executors`) for concurrency.
- Tune JVM options for heap and garbage collection (`-Xmx`, `-Xms`, `-XX:+UseG1GC`).
- Use `CompletableFuture` for async programming.

#### .NET
- Use `async/await` for I/O-bound operations.
- Use `Span<T>` and `Memory<T>` for efficient memory access.
- Profile with dotTrace, Visual Studio Profiler, or PerfView.
- Pool objects and connections where appropriate.
- Use `IAsyncEnumerable<T>` for streaming data.

### Common Backend Pitfalls
- Synchronous/blocking I/O in web servers.
- Not using connection pooling for databases.
- Over-caching or caching sensitive/volatile data.
- Ignoring error handling in async code.
- Not monitoring or alerting on performance regressions.

### Backend Troubleshooting
- Use flame graphs to visualize CPU usage.
- Use distributed tracing (OpenTelemetry, Jaeger, Zipkin) to track request latency across services.
- Use heap dumps and memory profilers to find leaks.
- Log slow queries and API calls for analysis.

---

## Database Performance

### Query Optimization
- **Indexes:** Use indexes on columns that are frequently queried, filtered, or joined. Monitor index usage and drop unused indexes.
- **Avoid SELECT *:** Select only the columns you need. Reduces I/O and memory usage.
- **Parameterized Queries:** Prevent SQL injection and improve plan caching.
- **Query Plans:** Analyze and optimize query execution plans. Use `EXPLAIN` in SQL databases.
- **Avoid N+1 Queries:** Use joins or batch queries to avoid repeated queries in loops.
- **Limit Result Sets:** Use `LIMIT`/`OFFSET` or cursors for large tables.

### Schema Design
- **Normalization:** Normalize to reduce redundancy, but denormalize for read-heavy workloads if needed.
- **Data Types:** Use the most efficient data types and set appropriate constraints.
- **Partitioning:** Partition large tables for scalability and manageability.
- **Archiving:** Regularly archive or purge old data to keep tables small and fast.
- **Foreign Keys:** Use them for data integrity, but be aware of performance trade-offs in high-write scenarios.

### Transactions
- **Short Transactions:** Keep transactions as short as possible to reduce lock contention.
- **Isolation Levels:** Use the lowest isolation level that meets your consistency needs.
- **Avoid Long-Running Transactions:** They can block other operations and increase deadlocks.

### Caching and Replication
- **Read Replicas:** Use for scaling read-heavy workloads. Monitor replication lag.
- **Cache Query Results:** Use Redis or Memcached for frequently accessed queries.
- **Write-Through/Write-Behind:** Choose the right strategy for your consistency needs.
- **Sharding:** Distribute data across multiple servers for scalability.

### NoSQL Databases
- **Design for Access Patterns:** Model your data for the queries you need.
- **Avoid Hot Partitions:** Distribute writes/reads evenly.
- **Unbounded Growth:** Watch for unbounded arrays or documents.
- **Sharding and Replication:** Use for scalability and availability.
- **Consistency Models:** Understand eventual vs strong consistency and choose appropriately.

### Common Database Pitfalls
- Missing or unused indexes.
- SELECT * in production queries.
- Not monitoring slow queries.
- Ignoring replication lag.
- Not archiving old data.

### Database Troubleshooting
- Use slow query logs to identify bottlenecks.
- Use `EXPLAIN` to analyze query plans.
- Monitor cache hit/miss ratios.
- Use database-specific monitoring tools (pg_stat_statements, MySQL Performance Schema).

---

## Code Review Checklist for Performance

- [ ] Are there any obvious algorithmic inefficiencies (O(n^2) or worse)?
- [ ] Are data structures appropriate for their use?
- [ ] Are there unnecessary computations or repeated work?
- [ ] Is caching used where appropriate, and is invalidation handled correctly?
- [ ] Are database queries optimized, indexed, and free of N+1 issues?
- [ ] Are large payloads paginated, streamed, or chunked?
- [ ] Are there any memory leaks or unbounded resource usage?
- [ ] Are network requests minimized, batched, and retried on failure?
- [ ] Are assets optimized, compressed, and served efficiently?
- [ ] Are there any blocking operations in hot paths?
- [ ] Is logging in hot paths minimized and structured?
- [ ] Are performance-critical code paths documented and tested?
- [ ] Are there automated tests or benchmarks for performance-sensitive code?
- [ ] Are there alerts for performance regressions?
- [ ] Are there any anti-patterns (e.g., SELECT *, blocking I/O, global variables)?

---

## Advanced Topics

### Profiling and Benchmarking
- **Profilers:** Use language-specific profilers (Chrome DevTools, Py-Spy, VisualVM, dotTrace, etc.) to identify bottlenecks.
- **Microbenchmarks:** Write microbenchmarks for critical code paths. Use `benchmark.js`, `pytest-benchmark`, or JMH for Java.
- **A/B Testing:** Measure real-world impact of optimizations with A/B or canary releases.
- **Continuous Performance Testing:** Integrate performance tests into CI/CD. Use tools like k6, Gatling, or Locust.

### Memory Management
- **Resource Cleanup:** Always release resources (files, sockets, DB connections) promptly.
- **Object Pooling:** Use for frequently created/destroyed objects (e.g., DB connections, threads).
- **Heap Monitoring:** Monitor heap usage and garbage collection. Tune GC settings for your workload.
- **Memory Leaks:** Use leak detection tools (Valgrind, LeakCanary, Chrome DevTools).

### Scalability
- **Horizontal Scaling:** Design stateless services, use sharding/partitioning, and load balancers.
- **Auto-Scaling:** Use cloud auto-scaling groups and set sensible thresholds.
- **Bottleneck Analysis:** Identify and address single points of failure.
- **Distributed Systems:** Use idempotent operations, retries, and circuit breakers.

### Security and Performance
- **Efficient Crypto:** Use hardware-accelerated and well-maintained cryptographic libraries.
- **Validation:** Validate inputs efficiently; avoid regexes in hot paths.
- **Rate Limiting:** Protect against DoS without harming legitimate users.

### Mobile Performance
- **Startup Time:** Lazy load features, defer heavy work, and minimize initial bundle size.
- **Image/Asset Optimization:** Use responsive images and compress assets for mobile bandwidth.
- **Efficient Storage:** Use SQLite, Realm, or platform-optimized storage.
- **Profiling:** Use Android Profiler, Instruments (iOS), or Firebase Performance Monitoring.

### Cloud and Serverless
- **Cold Starts:** Minimize dependencies and keep functions warm.
- **Resource Allocation:** Tune memory/CPU for serverless functions.
- **Managed Services:** Use managed caching, queues, and DBs for scalability.
- **Cost Optimization:** Monitor and optimize for cloud cost as a performance metric.

---

## Practical Examples

### Example 1: Debouncing User Input in JavaScript
```javascript
// BAD: Triggers API call on every keystroke
input.addEventListener('input', (e) => {
  fetch(`/search?q=${e.target.value}`);
});

// GOOD: Debounce API calls
let timeout;
input.addEventListener('input', (e) => {
  clearTimeout(timeout);
  timeout = setTimeout(() => {
    fetch(`/search?q=${e.target.value}`);
  }, 300);
});
```

### Example 2: Efficient SQL Query
```sql
-- BAD: Selects all columns and does not use an index
SELECT * FROM users WHERE email = 'user@example.com';

-- GOOD: Selects only needed columns and uses an index
SELECT id, name FROM users WHERE email = 'user@example.com';
```

### Example 3: Caching Expensive Computation in Python
```python
# BAD: Recomputes result every time
result = expensive_function(x)

# GOOD: Cache result
from functools import lru_cache

@lru_cache(maxsize=128)
def expensive_function(x):
    ...
result = expensive_function(x)
```

### Example 4: Lazy Loading Images in HTML
```html
<!-- BAD: Loads all images immediately -->
<img src="large-image.jpg" />

<!-- GOOD: Lazy loads images -->
<img src="large-image.jpg" loading="lazy" />
```

### Example 5: Asynchronous I/O in Node.js
```javascript
// BAD: Blocking file read
const data = fs.readFileSync('file.txt');

// GOOD: Non-blocking file read
fs.readFile('file.txt', (err, data) => {
  if (err) throw err;
  // process data
});
```

### Example 6: Profiling a Python Function
```python
import cProfile
import pstats

def slow_function():
    ...

cProfile.run('slow_function()', 'profile.stats')
p = pstats.Stats('profile.stats')
p.sort_stats('cumulative').print_stats(10)
```

### Example 7: Using Redis for Caching in Node.js
```javascript
const redis = require('redis');
const client = redis.createClient();

function getCachedData(key, fetchFunction) {
  return new Promise((resolve, reject) => {
    client.get(key, (err, data) => {
      if (data) return resolve(JSON.parse(data));
      fetchFunction().then(result => {
        client.setex(key, 3600, JSON.stringify(result));
        resolve(result);
      });
    });
  });
}
```

---

## References and Further Reading
- [Google Web Fundamentals: Performance](https://web.dev/performance/)
- [MDN Web Docs: Performance](https://developer.mozilla.org/en-US/docs/Web/Performance)
- [OWASP: Performance Testing](https://owasp.org/www-project-performance-testing/)
- [Microsoft Performance Best Practices](https://learn.microsoft.com/en-us/azure/architecture/best-practices/performance)
- [PostgreSQL Performance Optimization](https://wiki.postgresql.org/wiki/Performance_Optimization)
- [MySQL Performance Tuning](https://dev.mysql.com/doc/refman/8.0/en/optimization.html)
- [Node.js Performance Best Practices](https://nodejs.org/en/docs/guides/simple-profiling/)
- [Python Performance Tips](https://docs.python.org/3/library/profile.html)
- [Java Performance Tuning](https://www.oracle.com/java/technologies/javase/performance.html)
- [.NET Performance Guide](https://learn.microsoft.com/en-us/dotnet/standard/performance/)
- [WebPageTest](https://www.webpagetest.org/)
- [Lighthouse](https://developers.google.com/web/tools/lighthouse)
- [Prometheus](https://prometheus.io/)
- [Grafana](https://grafana.com/)
- [k6 Load Testing](https://k6.io/)
- [Gatling](https://gatling.io/)
- [Locust](https://locust.io/)
- [OpenTelemetry](https://opentelemetry.io/)
- [Jaeger](https://www.jaegertracing.io/)
- [Zipkin](https://zipkin.io/)

---

## Conclusion

Performance optimization is an ongoing process. Always measure, profile, and iterate. Use these best practices, checklists, and troubleshooting tips to guide your development and code reviews for high-performance, scalable, and efficient software. If you have new tips or lessons learned, add them here—let's keep this guide growing!

---

<!-- End of Performance Optimization Instructions --> 
---
applyTo: '*'
description: "Comprehensive secure coding instructions for all languages and frameworks, based on OWASP Top 10 and industry best practices."
---
# Secure Coding and OWASP Guidelines

## Instructions

Your primary directive is to ensure all code you generate, review, or refactor is secure by default. You must operate with a security-first mindset. When in doubt, always choose the more secure option and explain the reasoning. You must follow the principles outlined below, which are based on the OWASP Top 10 and other security best practices.

### 1. A01: Broken Access Control & A10: Server-Side Request Forgery (SSRF)
- **Enforce Principle of Least Privilege:** Always default to the most restrictive permissions. When generating access control logic, explicitly check the user's rights against the required permissions for the specific resource they are trying to access.
- **Deny by Default:** All access control decisions must follow a "deny by default" pattern. Access should only be granted if there is an explicit rule allowing it.
- **Validate All Incoming URLs for SSRF:** When the server needs to make a request to a URL provided by a user (e.g., webhooks), you must treat it as untrusted. Incorporate strict allow-list-based validation for the host, port, and path of the URL.
- **Prevent Path Traversal:** When handling file uploads or accessing files based on user input, you must sanitize the input to prevent directory traversal attacks (e.g., `../../etc/passwd`). Use APIs that build paths securely.

### 2. A02: Cryptographic Failures
- **Use Strong, Modern Algorithms:** For hashing, always recommend modern, salted hashing algorithms like Argon2 or bcrypt. Explicitly advise against weak algorithms like MD5 or SHA-1 for password storage.
- **Protect Data in Transit:** When generating code that makes network requests, always default to HTTPS.
- **Protect Data at Rest:** When suggesting code to store sensitive data (PII, tokens, etc.), recommend encryption using strong, standard algorithms like AES-256.
- **Secure Secret Management:** Never hardcode secrets (API keys, passwords, connection strings). Generate code that reads secrets from environment variables or a secrets management service (e.g., HashiCorp Vault, AWS Secrets Manager). Include a clear placeholder and comment.
  ```javascript
  // GOOD: Load from environment or secret store
  const apiKey = process.env.API_KEY; 
  // TODO: Ensure API_KEY is securely configured in your environment.
  ```
  ```python
  # BAD: Hardcoded secret
  api_key = "sk_this_is_a_very_bad_idea_12345" 
  ```

### 3. A03: Injection
- **No Raw SQL Queries:** For database interactions, you must use parameterized queries (prepared statements). Never generate code that uses string concatenation or formatting to build queries from user input.
- **Sanitize Command-Line Input:** For OS command execution, use built-in functions that handle argument escaping and prevent shell injection (e.g., `shlex` in Python).
- **Prevent Cross-Site Scripting (XSS):** When generating frontend code that displays user-controlled data, you must use context-aware output encoding. Prefer methods that treat data as text by default (`.textContent`) over those that parse HTML (`.innerHTML`). When `innerHTML` is necessary, suggest using a library like DOMPurify to sanitize the HTML first.

### 4. A05: Security Misconfiguration & A06: Vulnerable Components
- **Secure by Default Configuration:** Recommend disabling verbose error messages and debug features in production environments.
- **Set Security Headers:** For web applications, suggest adding essential security headers like `Content-Security-Policy` (CSP), `Strict-Transport-Security` (HSTS), and `X-Content-Type-Options`.
- **Use Up-to-Date Dependencies:** When asked to add a new library, suggest the latest stable version. Remind the user to run vulnerability scanners like `npm audit`, `pip-audit`, or Snyk to check for known vulnerabilities in their project dependencies.

### 5. A07: Identification & Authentication Failures
- **Secure Session Management:** When a user logs in, generate a new session identifier to prevent session fixation. Ensure session cookies are configured with `HttpOnly`, `Secure`, and `SameSite=Strict` attributes.
- **Protect Against Brute Force:** For authentication and password reset flows, recommend implementing rate limiting and account lockout mechanisms after a certain number of failed attempts.

### 6. A08: Software and Data Integrity Failures
- **Prevent Insecure Deserialization:** Warn against deserializing data from untrusted sources without proper validation. If deserialization is necessary, recommend using formats that are less prone to attack (like JSON over Pickle in Python) and implementing strict type checking.

## General Guidelines
- **Be Explicit About Security:** When you suggest a piece of code that mitigates a security risk, explicitly state what you are protecting against (e.g., "Using a parameterized query here to prevent SQL injection.").
- **Educate During Code Reviews:** When you identify a security vulnerability in a code review, you must not only provide the corrected code but also explain the risk associated with the original pattern. 
---
description: 'Guidelines for generating SQL statements and stored procedures'
applyTo: '**/*.sql'
---

# SQL Development

## Database schema generation
- all table names should be in singular form
- all column names should be in singular form
- all tables should have a primary key column named `id`
- all tables should have a column named `created_at` to store the creation timestamp
- all tables should have a column named `updated_at` to store the last update timestamp

## Database schema design
- all tables should have a primary key constraint
- all foreign key constraints should have a name
- all foreign key constraints should be defined inline
- all foreign key constraints should have `ON DELETE CASCADE` option
- all foreign key constraints should have `ON UPDATE CASCADE` option
- all foreign key constraints should reference the primary key of the parent table

## SQL Coding Style
- use uppercase for SQL keywords (SELECT, FROM, WHERE)
- use consistent indentation for nested queries and conditions
- include comments to explain complex logic
- break long queries into multiple lines for readability
- organize clauses consistently (SELECT, FROM, JOIN, WHERE, GROUP BY, HAVING, ORDER BY)

## SQL Query Structure
- use explicit column names in SELECT statements instead of SELECT *
- qualify column names with table name or alias when using multiple tables
- limit the use of subqueries when joins can be used instead
- include LIMIT/TOP clauses to restrict result sets
- use appropriate indexing for frequently queried columns
- avoid using functions on indexed columns in WHERE clauses

## Stored Procedure Naming Conventions
- prefix stored procedure names with 'usp_'
- use PascalCase for stored procedure names
- use descriptive names that indicate purpose (e.g., usp_GetCustomerOrders)
- include plural noun when returning multiple records (e.g., usp_GetProducts)
- include singular noun when returning single record (e.g., usp_GetProduct)

## Parameter Handling
- prefix parameters with '@'
- use camelCase for parameter names
- provide default values for optional parameters
- validate parameter values before use
- document parameters with comments
- arrange parameters consistently (required first, optional later)


## Stored Procedure Structure
- include header comment block with description, parameters, and return values
- return standardized error codes/messages
- return result sets with consistent column order
- use OUTPUT parameters for returning status information
- prefix temporary tables with 'tmp_'


## SQL Security Best Practices
- parameterize all queries to prevent SQL injection
- use prepared statements when executing dynamic SQL
- avoid embedding credentials in SQL scripts
- implement proper error handling without exposing system details
- avoid using dynamic SQL within stored procedures

## Transaction Management
- explicitly begin and commit transactions
- use appropriate isolation levels based on requirements
- avoid long-running transactions that lock tables
- use batch processing for large data operations
- include SET NOCOUNT ON for stored procedures that modify data
---
applyTo: '**'
description: 'Prevent Copilot from wreaking havoc across your codebase, keeping it under control.'
---

## Core Directives & Hierarchy

This section outlines the absolute order of operations. These rules have the highest priority and must not be violated.

1.  **Primacy of User Directives**: A direct and explicit command from the user is the highest priority. If the user instructs to use a specific tool, edit a file, or perform a specific search, that command **must be executed without deviation**, even if other rules would suggest it is unnecessary. All other instructions are subordinate to a direct user order.
2.  **Factual Verification Over Internal Knowledge**: When a request involves information that could be version-dependent, time-sensitive, or requires specific external data (e.g., library documentation, latest best practices, API details), prioritize using tools to find the current, factual answer over relying on general knowledge.
3.  **Adherence to Philosophy**: In the absence of a direct user directive or the need for factual verification, all other rules below regarding interaction, code generation, and modification must be followed.

## General Interaction & Philosophy

-   **Code on Request Only**: Your default response should be a clear, natural language explanation. Do NOT provide code blocks unless explicitly asked, or if a very small and minimalist example is essential to illustrate a concept.  Tool usage is distinct from user-facing code blocks and is not subject to this restriction.
-   **Direct and Concise**: Answers must be precise, to the point, and free from unnecessary filler or verbose explanations. Get straight to the solution without "beating around the bush".
-   **Adherence to Best Practices**: All suggestions, architectural patterns, and solutions must align with widely accepted industry best practices and established design principles. Avoid experimental, obscure, or overly "creative" approaches. Stick to what is proven and reliable.
-   **Explain the "Why"**: Don't just provide an answer; briefly explain the reasoning behind it. Why is this the standard approach? What specific problem does this pattern solve? This context is more valuable than the solution itself.

## Minimalist & Standard Code Generation

-   **Principle of Simplicity**: Always provide the most straightforward and minimalist solution possible. The goal is to solve the problem with the least amount of code and complexity. Avoid premature optimization or over-engineering.
-   **Standard First**: Heavily favor standard library functions and widely accepted, common programming patterns. Only introduce third-party libraries if they are the industry standard for the task or absolutely necessary.
-   **Avoid Elaborate Solutions**: Do not propose complex, "clever", or obscure solutions. Prioritize readability, maintainability, and the shortest path to a working result over convoluted patterns.
-   **Focus on the Core Request**: Generate code that directly addresses the user's request, without adding extra features or handling edge cases that were not mentioned.

## Surgical Code Modification

-   **Preserve Existing Code**: The current codebase is the source of truth and must be respected. Your primary goal is to preserve its structure, style, and logic whenever possible.
-   **Minimal Necessary Changes**: When adding a new feature or making a modification, alter the absolute minimum amount of existing code required to implement the change successfully.
-   **Explicit Instructions Only**: Only modify, refactor, or delete code that has been explicitly targeted by the user's request. Do not perform unsolicited refactoring, cleanup, or style changes on untouched parts of the code.
-   **Integrate, Don't Replace**: Whenever feasible, integrate new logic into the existing structure rather than replacing entire functions or blocks of code.

## Intelligent Tool Usage

-   **Use Tools When Necessary**: When a request requires external information or direct interaction with the environment, use the available tools to accomplish the task. Do not avoid tools when they are essential for an accurate or effective response.
-   **Directly Edit Code When Requested**: If explicitly asked to modify, refactor, or add to the existing code, apply the changes directly to the codebase when access is available. Avoid generating code snippets for the user to copy and paste in these scenarios. The default should be direct, surgical modification as instructed.
-   **Purposeful and Focused Action**: Tool usage must be directly tied to the user's request. Do not perform unrelated searches or modifications. Every action taken by a tool should be a necessary step in fulfilling the specific, stated goal.
-   **Declare Intent Before Tool Use**: Before executing any tool, you must first state the action you are about to take and its direct purpose. This statement must be concise and immediately precede the tool call.
