CREATE PROC usp_GetHoldersWithBalanceHigherThan (@MoreThan MONEY)
AS
SELECT 
	FirstName AS [First Name],
	LastName AS [Last Name]
FROM AccountHolders ah
JOIN Accounts a ON a.AccountHolderId = ah.Id
GROUP BY FirstName, LastName
HAVING SUM(a.Balance) > @MoreThan
ORDER BY FirstName, LastName

