CREATE FUNCTION ufn_CalculateFutureValue(@Sum DECIMAL(10,2), @Rate FLOAT, @Year INT)
RETURNS DECIMAL(10,4)
AS
BEGIN
	DECLARE @FutureValue DECIMAL(10,4) = @Sum * (POWER((1 + @Rate), @Year))
	RETURN @FutureValue
END

SELECT dbo.ufn_CalculateFutureValue(1000, 0.1, 5)