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
