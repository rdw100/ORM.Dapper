Use Northwind
GO
DROP PROCEDURE IF EXISTS [dbo].[GetShipperByID]
GO
CREATE procedure [dbo].[GetShipperByID]
	@ShipperId int
AS
BEGIN
	SELECT [ShipperId]
		  ,[CompanyName]
		  ,[Phone]
	  FROM [dbo].[Shippers]
	WHERE ShipperId = @ShipperId;
END