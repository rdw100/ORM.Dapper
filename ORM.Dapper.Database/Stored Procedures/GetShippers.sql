Use Northwind
GO
DROP PROCEDURE IF EXISTS [dbo].[GetShippers]
GO
CREATE procedure [dbo].[GetShippers]
AS
BEGIN
	SELECT *
	  FROM [dbo].[Shippers];
END