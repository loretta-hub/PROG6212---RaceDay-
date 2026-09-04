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
