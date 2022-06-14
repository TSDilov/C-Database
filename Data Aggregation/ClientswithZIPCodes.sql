SELECT 
	c.FirstName + ' ' + c.LastName AS FullName,
	a.Country,
	a.ZIP,
	CONCAT('$',MAX(cg.PriceForSingleCigar)) AS CigarPrice
FROM Clients c
JOIN Addresses a ON c.AddressId = a.Id
JOIN ClientsCigars cc ON c.Id = cc.ClientId
JOIN Cigars cg ON cc.CigarId = cg.Id
WHERE a.ZIP NOT LIKE '%[^0-9]%'
GROUP BY c.FirstName, c.LastName, a.Country, a.ZIP
ORDER BY FullName


