Use Northwind
GO
DROP PROCEDURE IF EXISTS [dbo].[GetShipperByID]
GO
CREATE procedure [dbo].[GetShipperByID]
	@ShipperID int
AS
BEGIN
	SELECT [ShipperID]
		  ,[CompanyName]
		  ,[Phone]
	  FROM [dbo].[Shippers]
	WHERE ShipperID = @ShipperID;
END