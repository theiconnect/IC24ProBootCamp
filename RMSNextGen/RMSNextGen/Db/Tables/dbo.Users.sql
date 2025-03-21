
CREATE TABLE [dbo].[Users](
	[UserIdPK] INT Primary key  IDENTITY(1,1) NOT NULL,
	[Password] VARCHAR(250)  NOT NULL,
	[EmployeeIdFK] INT foreign key references  [Employee](EmployeeIdPk)  NULL,
	[StatusIdFk] INT Foreign key References [StatusMaster](StatusIdPk)   NULL,
	[RoleIdFk] INT foreign key references  [Role](RoleIdPk)  NULL,
	[OTP] VARCHAR(20) NULL,
	[OTPSendDateTime] DATETIME NULL,
	[CreatedBy] VARCHAR(512) NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(512) NULL,
	[LastUpdatedOn] DATETIME NULL
) 
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
