Use Northwind
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteShipper]
GO
CREATE PROCEDURE [dbo].[DeleteShipper]
	@ShipperId int
AS
BEGIN
	DELETE FROM Shippers
	WHERE ShipperId = @ShipperId;
SELECT @@ROWCOUNT;
END;