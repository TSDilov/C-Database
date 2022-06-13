CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(10), @word VARCHAR(10)) 
RETURNS BIT
AS
BEGIN
	DECLARE @Iterator INT = 1
	WHILE @Iterator <= LEN(@word)
	BEGIN
		IF CHARINDEX(SUBSTRING(@word,@Iterator,1), @setOfLetters) = 0
			RETURN 0
		SET @Iterator += 1		
	END
	RETURN 1
END

SELECT dbo.ufn_IsWordComprised('oistmiahf', 'Sofia')


