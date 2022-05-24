CREATE DATABASE [CarRental]

USE [CarRental]

CREATE TABLE [Categories](
	[Id] INT PRIMARY KEY IDENTITY,
	[CategoryName] NVARCHAR(50) NOT NULL,
	[DailyRate] DECIMAL (6, 2),
	[WeeklyRate] DECIMAL (6, 2),
	[MonthlyRate] DECIMAL (6, 2),
	[WeekendRate] DECIMAL (6, 2)
)

CREATE TABLE [Cars](
	[Id] INT PRIMARY KEY IDENTITY,
	[PlateNumber] VARCHAR(20) NOT NULL,
	[Manufacturer] NVARCHAR(30) NOT NULL,
	[Model] NVARCHAR(30) NOT NULL,
	[CarYear] VARCHAR(4),
	[Doors] NVARCHAR(20),
	[Picture] VARBINARY(MAX),
	CHECK (DATALENGTH([Picture]) <= 2000000),
	[Condition] NVARCHAR(100),
	[Available] VARCHAR(5)
)

ALTER TABLE [Cars]
ADD [CategoryId] INT FOREIGN KEY REFERENCES [Categories] ([Id]) NOT NULL

CREATE TABLE [Employees](
	[Id] INT PRIMARY KEY IDENTITY,
	[FirstName] NVARCHAR(30) NOT NULL,
	[LastName] NVARCHAR(30) NOT NULL,
	[Title] NVARCHAR(30),
	[Notes] NVARCHAR(MAX)
)

CREATE TABLE [Customers](
	[Id] INT PRIMARY KEY IDENTITY,
	[DriverLicenceNumber] VARCHAR(15) NOT NULL,
	[FullName] NVARCHAR(50) NOT NULL,
	[Address] NVARCHAR(50) NOT NULL,
	[City] NVARCHAR(30) NOT NULL,
	[ZIPCode] VARCHAR(10),
	[Notes] NVARCHAR(MAX)
)

CREATE TABLE [RenalOrders](
	[Id] INT PRIMARY KEY IDENTITY,
	[TankLevel] DECIMAL (3,1),
	[KilometrageStart] DECIMAL(8, 2),
	[KilometrageEnd] DECIMAL(8,2),
	[TotalKilometrage] DECIMAL(8,2),
	[StartDate] DATE NOT NULL,
	[EndDate] DATE NOT NULL,
	[TotalDays] SMALLINT NOT NULL,
	[RateApplied] DECIMAL(6,2),
	[TaxRate] DECIMAL(6,2),
	[OrderStatus] VARCHAR(20) NOT NULL,
	[Notes] NVARCHAR(MAX)
)

ALTER TABLE [RenalOrders]
ADD [EmployeeId] INT FOREIGN KEY REFERENCES [Employees] ([Id]) NOT NULL

ALTER TABLE [RenalOrders]
ADD [CustomerId] INT FOREIGN KEY REFERENCES [Customers] ([Id]) NOT NULL

ALTER TABLE [RenalOrders]
ADD [CarId] INT FOREIGN KEY REFERENCES [Cars] ([Id]) NOT NULL

INSERT INTO [Categories] (CategoryName, DailyRate)
	VALUES
('Sedan', 120.00),
('Coupe', 100.00),
('Cabrio', 150.00)

INSERT INTO [Cars] (PlateNumber, Manufacturer, Model, [CategoryId])
	VALUES
('CV3167BP', 'Mercedes', 'CLK', 2),
('CV3387BP', 'BMW', '330', 1),
('CV4267BP', 'AUDI', 'A6', 3)

INSERT INTO [Employees] (FirstName, LastName)
	VALUES
('Ivan', 'Petrov'),
('Georgi', 'Ivanov'),
('Petar', 'Stoqnov')

INSERT INTO [Customers] (DriverLicenceNumber, FullName, Address, City)
	VALUES
('2315555', 'Maria Petrova', 'ul. Ivan Vazov', 'Plovdiv'),
('3215466', 'Silviq Petrova', 'ul. Hristo Botev', 'Sofiya'),
('2315555', 'MIhail Petrov', 'ul. Vasil Levski', 'Burgas')

INSERT INTO [RenalOrders] (StartDate, EndDate, TotalDays, OrderStatus, [EmployeeId], [CustomerId], [CarId])
	VALUES
('02-05-2015', '05-05-2015', 4, 'Payed', 1, 2, 3),
('02-06-2015', '07-06-2015', 6, 'Not Payed', 2, 3, 1),
('01-01-2015', '10-01-2015', 10, 'Payed', 3, 1, 2)

SELECT * FROM RenalOrders