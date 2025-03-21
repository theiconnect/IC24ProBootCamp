
CREATE TABLE [dbo].[Supplier](
	[SupplierIdPk] INT Primary Key IDENTITY(1,1) NOT NULL,
	[SupplierCode] VARCHAR(23)  NOT NULL,
	[StatusIdFk] INT Foreign key References [StatusMaster](StatusIdPk)   NULL,
	[SupplierName] VARCHAR(256) NOT NULL,
	[CompanyName] VARCHAR(128) NOT NULL,
	[ContactNumber1] VARCHAR(15)  NULL,
	[ContactNumber2] VARCHAR(15)  NOT NULL,
	[Email] VARCHAR(512)  NOT NULL,
	[GSTNumber] VARCHAR(15)  NULL,
	[Address] VARCHAR(256) NULL,
	[CreatedBy] VARCHAR(512) NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(512) NULL,
	[LastUpdatedOn] DATETIME NULL
) 
GO
ALTER TABLE [dbo].[Supplier] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
