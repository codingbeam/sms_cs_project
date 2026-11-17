# Student Management System

A .NET 10 console application project for managing student information. The project is currently a starter scaffold; student-management workflows described below represent the intended scope and are not implemented yet.

## Project Overview

The Student Management System is intended to provide a straightforward way to maintain and access student records from a command-line interface. It is designed as a foundation that can grow from a simple console application into a more complete student administration tool.

## Business Objectives

- Keep student information organized and easy to retrieve.
- Reduce reliance on scattered or manually maintained student records.
- Support consistent student record creation, updates, and removal.
- Provide a clear, menu-driven experience suitable for staff and small educational organizations.
- Establish a maintainable foundation for future persistence, reporting, and integrations.

## Technology Stack

- **Language:** C#
- **Platform:** .NET 10
- **Application type:** Console application
- **Project format:** SDK-style .NET project
- **External dependencies:** None currently declared

The project enables nullable reference types and implicit global usings.

## Application Features

The following capabilities describe the planned application scope. They are not available in the current starter scaffold.

- Add a student record.
- View a student record or list of students.
- Update student information.
- Delete a student record.
- Search or filter student records.
- Validate user input and provide clear command-line feedback.

## Project Structure

```text
StudentManagementConsoleApplication/
├── Program.cs
├── StudentManagementConsoleApplication.csproj
├── README.md
└── docs/
    ├── ARCHITECTURE.md
    ├── DEVELOPMENT-PLAN.md
    ├── REQUIREMENTS.md
    └── TEST-PLAN.md
```

- `Program.cs` — application entry point.
- `StudentManagementConsoleApplication.csproj` — target framework and project configuration.
- `docs/` — project planning and design documentation.

## Prerequisites

- Windows, macOS, or Linux.
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) installed and available on your `PATH`.

Verify the SDK installation with:

```shell
dotnet --version
```

The reported SDK version should be `10.0.x`.

## How to Build the Application

From the project directory, run:

```shell
dotnet build
```

To build a Release configuration:

```shell
dotnet build --configuration Release
```

## How to Run the Application

From the project directory, run:

```shell
dotnet run
```

To run using the Release configuration:

```shell
dotnet run --configuration Release
```

At this stage, the application is only a starter console program; the student-management features listed above have not yet been implemented.

## Future Enhancements

- Implement the planned student record management workflows.
- Add persistent storage, such as a local database or file-based repository.
- Add sorting, filtering, and more advanced search.
- Provide reports and data export.
- Add automated tests for business rules and user flows.
- Improve accessibility and usability of console prompts and output.
- Consider authentication, role-based access, and a graphical or web interface as the project grows.
