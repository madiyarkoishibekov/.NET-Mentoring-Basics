CREATE PROCEDURE [dbo].[SaveEmployeeInfo]
	@EmployeeName nvarchar(100) = NULL,
	@FirstName nvarchar(50) = NULL,
	@LastName nvarchar(50) = NULL,
	@CompanyName nvarchar(20),
	@Position nvarchar(30) = NULL,
	@Street nvarchar(50),
	@City nvarchar(20) = NULL,
	@State nvarchar(50) = NULL,
	@ZipCode nvarchar(50) = NULL
AS
	DECLARE @AddressId INT, @PersonId INT;

	IF (LTRIM(RTRIM(ISNULL(@EmployeeName,''))) = '')
		AND (LTRIM(RTRIM(ISNULL(@FirstName,''))) = '')
		AND (LTRIM(RTRIM(ISNULL(@LastName,''))) = '')
	BEGIN
		PRINT 'No names were submitted.';
		RETURN;
	END;

	BEGIN TRAN
		SET @CompanyName = SUBSTRING(@CompanyName, 1, 20);

		-- Fill Address table
		SELECT @AddressId = a.[Id] FROM [Address] a 
		WHERE a.[Street] = @Street 
			AND a.[City] = @City
			AND a.[State] = @State
			AND a.[ZipCode] = @ZipCode;
		
		IF @AddressId IS NULL
		BEGIN
			INSERT INTO [Address] ([Street], [City], [State], [ZipCode]) 
			VALUES (@Street, @City, @State, @ZipCode);
			SELECT @AddressId = SCOPE_IDENTITY();	
		END;
		

		-- Fill Person table
		SELECT @PersonId = p.[Id] FROM [Person] p 
		WHERE p.FirstName = ISNULL(@FirstName,'') 
			  AND p.LastName = ISNULL(@LastName,'');
		
		IF @PersonId IS NULL
		BEGIN
			INSERT INTO [Person] ([FirstName], [LastName])
			VALUES (ISNULL(@FirstName,''), ISNULL(@LastName,''));
			SELECT @PersonId = SCOPE_IDENTITY();	
		END

		-- Fill Employee table
		INSERT INTO [Employee] ([AddressId], [PersonId], [CompanyName], [Position], [EmployeeName])
		VALUES (@AddressId, @PersonId, @CompanyName, @Position, @EmployeeName);

	COMMIT
RETURN 0
