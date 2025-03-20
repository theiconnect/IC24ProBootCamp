CREATE PROC usp_UpsertEmployee
	@EmployeeId	INT = NULL,
	@EmployeeFirstName VARCHAR(512) ,
	@EmployeeLastName VARCHAR(512) = null,
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
	@UserId	 VARCHAR(512) = 'iConnect'
AS
BEGIN
	IF ISNULL(@EmployeeId , 0) = 0
	BEGIN
		DECLARE @EmployeeCode VARCHAR(50)
		DECLARE @MaxEmpCode varchar(50)

		SELECT @MaxEmpCode = MAX(EmployeeCode) FROM Employee 
		IF ISNULL(@MaxEmpCode , '') = ''	
		BEGIN
			-- First emp
			SET @EmployeeCode = 'EMP-0001'
		END
		ELSE
		BEGIN	
			--Not first emp
			DECLARE @MAXID INT
			SET @MAXID = CAST(REPLACE(@MaxEmpCode, 'EMP-', '') AS INT) + 1
			SET @EmployeeCode = 'EMP-' + RIGHT('000' + CAST(@MAXID AS VARCHAR), 4)			
		END

		DECLARE @ActiveStatusId INT
		select @ActiveStatusId = StatusIdPk from StatusMaster where StatusCode = 'ACTIVE'

		INSERT INTO Employee(
			EmployeeCode, EmployeeFirstName, EmployeeLastName, Email,
			MobileNumber,  DepartmentIdFk, Designation, PersonalEmail, Gender, SalaryCTC, 
			PermanentAddressLine1, PermanentAddressLine2, PermanentCity, PermanentState, 
			PermanentPincode, CurrentAddressLine1, CurrentAddressLine2, CurrentCity, CurrentState, CurrentPincode, 
			CreatedBy, CreatedOn, StatusIdFk)  
		SELECT @EmployeeCode, @EmployeeFirstName, @EmployeeLastName,@Email,
			@MobileNumber, @DepartmentIdFk, @Designation, @PersonalEmail, @Gender, @Salary,
			@PermanentAddressLine1, @PermanentAddressLine2, @PermanentCity, @PermanentState,
			@PermanentPincode, @AddressLine1, @AddressLine2, @City, @State, @Pincode, 
			@UserId, GETDATE(), @ActiveStatusId

		SET @EmployeeId = SCOPE_IDENTITY();
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

	SELECT @EmployeeId AS EmployeeId, @EmployeeCode As EmployeeCode
END
