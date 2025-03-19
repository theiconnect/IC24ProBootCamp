CREATE PROC dbo.usp_UpsertEmployee
	@EmployeeId	INT = NULL,
	@EmployeeFirstName VARCHAR(512) ,
	@EmployeeLastName VARCHAR(512) = NULL,
	@Email VARCHAR(512),
	@MobileNumber VARCHAR(20),
	@DepartmentIdFk INT = NULL,
	@Designation VARCHAR(512) = NULL,
	@PersonalEmail VARCHAR(512) = NULL,
	@Gender VARCHAR(10),
	@Salary decimal(10,2) = NULL,
	@PermanentAddressLine1 VARCHAR(512) = NULL,
	@PermanentAddressLine2 VARCHAR(512) = NULL,
	@PermanentCity VARCHAR(512)  = NULL,
	@PermanentState VARCHAR(512) = NULL,
	@PermanentPincode VARCHAR(512) = NULL,
	@AddressLine1 VARCHAR(512) = NULL,
	@AddressLine2 VARCHAR(512) = NULL,
	@City VARCHAR(512) = NULL,
	@State VARCHAR(512) = NULL,
	@Pincode VARCHAR(512) = NULL,
	@UserId	 VARCHAR(512)
AS
BEGIN
	IF ISNULL(@EmployeeId , 0) = 0
	BEGIN
		INSERT INTO Employee(
			EmployeeFirstName, EmployeeLastName, Email,
			MobileNumber,  DepartmentIdFk, Designation, PersonalEmail, Gender, SalaryCTC, 
			PermanentAddressLine1, PermanentAddressLine2, PermanentCity, PermanentState, 
			PermanentPincode, CurrentAddressLine1, CurrentAddressLine2, CurrentCity, CurrentState, CurrentPincode, 
			CreatedBy, CreatedOn)  
		SELECT @EmployeeFirstName, @EmployeeLastName,@Email,
			@MobileNumber, @DepartmentIdFk, @Designation, @PersonalEmail, @Gender, @Salary,
			@PermanentAddressLine1, @PermanentAddressLine2, @PermanentCity, @PermanentState,
			@PermanentPincode, @AddressLine1, @AddressLine2, @City, @State, @Pincode, 
			@UserId,GETDATE()

		SET @EmployeeId = SCOPE_IDENTITY();

		DECLARE @EmployeeCode VARCHAR(50)
		DECLARE @MaxEmpCode varchar(50)

		SELECT @MaxEmpCode = MAX(EmployeeCode) FROM Employee 
		IF ISNULL(@MaxEmpCode , '') = ''	
		BEGIN
			-- First emp
			SET @EmployeeCode = 'EMP-001'
		END
		ELSE
		BEGIN	
			--Not first emp
			DECLARE @MAXID INT
			SET @MAXID = CAST(REPLACE(@MaxEmpCode, 'EMP-', '') AS INT) + 1
			SET @EmployeeCode = 'EMP-' + RIGHT('00' + CAST(@MAXID AS VARCHAR), 3)			
		END

		UPDATE Employee SET EmployeeCode = @EmployeeCode WHERE EmployeeIdPk = @EmployeeId
	END
	ELSE
	BEGIN
		UPDATE 
			  Employee 
			SET 
			  EmployeeFirstName = @EmployeeFirstName, 
			  EmployeeLastName = @EmployeeLastName, 
			  Email = @Email, 
			  MobileNumber = @MobileNumber, 
			  DepartmentIdFk = @DepartmentIdFk, 
			  Designation = @Designation, 
			  PersonalEmail = @PersonalEmail, 
			  Gender = @Gender, 
			  SalaryCTC = @Salary, 
			  PermanentAddressLine1 = @PermanentAddressLine1, 
			  PermanentAddressLine2 = @PermanentAddressLine2, 
			  PermanentCity = @PermanentCity, 
			  PermanentState = @PermanentState, 
			  PermanentPincode = @PermanentPincode, 
			  CurrentAddressLine1 = @AddressLine1, 
			  CurrentAddressLine2 = @AddressLine2, 
			  CurrentCity = @City, 
			  CurrentState = @State, 
			  CurrentPincode = @Pincode, 
			  LastUpdatedBy = @UserId, 
			  LastUpdatedOn = GETDATE() 
			WHERE 
			  EmployeeIdPk = @EmployeeId

	END
END
