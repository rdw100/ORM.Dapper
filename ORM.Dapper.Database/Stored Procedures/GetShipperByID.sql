USE [Northwind];
GO
DROP PROCEDURE IF EXISTS [dbo].[GetShipperByID];
GO
CREATE PROCEDURE [dbo].[GetShipperByID]
    @ShipperID INT
AS 
BEGIN
    SET NOCOUNT ON;

    SELECT [ShipperID],[CompanyName],[Phone]
    FROM [dbo].[Shippers]
    WHERE [ShipperID]=@ShipperID;

    RETURN @@ROWCOUNT;
END;
GO