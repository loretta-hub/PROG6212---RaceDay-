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

### Development status
The API project builds successfully. Development and testing will continue as the endpoints and supporting functionality are completed.

# RaceDay – Event Management System

## 1. Project Overview

RaceDay is a web-based event management system developed for running, walking and cycling events in South Africa.

The system provides two main user roles: Organisers and Participants.

Organisers can create and manage sporting events, manage categories, view participant enrolments and capture event results. Participants can browse events, enrol in events, view their enrolments and results, and manage their profiles.

## 2. Technologies Used

The project uses the following technologies:

- C#
- ASP.NET Core Web API
- ASP.NET Core MVC
- Entity Framework Core
- Microsoft SQL Server
- HTML and CSS
- Bootstrap
- JavaScript
- Azure Blob Storage integration
- Git and GitHub

## 3. Main System Features

### Organiser Features

- Organiser registration and login.
- Organiser dashboard.
- Create, update and delete events.
- Manage event categories.
- View participant enrolments.
- Capture event results.
- Upload event banner images.

### Participant Features

- Participant registration and login.
- Participant dashboard.
- Browse available events.
- View event information.
- Enrol in events.
- View personal enrolments.
- View event results.
- Update profile information.
- Upload profile pictures.

## 4. Project Architecture

RaceDay is divided into two main application components:

**RaceDay.API**

The API handles data access, authentication, event management, enrolments, categories, results and image storage operations.

**RaceDay.Web**

The MVC application provides the user interface. It communicates with the API through HTTP requests.

The MVC application does not access the database directly. This separation helps maintain a clear distinction between the presentation layer and backend functionality.

## 5. Database

The application uses Microsoft SQL Server to store information such as:

- Users
- Events
- Event types
- Categories
- Enrolments
- Results

The development database connection is configured in the API's `appsettings.json` file.

Example local SQL Server Express connection string:

```json
"ConnectionStrings": {
  "RaceDayConnection": "Server=.\\SQLEXPRESS;Database=RaceDay;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Use the supplied database SQL script in the `docs` folder when setting up the database.

## 6. Requirements

To run the project locally, you need:

- Visual Studio 2022.
- .NET 8 SDK.
- Microsoft SQL Server Express.
- SQL Server Management Studio (SSMS).
- Git.
- Azurite for local Azure Blob Storage development and testing, where required.

## 7. How to Run the Application

### Step 1: Clone the repository

Clone the RaceDay repository from GitHub:

```bash
git clone https://github.com/loretta-hub/PROG6212---RaceDay-.git
```

### Step 2: Open the solution

Open the RaceDay solution in Visual Studio.

### Step 3: Configure the database

Make sure SQL Server Express is running.

Check the API connection string in `appsettings.json` and ensure it matches your local SQL Server configuration.

Create and prepare the RaceDay database using the supplied SQL script and the project's database setup instructions.

### Step 4: Configure image storage

The project includes Azure Blob Storage integration.

For local development, the storage configuration can use Azurite:

```json
"AzureBlobStorage": {
  "ConnectionString": "UseDevelopmentStorage=true",
  "ContainerName": "raceday-images"
}
```

Start Azurite before testing image uploads.

For a real Azure deployment, configure an appropriate Azure Storage connection string securely.

### Step 5: Run the projects

Run both projects:

1. `RaceDay.API`
2. `RaceDay.Web`

In Visual Studio, right-click the solution, select **Configure Startup Projects**, choose **Multiple startup projects**, and set both projects to Start.

The API development address used in this project is:

`https://localhost:7155`

The MVC application provides the website interface.

## 8. Authentication and Security

RaceDay supports role-based access for Organisers and Participants.

Passwords are hashed before being stored in the database.

Session-based authentication is used to maintain login sessions and restrict access to role-specific functionality.

## 9. Project Documentation

The repository includes supporting documentation for the RaceDay project, including database design and API planning documents.

Refer to the `docs` folder for the available project documentation.

## 10. Future Improvements

Potential future improvements include:

- Completing Docker containerisation.
- Deploying the application to a cloud hosting environment.
- Expanding automated testing.
- Further improving responsive design and user experience.

## 11. Author

**Loretta Mukhethwa Mamathuntsha**

PROG6212 – Programming 2B

RaceDay Event Management System

## 12. Testing and Verification

The RaceDay application should be tested to verify that its main features operate correctly.

