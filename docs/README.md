# RaceDay Documentation

This folder contains the planning documents for the RaceDay project.

## Part 1 Documentation

The following documents are included:

- **RaceDay ERD.pdf** – Entity Relationship Diagram showing the database entities, attributes, keys and relationships.
- **API_Endpoint_Plan.md** – Complete API endpoint plan for the RaceDay RESTful API.
- **RaceDay_Database.sql** – SQL Server script for creating and populating the RaceDay database.

These documents form the planning foundation for Part 2, where the RESTful API will be developed.

## Technologies and Tools

The RaceDay project will use the following technologies and tools:

- C# and ASP.NET Core Web API for the RESTful API.
- SQL Server for the database.
- ASP.NET Core MVC for the web application.
- GitHub and GitHub Actions for source control and CI/CD.
- Azure Blob Storage for file storage in Part 3.
- Docker for containerisation in Part 3.
## Project Structure

## Project Structure
The project documentation is organised in the `Docs` folder. It contains the planning documents required for Part 1 of the RaceDay project.

- `RaceDay ERD.pdf` – Database Entity Relationship Diagram.
- `API_Endpoint_Plan.md` – RESTful API endpoint planning document.
- `RaceDay_Database.sql` – SQL Server database creation and population script.
- 
- ## Database

The RaceDay system uses SQL Server to store and manage information related to events, categories, participants and race results.

The database design is documented in the RaceDay ERD, while the `RaceDay_Database.sql` file contains the SQL script used to create and populate the database.

## RESTful API

The RaceDay system will provide a RESTful API to allow the application to manage and access race event information.

The API will support operations such as viewing events, managing event categories, registering participants and accessing race results. The planned endpoints are documented in the `API_Endpoint_Plan.md` file.

The RESTful API will be developed in Part 2 of the project.

## Organiser

Organisers are responsible for managing RaceDay events. They can create and manage events, set up event categories and manage participant results.

The Organiser role helps ensure that event information and results are kept accurate and up to date.

## Participant

Participants can browse available RaceDay events, view event and category information, enter events and track their race results.

The Participant role provides users with access to their event participation and performance history.

# Part 2: RaceDay Web API

## Project Overview

Part 2 implements the RaceDay event management system as an ASP.NET Core Web API. The API provides functionality for organisers to manage sporting events, categories and participant results, while participants can register, log in, browse events and manage their enrolments.

## Technologies Used

- C#
- ASP.NET Core Web API
- Entity Framework Core
- Microsoft SQL Server
- Swagger / OpenAPI
- xUnit for unit testing
- GitHub Actions for continuous integration

## Main Features

### Authentication and User Profiles
- User registration and login.
- Password hashing.
- Organiser and Participant roles.
- Viewing and updating user profiles.
- Session-based authentication and access control.

### Event Management
- View all events.
- View an individual event.
- Create, update and delete events.
- Manage event categories.

### Participant Enrolments
- Enrol participants in events.
- View enrolments.
- Prevent duplicate enrolments.
- Restrict access to enrolment information.

### Results Management
- Record participant finishing times and positions.
- View event results.
- Update and delete results subject to access permissions.

## Database

The API uses Microsoft SQL Server with Entity Framework Core.

The database includes the following main entities:

- User
- Event
- EventType
- Category
- Enrolment
- Result

The database structure follows the relationships established during Part 1.

## Running the API

1. Open the RaceDay solution in Visual Studio.
2. Check the SQL Server connection string in `appsettings.json`.
3. Ensure SQL Server is running and the configured database is available.
4. Restore the NuGet packages.
5. Build the solution.
6. Set `RaceDay.API` as the startup project.
7. Run the application.
8. Open the Swagger interface to explore and test the API endpoints.

Do not commit real passwords, database credentials or other secrets to the repository.

## Testing

The project includes an automated unit test project named `RaceDay.API.Tests`.

Tests can be run in Visual Studio using Test Explorer by selecting **Test → Run All Tests**.

The project should be built and tested before submission.

## Continuous Integration

GitHub Actions is configured to build the RaceDay API and run the configured unit tests when changes are pushed to the Part 2 branch.

The workflow is stored under `.github/workflows/`.

## Repository

The source code and project documentation are maintained in the RaceDay GitHub repository.

## Demonstration Video
The demonstration should show the running API, Swagger endpoints, authentication, role-based access, database integration and automated tests.