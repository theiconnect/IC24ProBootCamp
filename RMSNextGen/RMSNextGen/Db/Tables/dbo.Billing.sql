
CREATE TABLE [dbo].[Billing](
	[BillingIdPk] INT Primary key IDENTITY(1,1) NOT NULL,
	[OrderIdFk] INT foreign key references Orders(OrderIdPk) NOT NULL,
	[BillingCode] varchar(50)  NOT NULL,
	[StoreIdFk] INT Foreign key References Store(StoreIdPk)  NULL,
	[ProductIdFk] INT Foreign key References ProductMaster(ProductIdPk)   NULL,
	[StatusIdFk] INT Foreign key References [StatusMaster](StatusIdPk)   NULL,
	[Quantity] DECIMAL(18, 0) NOT NULL,
	[PricePerUnit] DECIMAL(18, 0) NOT NULL,
	[InvoiceNumber] VARCHAR(32)  NOT NULL,
	[TotalAmount] DECIMAL(18, 0) NOT NULL
) 
