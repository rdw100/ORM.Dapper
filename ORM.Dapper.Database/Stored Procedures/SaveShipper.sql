Use Northwind
GO
DROP PROCEDURE IF EXISTS [dbo].[SaveShipper]
GO
CREATE PROCEDURE [dbo].[SaveShipper]
	@ShipperID int output,
	@CompanyName nvarchar(40),
    @Phone varchar (24)
AS
BEGIN
	UPDATE	Shippers
	SET		CompanyName   = @CompanyName,
			Phone         = @Phone
	WHERE	ShipperID     = @ShipperID;

	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO [dbo].[Shippers]
            ([CompanyName],
             [Phone])
		VALUES
           (@CompanyName,
            @Phone);
		SET @ShipperID = cast(scope_identity() as int);
	END;
END;
GO