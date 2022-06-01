CREATE TABLE Passports
(
	PassportId INT IDENTITY(101,1) PRIMARY KEY,
	PassportNumber NVARCHAR(20) NOT NULL
)

CREATE TABLE Persons
(
	PersonId INT IDENTITY PRIMARY KEY,
	FirstName NVARCHAR(50) NOT NULL,
	Salary DECIMAL(7, 2) NOT NULL,
  	PassportId INT FOREIGN KEY
    REFERENCES Passports(PassportId) UNIQUE NOT NULL
)

INSERT INTO Passports(PassportNumber)
	VALUES
('N34FG21B'),
('K65LO4R7'),
('ZE657QP2')

INSERT INTO Persons (FirstName, Salary,PassportId)
	VALUES
('Roberto', 43300.00, 102),
('Tom', 56100.00, 103),
('Yana', 60200.00, 101)
