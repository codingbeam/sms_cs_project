# Student Management System — Requirements

## 1. Purpose

This document defines the functional and non-functional requirements for the Student Management System, a .NET 10 console application. It is intended to guide implementation and provide verifiable acceptance criteria.

## 2. Scope

The application shall support basic student record management through a text-based console interface: creating, listing, searching, updating, and deleting records. Student records shall be retained between application runs.

## 3. Definitions and Assumptions

- **Student ID:** A user-provided identifier that uniquely identifies a student in this system.
- **Student record:** The set of identity, contact, and enrollment details maintained for one student.
- The proposed baseline student fields are: Student ID, first name, last name, date of birth, email address, phone number, enrollment date, and program/course.
- Student ID is required, unique, and immutable after creation. Corrections requiring a different ID shall be handled by deleting the record and creating a new one.
- Required fields are Student ID, first name, and last name. Other fields may be left blank unless the implementation adopts stricter business rules.
- The system is intended for a single operator and a local persistent data store. Concurrent multi-user access, authentication, and synchronization are out of scope.

## 4. Functional Requirements

### FR-001 — Add a New Student

The system shall allow an operator to create a student record by entering the student details.

**Acceptance criteria**

- The system prompts for each supported student field and identifies required fields.
- The system validates the supplied values before saving the record.
- A record is saved only when all required values are valid and the Student ID is not already in use.
- On success, the system confirms that the student was added and identifies the record by Student ID.
- If validation or persistence fails, the system explains the failure and does not report a successful addition.

### FR-002 — Display All Students

The system shall allow an operator to display all student records currently stored.

**Acceptance criteria**

- Each displayed record includes its Student ID and all supported student details.
- Records are presented in a consistent, readable format; the ordering is stable (ascending Student ID unless another order is specified by the implementation).
- If no records exist, the system displays an informative empty-state message.
- A failure to retrieve records is reported without displaying incomplete data as a successful result.

### FR-003 — Search Student by ID

The system shall allow an operator to retrieve one student record by entering its Student ID.

**Acceptance criteria**

- The system validates that the search ID is not empty or whitespace.
- If a matching record exists, the system displays its supported details.
- If no matching record exists, the system displays a clear “not found” result.
- Searching shall not modify student data.

### FR-004 — Update Student Details

The system shall allow an operator to update the supported details of an existing student record.

**Acceptance criteria**

- The operator identifies the record by Student ID.
- If the ID does not exist, the system reports that no matching student was found and makes no changes.
- The system permits updates to supported student details while preserving the original Student ID.
- The system validates the resulting record before saving it.
- The update is all-or-nothing: if validation or persistence fails, the previously stored record remains unchanged.
- On success, the system confirms that the record was updated.

### FR-005 — Delete a Student

The system shall allow an operator to delete an existing student record by Student ID.

**Acceptance criteria**

- The system identifies the target record using Student ID.
- If the ID does not exist, the system reports that no matching student was found and makes no changes.
- Before deletion, the system displays the target student and requests confirmation.
- The record is removed only after explicit confirmation; declining or cancelling leaves it unchanged.
- On success, the system confirms that the record was deleted.

### FR-006 — Validate Student Information

The system shall validate student data when a record is added or updated.

**Acceptance criteria**

- Required fields shall reject empty or whitespace-only values.
- Student ID shall be trimmed of leading and trailing whitespace before uniqueness checks and storage.
- Names shall reject values that are empty or whitespace-only; surrounding whitespace shall not be stored.
- A supplied date of birth shall be a valid calendar date and shall not be in the future.
- A supplied enrollment date shall be a valid calendar date.
- A supplied email address shall conform to a basic email-address format when provided.
- Optional values may be omitted; omitted values shall be displayed consistently (for example, as “Not provided”).
- For each rejected value, the system shall identify the field and explain how to correct it, then allow the operator to retry without losing other entered values.

