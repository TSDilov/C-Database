CREATE TABLE [Users](
	[Id] BIGINT PRIMARY KEY IDENTITY,
	[Username] VARCHAR(30) UNIQUE NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	[ProfilePicture] VARBINARY(MAX),
	CHECK (DATALENGTH([ProfilePicture]) <= 900000),
	[LastLoginTime] DATETIME2,
	[IsDeleted] VARCHAR(5),
	CHECK ([IsDeleted] = 'true' OR [IsDeleted] = 'false'),
)

INSERT INTO [Users] ([Username], [Password], [LastLoginTime], [IsDeleted])
	VALUES
('Pesho', '2536', NULL, 'false'),
('Silviq', '3526', NULL, 'true'),
('Gosho', '32114', NULL, 'true'),
('Mariq', '14DASD', NULL, 'false'),
('Nasko', 'DASDSA', NULL, 'true')

SELECT * FROM [Users]

ALTER TABLE [Users]
ADD PRIMARY KEY (Id)

ALTER TABLE [Users]
ADD CONSTRAINT PK_User PRIMARY KEY ([Id], [Username])

ALTER TABLE [Users]
ADD CONSTRAINT DF_DefaultLastLoginTime DEFAULT ('2022-05-22') FOR [LastLoginTime]

INSERT INTO [Users] ([Username], [Password], [IsDeleted])
	VALUES
('Misho', '2536', 'false')

SELECT * FROM [Users]

