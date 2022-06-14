CREATE OR ALTER FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4)) 
RETURNS NVARCHAR(10)
AS
BEGIN
	DECLARE @Level NVARCHAR(10);
	IF @salary < 30000
		SET @Level = 'Low'
	ELSE IF (@salary BETWEEN 30000 AND 50000)
		SET @Level = 'Average'
	ELSE
		SET @Level = 'High'
	RETURN @Level
END

SELECT
	Salary,
	dbo.ufn_GetSalaryLevel(Salary) AS [Salary Level]
FROM Employees

SELECT dbo.ufn_GetSalaryLevel(13500)