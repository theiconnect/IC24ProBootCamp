
CREATE TABLE [dbo].[Store](
	[StoreIdpk] INT Primary key IDENTITY(1,1) NOT NULL,
	[StoreCode] VARCHAR(50)  NOT NULL,
	[StatusIdFk] INT Foreign key References [StatusMaster](StatusIdPk)   NULL,
	[StoreName] VARCHAR(512) NOT NULL,
	[Location] VARCHAR(100) NOT NULL,
	[ContactNumber] VARCHAR(20)  NOT NULL,
	[City] VARCHAR(100) NULL,
	[State] VARCHAR(100) NULL,
	[Fax] VARCHAR(15) NULL,
	[GST] VARCHAR(21) NULL,
	[CIN] VARCHAR(21) NULL,
	[ManagerName] VARCHAR(100) NOT NULL,
	[ManagerContactNumber] VARCHAR(20) NOT NULL,
	[IsCorporateOffice] BIT NOT NULL,
	[CreatedBy] VARCHAR(512) NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(512) NULL,
	[LastUpdatedOn] DATETIME NULL
) 
GO
ALTER TABLE [dbo].[Store] ADD  DEFAULT ((0)) FOR [IsCorporateOffice]
GO
ALTER TABLE [dbo].[Store] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