The following areas should be checked during testing:

### Authentication
- Register a new Organiser account.
- Register a new Participant account.
- Log in using valid credentials.
- Confirm that each user is directed to the correct dashboard.

### Organiser Functionality
- Create a sporting event.
- Update event information.
- Delete an event.
- View event enrolments.
- Manage event categories.
- Capture event results.
- Test event banner uploads.

### Participant Functionality
- Browse available events.
- View event details.
- Enrol in an event.
- View personal enrolments and results.
- Update profile information.
- Test profile picture uploads.

### Technical Verification
- Build the API and MVC projects.
- Verify API communication from the MVC application.
- Check database records after relevant operations.
- Test image uploads using the configured storage service.
- Review application errors and validation messages.

Testing should be performed before the final project submission.

## 13. API Communication

RaceDay uses a RESTful Web API to handle communication between the MVC application and the database.

The MVC application sends HTTP requests to the API to perform operations. The API processes these requests and returns responses to the MVC application.

### Main API Areas

| API Area | Purpose |
|---|---|
| Authentication | Registration, login and logout |
| Events | Create, retrieve, update and delete events |
| Categories | Manage event categories |
| Enrolments | Manage participant event enrolments |
| Results | Capture and retrieve event results |
| Users | Retrieve and update user profile information |

### Communication Process

1. A user performs an action on the MVC website.
2. The MVC controller sends a request through the API service.
3. The API receives and processes the request.
4. The API communicates with the SQL Server database where necessary.
5. The API returns a response.
6. The MVC application displays the result to the user.

This architecture separates presentation functionality from backend processing and database operations.
## 14. User Roles and Permissions

RaceDay uses role-based access to provide different features to Organisers and Participants. Users must log in before accessing role-specific functionality.

### Organiser

Organisers are responsible for managing events and overseeing event participation.

Organiser functionality includes:
- Accessing the Organiser Dashboard.
- Creating, viewing, updating and deleting events.
- Managing event categories.
- Viewing participant enrolments.
- Capturing and managing event results.
- Uploading event banner images.

### Participant

Participants use RaceDay to discover events and manage their participation.

Participant functionality includes:
- Accessing the Participant Dashboard.
- Browsing available events.
- Viewing event details and categories.
- Enrolling in events.
- Viewing and managing their enrolments.
- Viewing event results.
- Updating their profile information.
- Uploading a profile picture.

### Access Control

RaceDay uses session-based authentication and role checks to restrict access to protected functionality. Organiser features are intended for Organiser accounts, while participant features are intended for Participant accounts.
## 15. Technologies and Development Tools

The RaceDay application uses the following technologies and tools:

| Technology / Tool | Purpose |
|---|---|
| C# | Main programming language used for the application. |
| ASP.NET Core Web API | Handles application data and backend operations. |
| ASP.NET Core MVC | Provides the user-facing web application. |
| Entity Framework Core | Supports database operations in the API. |
| Microsoft SQL Server | Stores RaceDay application data. |
| Bootstrap | Supports responsive page layouts and styling. |
| HTML and CSS | Structure and style the web pages. |
| JavaScript | Supports client-side interactions. |
| Azure Blob Storage | Stores uploaded profile pictures and event banner images. |
| Visual Studio | Main development environment. |
| Git and GitHub | Source control and project collaboration. |
| xUnit | Used for automated unit testing. |
| Docker | Intended for application containerisation. |

These technologies work together to support the development, maintenance and operation of RaceDay.

## 16. Project Structure

The RaceDay solution is organised into separate projects to keep the user interface, backend operations and automated tests separate.

### RaceDay.API
Contains the backend functionality, including:
- Controllers for handling API requests.
- Models representing application data.
- Database context and Entity Framework Core configuration.
- Services for application functionality, including image storage.
- Authentication and session-based access control.

### RaceDay.Web
Contains the MVC web application, including:
- Controllers that manage web page requests.
- Views that display the user interface.
- ViewModels used to transfer data between views and controllers.
- API service classes that communicate with the backend.
- Session-based user access and navigation.

### RaceDay.API.Tests
Contains automated tests used to verify selected API functionality.

### Documentation
The documentation includes project planning and supporting information, such as the Entity Relationship Diagram, API Endpoint Plan and database script.

Separating the API, MVC application and tests helps keep the project organised and easier to maintain.
