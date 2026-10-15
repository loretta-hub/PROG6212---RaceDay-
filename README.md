# RaceDay

## Project Description

RaceDay is a web-based event management system designed for the South African road running, walking and cycling community. The system allows Organisers to manage events, categories, participant enrolments and results. Participants can browse events, enter events and track their results.

## User Roles

### Organiser
Organisers can:
- Create, update and delete events.
- Manage event categories.
- View participant enrolments.
- Capture participant results.

### Participant
Participants can:
- Create an account and log in.
- Browse available events.
- Enter an event by selecting a category.
- View their own enrolments.
- View their personal results.

## Part 1

Part 1 focuses on planning the RaceDay system before application development. The planning includes:

- Entity Relationship Diagram (ERD)
- API Endpoint Plan
- SQL Database Script

## Documentation

The planning documents are stored in the `/docs` folder.

## CI/CD

A GitHub Actions workflow will be used to validate the required project structure.

## Video Presentation

YouTube video link will be added after the Part 1 presentation has been recorded.
## Part 1 Status

The Part 1 planning documents have been completed and added to the `/docs` folder.

The GitHub Actions workflow also validates that the required Part 1 documentation files are present in the repository.
## Part 2: Support API Development

The RaceDay Support API provides backend functionality for the running, walking and cycling event platform.

### Development objectives
- Provide endpoints for user registration, login and profile management.
- Support event and category management.
- Allow participants to manage event enrolments.
- Support the capture and retrieval of event results.
- Apply validation and appropriate HTTP responses.
- Verify API functionality through testing.

- ## API Testing and Validation

The RaceDay Support API must be tested to ensure that its endpoints behave as expected.

### Testing considerations
- Verify that valid requests return the expected responses.
- Check that invalid input is handled appropriately.
- Confirm that missing or incorrect authentication is rejected where required.
- Verify that organiser and participant permissions are enforced.
- Check that unsuccessful requests return appropriate HTTP status codes.
- Confirm that API endpoints operate correctly with the configured database.
## API Security and Access Control

Security is an important requirement of the RaceDay Support API because the platform supports both organisers and participants.

### Security requirements
- Authenticate users before allowing access to protected endpoints.
- Apply role-based authorization to restrict access to organiser and participant functions.
- Validate incoming data before processing requests.
- Avoid exposing passwords or other sensitive information in API responses.
- Return appropriate HTTP status codes for unauthorized and forbidden requests.
- Handle errors without exposing internal application details.

Organisers should only access functions permitted by their role, while participants should only access their own enrolments and results where applicable.

Testing helps identify errors, improve reliability and ensure that the API meets the functional requirements of the RaceDay platform.

### Development Status

- The RaceDay Support API builds successfully.
- The API project is configured to use SQL Server and Entity Framework Core.
- API endpoints can be explored using Swagger.
- GitHub Actions is used to validate the project through continuous integration.
- Further development and testing will be completed according to the Part 2 requirements.

- ## Part 2: API Endpoint Reference

The RaceDay Support API is designed to provide backend services for authentication, user profiles, event management, categories, participant enrolments and results.

The following table summarises the 25 planned API endpoints.

### Authentication — 2 endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Authenticate a user |

### User Profiles — 2 endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/users/profile` | Retrieve the authenticated user's profile |
| PUT | `/api/users/profile` | Update the authenticated user's profile |

### Events — 5 endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/events` | Retrieve all events |
| GET | `/api/events/{id}` | Retrieve a specific event |
| POST | `/api/events` | Create an event |
| PUT | `/api/events/{id}` | Update an event |
| DELETE | `/api/events/{id}` | Delete an event |

### Categories — 5 endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/events/{eventId}/categories` | Retrieve categories for an event |
| GET | `/api/categories/{id}` | Retrieve a specific category |
| POST | `/api/categories` | Create a category |
| PUT | `/api/categories/{id}` | Update a category |
| DELETE | `/api/categories/{id}` | Delete a category |

### Enrolments — 5 endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/enrolments` | Retrieve enrolments |
| GET | `/api/events/{eventId}/enrolments` | Retrieve enrolments for an event |
| GET | `/api/enrolments/{id}` | Retrieve a specific enrolment |
| POST | `/api/enrolments` | Enrol a participant in an event |
| DELETE | `/api/enrolments/{id}` | Cancel an enrolment |

### Results — 6 endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/results` | Retrieve results |
| GET | `/api/results/{id}` | Retrieve a specific result |
| GET | `/api/events/{eventId}/results` | Retrieve results for an event |
| POST | `/api/results` | Capture a result |
| PUT | `/api/results/{id}` | Update a result |
| DELETE | `/api/results/{id}` | Delete a result |

### Endpoint Security and Validation

Endpoints must enforce appropriate authentication and role-based authorisation. Input validation and suitable HTTP status codes should be used to support reliable API operations.

The endpoint reference must be checked against the implemented controllers and the Part 1 API Endpoint Plan. Planned endpoints are not considered implemented until their functionality has been developed and tested.
