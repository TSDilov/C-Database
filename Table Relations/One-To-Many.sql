CREATE DATABASE Demo

USE Demo

CREATE TABLE Manufacturers
(
	ManufacturerID INT IDENTITY PRIMARY KEY,
	NAME NVARCHAR(30) NOT NULL,
	EstablishedOn DATE NOT NULL
)

CREATE TABLE Models
(
	ModelId INT IDENTITY(101, 1) PRIMARY KEY,
	Name NVARCHAR(30) NOT NULL,
	ManufacturerID INT,
	CONSTRAINT FK_Models_Manufactures FOREIGN KEY (ManufacturerID)
	REFERENCES Manufacturers(ManufacturerID)
)

INSERT INTO Manufacturers(NAME, EstablishedOn)
	VALUES
('BMW', '07/03/1916'),
('Tesla', '01/01/2003'),
('Lada', '01/05/1966')

INSERT INTO Models (Name, ManufacturerID)
	VALUES
('X1', 1),
('i6', 1),
('Model S', 2),
('Model X', 2),
('Model 3', 2),
('Nova', 1)










