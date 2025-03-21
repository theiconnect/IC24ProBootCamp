
CREATE TABLE [dbo].[StockProduct](
	[StockProductIdPk] INT Primary key  IDENTITY(1,1) NOT NULL,
	[StockIdFk] INT foreign key references [Stock](StockIdPk) NOT NULL,
	[ProductIdFk] INT foreign key references [ProductMaster](ProductIdPk) NOT NULL,
	[StatusIdFk] INT Foreign key References [StatusMaster](StatusIdPk)   NULL,
	[RecievedQuantity] DECIMAL(10, 2) NOT NULL,
	[PricePerUnit] DECIMAL(10, 2) NOT NULL,
	[AvailableQuantity] DECIMAL(10, 2) NOT NULL,
	[CreatedBy] VARCHAR(512) NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(512) NULL,
	[LastUpdatedOn] DATETIME NULL
) 
GO
ALTER TABLE [dbo].[StockProduct] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
