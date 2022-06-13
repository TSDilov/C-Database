CREATE PROC usp_EmployeesBySalaryLevel (@Level NVARCHAR(10))
AS
	SELECT
		FirstName,
		LastName
	FROM Employees
	WHERE dbo.ufn_GetSalaryLevel(Salary) = @Level
GO

EXEC usp_EmployeesBySalaryLeveL 'High'