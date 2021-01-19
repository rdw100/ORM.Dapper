DROP PROCEDURE IF EXISTS [dbo].[SaveShipper]
GO
CREATE PROCEDURE [dbo].[SaveShipper]
	@ShipperId int output,
	@CompanyName nvarchar(40),
    @Phone varchar (24)
AS
BEGIN
	UPDATE	Shipper
	SET		CompanyName   = @CompanyName,
			Phone         = @Phone
	WHERE	ShipperId     = @ShipperId

	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO [dbo].[Shipper]
            ([CompanyName],
             [Phone])
		VALUES
           (@CompanyName,
            @Phone);
		SET @ShipperId = cast(scope_identity() as int)
	END;
END;
GO