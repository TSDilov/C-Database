SELECT 
	subQueryForRanks.CountryName,
	subQueryForRanks.[Highest Peak Name],
	subQueryForRanks.[Highest Peak Elevation],
	subQueryForRanks.Mountain
	FROM
(
	SELECT 
		CountryName,
		ISNULL(p.PeakName, '(no highest peak)') AS [Highest Peak Name],
		ISNULL(MAX(p.Elevation), 0) AS [Highest Peak Elevation],
		ISNULL(m.MountainRange, '(no mountain)') AS Mountain,
		DENSE_RANK() OVER (PARTITION BY CountryName ORDER BY MAX(p.Elevation) DESC) AS Rank
		FROM Countries c
		LEFT JOIN MountainsCountries mc ON mc.CountryCode = c.CountryCode
		LEFT JOIN Mountains m ON m.Id = mc.MountainId
		LEFT JOIN Peaks p on p.MountainId = m.Id
		GROUP BY c.CountryName, p.PeakName, m.MountainRange
) AS subQueryForRanks
WHERE Rank = 1
ORDER BY CountryName, [Highest Peak Name]
