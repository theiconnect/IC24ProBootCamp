
CREATE TABLE [dbo].[Stock](
	[StockIdPk] INT Primary Key IDENTITY(1,1) NOT NULL,
	[StockCode] VARCHAR(50)   NOT NULL,
	[PurchaseOrderNumber] INT  NOT NULL,
	[InvoiceNumber] VARCHAR(32)  NOT NULL,
	[StoreIdFk] INT Foreign key References Store(StoreIdPk)  NULL,
	[SuplierIdFk] INT Foreign key References Supplier(SupplierIdPk)  NULL,
	[StatusIdFk] INT Foreign key References [StatusMaster](StatusIdPk)   NULL,
	[StockInTime] DATETIME NOT NULL,
	[Remarks] VARCHAR(max) NULL,
	[VehicleNumber] VARCHAR(100) NULL,
	[ApprovedBy] VARCHAR(512) NULL,
	[ApprovedOn] DATETIME NOT NULL,
	[ApprovedComments] VARCHAR(max) NULL,
	[CreatedBy] VARCHAR(512) NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(512) NULL,
	[LastUpdatedOn] DATETIME NULL
) 
GO
ALTER TABLE [dbo].[Stock] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
