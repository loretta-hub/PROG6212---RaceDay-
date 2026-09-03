-- INSERT CATEGORIES
-- Categories for each event


INSERT INTO Category
    (EventsID, CategoryName, Description)
VALUES
-- Soweto Marathon
(1, 'Senior', 'Senior participants'),
(1, 'Under 20', 'Participants under the age of 20'),
(1, '40-49', 'Participants aged 40 to 49'),

-- Pretoria Charity Walk
(2, '10km Open', 'Open category for the 10km walk'),
(2, 'Senior', 'Senior participants'),
(2, 'Under 20', 'Participants under the age of 20'),

-- Cape Town Cycle Challenge
(3, 'Open', 'Open cycling category'),
(3, 'Veteran', 'Veteran cycling category'),
(3, 'Junior', 'Junior cycling category');
GO


-- INSERT SAMPLE ENROLMENTS


INSERT INTO Enrolment
    (ParticipantID, EventID, CategoryID)
VALUES
(3, 1, 1),
(4, 1, 2),
(3, 2, 4),
(4, 3, 7);
GO


-- INSERT SAMPLE RESULTS


INSERT INTO Result
    (EnrollmentID, FinishTime, FinishingPosition)
VALUES
(1, '03:45:20', 125),
(2, '04:10:15', 218),
(3, '01:20:30', 45);
GO


-- TEST THE DATABASE


SELECT * FROM [User];

SELECT * FROM EventType;

SELECT * FROM Event;

SELECT * FROM Category;

SELECT * FROM Enrolment;

SELECT * FROM Result;
GO
