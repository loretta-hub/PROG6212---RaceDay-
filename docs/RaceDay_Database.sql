
-- RaceDay Database
-- Section C: SQL Database Script
-- Purpose: Creates and populates the RaceDay database
-- Entities :User, Event, EventType, Category, Enrolment, Result

-- Create the RaceDay database
CREATE DATABASE RaceDay;
GO

-- Select the RaceDay database
USE RaceDay;
GO


-- 1. USER TABLE


CREATE TABLE [User]
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Role VARCHAR(20) NOT NULL,
    PhoneNumber VARCHAR(20),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CK_User_Role
        CHECK (Role IN ('Organiser', 'Participant'))
);
GO



-- 2. EVENT TYPE TABLE

CREATE TABLE EventType
(
    EventTypeID INT IDENTITY(1,1) PRIMARY KEY,
    TypeName VARCHAR(20) NOT NULL UNIQUE
);
GO


-- 3. EVENT TABLE

CREATE TABLE Event
(
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    OrganiserID INT NOT NULL,
    EventTypeID INT NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(500),
    EventDate DATE NOT NULL,
    Location VARCHAR(150) NOT NULL,
    Distance DECIMAL(6,2) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Event_Organiser
        FOREIGN KEY (OrganiserID)
        REFERENCES [User](UserID),

    CONSTRAINT FK_Event_EventType
        FOREIGN KEY (EventTypeID)
        REFERENCES EventType(EventTypeID),

    CONSTRAINT CK_Event_Distance
        CHECK (Distance > 0)
);
GO



-- 4. CATEGORY TABLE

CREATE TABLE Category
(
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    EventsID INT NOT NULL,
    CategoryName VARCHAR(100) NOT NULL,
    Description VARCHAR(255),

    CONSTRAINT FK_Category_Event
        FOREIGN KEY (EventsID)
        REFERENCES Event(EventID)
);
GO



-- 5. ENROLMENT TABLE

CREATE TABLE Enrolment
(
    EnrolmentID INT IDENTITY(1,1) PRIMARY KEY,
    ParticipantID INT NOT NULL,
    EventID INT NOT NULL,
    CategoryID INT NOT NULL,
    EnrolmentDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Enrolment_Participant
        FOREIGN KEY (ParticipantID)
        REFERENCES [User](UserID),

    CONSTRAINT FK_Enrolment_Event
        FOREIGN KEY (EventID)
        REFERENCES Event(EventID),

    CONSTRAINT FK_Enrolment_Category
        FOREIGN KEY (CategoryID)
        REFERENCES Category(CategoryID),

    CONSTRAINT UQ_Enrolment_Participant_Event
        UNIQUE (ParticipantID, EventID)
);
GO



-- 6. RESULT TABLE

CREATE TABLE Result
(
    ResultID INT IDENTITY(1,1) PRIMARY KEY,
    EnrollmentID INT NOT NULL,
    FinishTime TIME NOT NULL,
    FinishingPosition INT NOT NULL,
    RecordedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Result_Enrolment
        FOREIGN KEY (EnrollmentID)
        REFERENCES Enrolment(EnrolmentID),

    CONSTRAINT UQ_Result_Enrolment
        UNIQUE (EnrollmentID),

    CONSTRAINT CK_Result_Position
        CHECK (FinishingPosition > 0)
);
GO



-- INSERT EVENT TYPES


INSERT INTO EventType (TypeName)
VALUES
('Run'),
('Walk'),
('Cycle');
GO

-- INSERT USERS
-- 2 Organisers
-- 2 Participants


INSERT INTO [User]
(
    FirstName,
    LastName,
    Email,
    PasswordHash,
    Role,
    PhoneNumber
)
VALUES
(
    'Thabo',
    'Mokoena',
    'thabo.organiser@example.com',
    'HASHED_PASSWORD_001',
    'Organiser',
    '0712345678'
),
(
    'Lerato',
    'Nkosi',
    'lerato.organiser@example.com',
    'HASHED_PASSWORD_002',
    'Organiser',
    '0723456789'
),
(
    'Sipho',
    'Dlamini',
    'sipho.participant@example.com',
    'HASHED_PASSWORD_003',
    'Participant',
    '0734567890'
),
(
    'Amahle',
    'Ndlovu',
    'amahle.participant@example.com',
    'HASHED_PASSWORD_004',
    'Participant',
    '0745678901'
);
GO



-- INSERT EVENTS
-- 3 EVENTS


INSERT INTO Event
(
    OrganiserID,
    EventTypeID,
    Name,
    Description,
    EventDate,
    Location,
    Distance
)
VALUES
(
    1,
    1,
    'Soweto Marathon',
    'Annual road running event in Soweto.',
    '2026-11-01',
    'Soweto, Johannesburg',
    42.20
),
(
    1,
    2,
    'Pretoria Charity Walk',
    'Community charity walking event.',
    '2026-10-10',
    'Pretoria, Gauteng',
    10.00
),
(
    2,
    3,
    'Cape Town Cycle Challenge',
    'Road cycling event around Cape Town.',
    '2026-12-06',
    'Cape Town, Western Cape',
    109.00
);
GO



-- INSERT CATEGORIES
-- Categories for each event

INSERT INTO Category
(
    EventsID,
    CategoryName,
    Description
)
VALUES
-- Soweto Marathon
(
    1,
    'Senior',
    'Senior participants'
),
(
    1,
    'Under 20',
    'Participants under the age of 20'
),
(
    1,
    '40-49',
    'Participants aged 40 to 49'
),

-- Pretoria Charity Walk
(
    2,
    '10km Open',
    'Open category for the 10km walk'
),
(
    2,
    'Senior',
    'Senior participants'
),
(
    2,
    'Under 20',
    'Participants under the age of 20'
),

-- Cape Town Cycle Challenge
(
    3,
    'Open',
    'Open cycling category'
),
(
    3,
    'Veteran',
    'Veteran cycling category'
),
(
    3,
    'Junior',
    'Junior cycling category'
);
GO



-- INSERT SAMPLE ENROLMENTS


INSERT INTO Enrolment
(
    ParticipantID,
    EventID,
    CategoryID
)
VALUES
(
    3,
    1,
    1
),
(
    4,
    1,
    2
),
(
    3,
    2,
    4
),
(
    4,
    3,
    7
);
GO



-- INSERT SAMPLE RESULTS

INSERT INTO Result
(
    EnrollmentID,
    FinishTime,
    FinishingPosition
)
VALUES
(
    1,
    '03:45:20',
    125
),
(
    2,
    '04:10:15',
    218
),
(
    3,
    '01:20:30',
    45
);
GO


-- TEST THE DATABASE


SELECT * FROM [User];

SELECT * FROM EventType;

SELECT * FROM Event;

SELECT * FROM Category;

SELECT * FROM Enrolment;



-- Verification Queries

SELECT 'Users' AS TableName, COUNT(*) AS RecordCount FROM [User]
UNION ALL
SELECT 'Events', COUNT(*) FROM Event
UNION ALL
SELECT 'Categories', COUNT(*) FROM Category
UNION ALL
SELECT 'Enrolments', COUNT(*) FROM Enrolment
UNION ALL
SELECT 'Results', COUNT(*) FROM Result;

SELECT * FROM Result;
GO
