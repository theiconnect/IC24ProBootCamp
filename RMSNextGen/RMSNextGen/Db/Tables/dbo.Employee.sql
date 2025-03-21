
CREATE TABLE [dbo].[Employee](
	[EmployeeIdPk] INT PRIMARY KEY  IDENTITY(1,1) NOT NULL,
	[EmployeeCode] VARCHAR(10)  NOT NULL,
	[EmployeeFirstName] VARCHAR(512) NOT NULL,
	[EmployeeLastName] VARCHAR(512) NULL,
	[Email] VARCHAR(250)  NOT NULL,
	[MobileNumber] VARCHAR(10)  NOT NULL,
	[StoreIdFk] INT Foreign key References Store(StoreIdPk)  NULL,
	[StatusIdFk] INT Foreign key References [StatusMaster](StatusIdPk) NULL,
	[DepartmentIdFk] INT Foreign key References [DepartmentMaster](DepartmentIdPk) NULL,
	[Designation] VARCHAR(100) NULL,
	[PersonalEmail] VARCHAR(250)  NULL,
	[Gender] VARCHAR(10) NOT NULL,
	[SalaryCTC] DECIMAL(10, 2) NULL,
	[PermanentAddressLine1] VARCHAR(256) NULL,
	[PermanentAddressLine2] VARCHAR(256) NULL,
	[PermanentCity] VARCHAR(256) NULL,
	[PermanentState] VARCHAR(50) NULL,
	[PermanentPinCode] VARCHAR(6) NULL,
	[CurrentAddressLine1] VARCHAR(250) NULL,
	[CurrentAddressLine2] VARCHAR(250) NULL,
	[CurrentCity] VARCHAR(250) NULL,
	[CurrentState] VARCHAR(250) NULL,
	[CurrentPinCode] VARCHAR(6) NULL,
	[CreatedBy] VARCHAR(512) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastUpdatedBy] VARCHAR(512) NULL,
	[LastUpdatedOn] DATETIME NULL
) 
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
