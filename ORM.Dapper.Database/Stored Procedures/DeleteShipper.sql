USE [Northwind];
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteShipper];
GO
CREATE PROCEDURE [dbo].[DeleteShipper]
@ShipperID INT
AS 
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM [dbo].[Shippers]
        WHERE [ShipperID]=@ShipperID;
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


    --DECLARE @ErrorVar INT;
    --DECLARE @RowCountVar INT;

    --DELETE FROM [dbo].[Shippers]
    --WHERE [ShipperID]=@ShipperID;

    --SELECT @ErrorVar=@@ERROR,@RowCountVar=@@ROWCOUNT;

    --IF @ErrorVar<>0
    --BEGIN

    --    -- Return 9999 to the calling program to indicate failure.  
    --    PRINT N'An error occurred deleting the shipper information.';

    --    RETURN 9999;
    --END;
    --ELSE
    --BEGIN

    --    -- Return RowCount to the calling program to indicate success.  
    --    PRINT N'The shipper has been deleted.';

    --    RETURN @RowCountVar;
    --END;
END;
GO