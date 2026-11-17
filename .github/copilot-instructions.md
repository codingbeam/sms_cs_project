# Project Instructions

## Project

Student Management System

## Technology

- Language: C#
- Platform: .NET 10
- Application type: Console application

## Coding Standards

- Follow Naming conventions for C#.
- Use meaningful variable and method names.
- Follow SOLID principles for object-oriented design.
- Prefer async programming when required.
- Avoid duplicate code.
- Use exception handling for error scenarios.

## Architecture

- Separate business logic from data access and presentation layers.
- Use models for student data representation.
- Use services for business operations.
- Use Repositories for data access and storage.

## Application Architecture

Follows Layered Architecture with the following layers:

### Models

Contains application entities.

### Interfaces

Contains service and repository interfaces.

### Services

Contains business logic, operations and validation.

### Repositories

Contains data access logic and storage operations.

### Data

Contains JSON storage files for student data.

## Important Notes:

- Keep Program.cs minimal.
- Do not put business logic in presentation layer.
- Use dependency injection.
- Use interfaces for services and repositories.
- Do not access JSON files directly from services, use repositories for data access.
- Keep the application easy to unit test.

## Testing

- Write unit tests for critical business logic.
- Cover positive and negative scenarios in tests.
- Validate all user inputs and handle exceptions gracefully.

## Copilot Guidelines

- Explain the generated code.
- Follow Existing Project architecture.
- Do not introduce unnecessary dependencies.
- Do not modify unrelated files.
