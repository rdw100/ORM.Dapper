USE [Northwind];
GO
DROP PROCEDURE IF EXISTS [dbo].[GetShippers];
GO
CREATE PROCEDURE [dbo].[GetShippers]
AS 
BEGIN
    SET NOCOUNT ON;

    SELECT [ShipperID],[CompanyName],[Phone]
    FROM [dbo].[Shippers];

    RETURN @@ROWCOUNT;
END;
GO