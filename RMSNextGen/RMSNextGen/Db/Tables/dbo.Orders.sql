
CREATE TABLE [dbo].[Orders](
	[OrderIdpk] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] VARCHAR(50)  NOT NULL,
	[OrderName] VARCHAR(50) NOT NULL,
	[StoreIdFk] INT Foreign key References Store(StoreIdPk)  NULL,
	[CustomerIdFk] INT Foreign key References  [Customer](CustomerIdPk)  NULL,
	[StatusIdFk] INT Foreign key References [StatusMaster](StatusIdPk)   NULL,
	[OrderDate] DATE NOT NULL,
	[NoOfItems] DECIMAL(18, 0) NULL,
	[TotalAmount] DECIMAL(10, 2) NULL,
	[CreatedBy] VARCHAR(512) NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(512) NULL,
	[LastUpdatedOn] DATETIME NULL
) 
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
