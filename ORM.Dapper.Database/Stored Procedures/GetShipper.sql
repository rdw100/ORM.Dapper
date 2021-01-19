DROP PROCEDURE IF EXISTS [dbo].[GetShipper]
GO
CREATE procedure [dbo].[GetShipper]
	@ShipperId int
AS
BEGIN
	SELECT [ShipperId]
		  ,[CompanyName]
		  ,[Phone]
	  FROM [dbo].[Shipper]
	WHERE ShipperId = @ShipperId;
END