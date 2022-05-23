CREATE TABLE [People](
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL,
	[Picture] VARBINARY(MAX),
	CHECK (DATALENGTH([Picture]) <= 2000000),
	[Height] DECIMAL (3, 2),
	[Weight] DECIMAL (5, 2),
	[Gender] CHAR(1) NOT NULL
	CHECK ([Gender] = 'm' OR [Gender] = 'f'),
	[Birthday] DATE NOT NULL,
	[Biography] NVARCHAR(MAX)
)

INSERT INTO [People] ([Name], [Height], [Weight], [Gender], [Birthday])
	VALUES
('Pesho', 1.65, 72.2, 'm', '1995-02-27'),
('Silviq', 1.68, 42.3, 'm', '1998-06-21'),
('Gosho', 1.85, NULL, 'm', '1991-09-15'),
('Mariq', NULL, NULL, 'f', '1999-11-09'),
('Nasko', 1.87, 92.2, 'm', '1992-07-01')

SELECT * FROM [People]