### FR-007 — Prevent Duplicate Student IDs

The system shall ensure that no two stored student records have the same Student ID.

**Acceptance criteria**

- Before adding a student, the system checks the normalized Student ID against existing records.
- Duplicate detection is case-insensitive, so IDs differing only by letter casing are considered duplicates.
- If an ID is already in use, the system rejects the addition, identifies the duplicate-ID issue, and prompts for a different ID.
- The persistence layer shall also enforce uniqueness so that duplicate records cannot be stored if application-level validation is bypassed.

### FR-008 — Navigate Console Operations

The system shall provide a console menu through which the operator can select each supported operation and exit the application.

**Acceptance criteria**

- The menu identifies the available operations and their selection keys.
- An invalid selection produces a clear message and returns the operator to the menu.
- After completing or cancelling an operation, the system returns to the menu unless the operator chooses to exit.
- Exiting the application shall not discard successfully saved changes.

## 5. Non-Functional Requirements

### NFR-001 — Maintainable Code

The implementation shall use consistent naming, formatting, and focused components so that changes can be made without unnecessary impact on unrelated behavior.

**Verification**

- Each component or method has a clearly defined responsibility.
- Business rules are not unnecessarily duplicated across operations.
- Public types and non-obvious business rules are documented where appropriate.

### NFR-002 — Proper Exception Handling

The application shall handle expected operational failures, including invalid input, storage access failures, and malformed persisted data, without terminating unexpectedly.

**Verification**

- Recoverable errors are reported in plain language and return the operator to a safe point in the workflow.
- Failures are not silently ignored or reported as successful operations.
- A failed write does not leave a partially updated or partially deleted record.
- Unexpected errors do not expose stack traces or internal implementation details in normal user-facing output.

### NFR-003 — Input Validation and Safe Recovery

All operator-provided values shall be validated at the application boundary before they are used or persisted.

**Verification**

- Empty, malformed, out-of-range, or unsupported values are rejected with actionable feedback.
- Invalid menu selections and operation inputs do not crash the application.
- Validation failures do not corrupt or partially save a student record.

### NFR-004 — Separation of Concerns

The implementation shall separate console interaction, student-management rules, and persistence responsibilities.

**Verification**

- Console input/output logic does not directly implement persistence details.
- Student validation and business rules can be exercised independently of console prompts.
- The persistence implementation can be changed without rewriting the student-management workflows.

### NFR-005 — Readable Console Output

Console prompts, results, and errors shall be clear, consistent, and understandable to an operator without technical knowledge.

**Verification**

- Menus and prompts explain what input is expected.
- Success, empty-state, not-found, validation, and error outcomes are distinguishable.
- Student details are labeled consistently and do not rely on color alone to convey meaning.

### NFR-006 — Persistent Data Storage

Student records shall be stored in a persistent local data store and remain available after a normal application restart.

**Verification**

- A successfully saved record can be read after closing and reopening the application.
- Add, update, and delete operations persist their changes before reporting success.
- The application creates or initializes its store when required and reports storage initialization or access failures clearly.
- The storage format and location are documented for operators.

### NFR-007 — Data Integrity

The system shall preserve valid student records and enforce the uniqueness and consistency rules defined in this document.

**Verification**

- A failed add, update, or delete does not leave the store in a partially changed state.
- Duplicate Student IDs cannot be persisted.
- Invalid or corrupted stored data is reported and is not silently replaced with an empty store.

### NFR-008 — Platform and Runtime

The application shall target .NET 10 and shall be buildable and runnable using the .NET 10 SDK on supported .NET platforms.

**Verification**

- A clean restore and build completes with the .NET 10 SDK.
- The application starts using the documented run instructions.

## 6. Out of Scope

- Multiple concurrent operators or network-shared storage.
- Authentication, authorization, and role-based permissions.
- Student self-service access.
- Attendance, grading, billing, and timetable management.
- Integration with external student information systems.
