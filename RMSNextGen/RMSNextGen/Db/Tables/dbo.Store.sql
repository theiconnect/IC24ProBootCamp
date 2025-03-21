
CREATE TABLE [dbo].[Store](
	[StoreIdpk] INT Primary key IDENTITY(1,1) NOT NULL,
	[StoreCode] VARCHAR(50)  NOT NULL,
	[StatusIdFk] INT Foreign key References [StatusMaster](StatusIdPk)   NULL,
	[StoreName] VARCHAR(512) NOT NULL,
	[StoreLocation] VARCHAR(100) NOT NULL,
	[ContactNumber] VARCHAR(20)  NOT NULL,
	[City] VARCHAR(100) NULL,
	[State] VARCHAR(100) NULL,
	[FAX] VARCHAR(15) NULL,
	[GSTNo] VARCHAR(21) NULL,
	[CINNo] VARCHAR(21) NULL,
	[ManagerName] VARCHAR(100) NOT NULL,
	[ManagerNo] VARCHAR(20) NOT NULL,
	[IsCorporateOffice] BIT NOT NULL,
	[CreatedBy] VARCHAR(512) NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(512) NULL,
	[LastUpdatedOn] DATETIME NULL, 
    [NickName] VARCHAR(50) NOT NULL, 
    [Address] VARCHAR(100) NOT NULL, 
    [OfficeNo] VARCHAR(50) NOT NULL
) 
GO
ALTER TABLE [dbo].[Store] ADD  DEFAULT ((0)) FOR [IsCorporateOffice]
GO
ALTER TABLE [dbo].[Store] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
