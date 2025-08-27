# Library Management

This project is built with **.NET** following **Clean Architecture** principles.  
It demonstrates modern backend practices such as **Dependency Injection (DI)**, **CQRS with MediatR**, **Entity Framework Core**, and **Swagger API documentation** and **FluentValidation**.

---

##  Tech Stack & Features

- **Clean Architecture** – Separation of concerns between layers (Presentation, Application, Infrastructure, Domain).
- **Dependency Injection (DI)** – Loose coupling and testable components.
- **CQRS (Command Query Responsibility Segregation)** – Clear separation between read and write operations.
- **MediatR** – Simplified in-process messaging for CQRS commands and queries.
- **Entity Framework Core (EF Core)** – ORM for database access.
- **Swagger / OpenAPI** – Interactive API documentation and testing.
- **FluentValidation** – Strong input validation using `AbstractValidator` for DTOs.
- **JWT Authentication** – Secure authentication using JSON Web Tokens for user login and authorization.  
- **Generic Response Wrapper (`ServiceResponse<T>`)** – Standardized responses for API endpoints, including success, error, and not-found messages, with optional data payload.  
---
