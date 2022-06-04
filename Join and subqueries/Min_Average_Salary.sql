SELECT
	MIN(A.AverageSalary) AS MinAverageSalary
	FROM
	(
		SELECT
			e.DepartmentID,
			AVG(e.Salary) AS AverageSalary
			FROM Employees e
			GROUP BY e.DepartmentID
	) AS a
	


