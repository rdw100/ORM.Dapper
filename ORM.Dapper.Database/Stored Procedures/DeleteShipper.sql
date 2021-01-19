DROP PROCEDURE IF EXISTS [dbo].[DeleteShipper]
GO
CREATE PROCEDURE [dbo].[DeleteShipper]
	@ShipperId int
AS
BEGIN
	DELETE FROM Shipper
	WHERE ShipperId = @ShipperId;
END;