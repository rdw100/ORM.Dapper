USE [Northwind];
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateShipper];
GO
CREATE PROCEDURE [dbo].[UpdateShipper]
	@ShipperID int,
	@CompanyName nvarchar(40),
    @Phone varchar (24)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
	    UPDATE	[dbo].[Shippers]
	    SET		[CompanyName]   = @CompanyName,
			    [Phone]         = @Phone
	    WHERE	[ShipperID]     = @ShipperID;
        RETURN @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        DECLARE @err INT = @@ERROR;                

        INSERT INTO [dbo].[Errors]
        VALUES
        (SUSER_SNAME(),
        ERROR_NUMBER(),
        ERROR_STATE(),
        ERROR_SEVERITY(),
        ERROR_LINE(),
        ERROR_PROCEDURE(),
        ERROR_MESSAGE(),
        GETDATE());

        IF @err <> 0
            RETURN 9999;
    END CATCH;
END;
GO