CREATE OR ALTER PROC usp_GetEmployeesFromTown(@Town VARCHAR(20))
AS
	SELECT 
		e.FirstName,
		e.LastName
	FROM Employees e
	JOIN Addresses a ON e.AddressID = a.AddressID
	JOIN Towns t ON a.TownID = t.TownID
	WHERE t.Name = @Town

EXEC usp_GetEmployeesFromTown 'Sofia'