USE [Northwind];
GO
DROP PROCEDURE IF EXISTS [dbo].[InsertShipper];
GO
CREATE PROCEDURE [dbo].[InsertShipper]
	@ShipperID   int output,
    @CompanyName nvarchar(40),
    @Phone       varchar (24)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        INSERT INTO [dbo].[Shippers]
            ([CompanyName],
             [Phone])
	    VALUES
            (@CompanyName,
             @Phone);

        SET @ShipperID = CAST(scope_identity() as INT);

        RETURN @ShipperID;
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
    END CATCH
END;
GO