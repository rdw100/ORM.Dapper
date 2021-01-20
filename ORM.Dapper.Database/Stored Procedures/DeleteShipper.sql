Use Northwind
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteShipper]
GO
CREATE PROCEDURE [dbo].[DeleteShipper]
	@ShipperID int
AS
BEGIN
	DELETE FROM Shippers
	WHERE ShipperID = @ShipperID;
SELECT @@ROWCOUNT;
END;