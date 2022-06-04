SElECT 
	c.CountryCode,
	m.MountainRange,
	p.PeakName,
	p.Elevation
	FROM Countries c
	JOIN MountainsCountries mc ON C.CountryCode = mc.CountryCode
	JOIN Mountains m ON mc.MountainId = m.Id
	JOIN Peaks p ON p.MountainId = m.Id
	WHERE c.CountryCode = 'BG' AND p.Elevation > 2835
	ORDER BY p.Elevation DESC