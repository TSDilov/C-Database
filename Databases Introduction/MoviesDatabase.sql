CREATE DATABASE [Movies]

USE [Movies]

CREATE TABLE [Directors](
	[Id] INT PRIMARY KEY IDENTITY,
	[DirectorName] NVARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(MAX)	
)

CREATE TABLE [Genres](
	[Id] INT PRIMARY KEY IDENTITY,
	[GenreName] NVARCHAR(30) NOT NULL,
	[Notes] NVARCHAR(MAX)	
)

CREATE TABLE [Categories](
	[Id] INT PRIMARY KEY IDENTITY,
	[CategoryName] NVARCHAR(30) NOT NULL,
	[Notes] NVARCHAR(MAX)	
)

CREATE TABLE [Movies](
	[Id] INT PRIMARY KEY IDENTITY,
	[Title] NVARCHAR(100) NOT NULL,
	[CopyrightYear] DATE NOT NULL,
	[Length] INT,
	[Notes] NVARCHAR(MAX)	
)

ALTER TABLE [Movies]
ADD [DirectorId] INT FOREIGN KEY REFERENCES [Directors] ([Id]) NOT NULL

ALTER TABLE [Movies]
ADD [GenreId] INT FOREIGN KEY REFERENCES [Genres] ([Id]) NOT NULL

ALTER TABLE [Movies]
ADD [CategoryId] INT FOREIGN KEY REFERENCES [Categories] ([Id]) NOT NULL

INSERT INTO [Directors] ([DirectorName], [Notes])
	VALUES
('Pesho', NULL),
('Silviq', 'Verry Good'),
('Gosho', 'Nice'),
('Mariq', NULL),
('Nasko', 'Excellent')

INSERT INTO [Genres] ([GenreName], [Notes])
	VALUES
('Action', 'Shooting'),
('Comedy', 'Fun'),
('Romance', 'Kissing'),
('Horror', 'Scary'),
('Documental', 'Nature')

INSERT INTO [Categories] ([CategoryName], [Notes])
	VALUES
('A', NULL),
('B', 'Verry Good'),
('C', 'Nice'),
('D', NULL),
('E', 'Excellent')

INSERT INTO [Movies] ([Title], [CopyrightYear], [DirectorId], [GenreId], [CategoryId])
	VALUES
('Rambo', '1990', 1, 1, 2),
('Bears', '2000', 2, 5, 1),
('Alf', '1989', 3, 2, 3),
('Jigsal', '1999', 4, 4, 5),
('The Wedding', '2005', 5, 3, 4)


