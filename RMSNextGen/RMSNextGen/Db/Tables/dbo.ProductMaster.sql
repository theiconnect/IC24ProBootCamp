
CREATE TABLE [dbo].[ProductMaster](
	[ProductIdPk] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[ProductCode] VARCHAR(20)   NOT NULL,
	[ProductName] VARCHAR(100) NOT NULL,
	[PricePerUnit] DECIMAL(18, 0) NOT NULL,
	[UOMIdFk] [int] FOREIGN KEY REFERENCES [UOM](UOMIdPk)  NULL,
	[ThresholdLimit] DECIMAL(18, 0) NOT NULL,
	[ProductCategoryIdFk] [int] FOREIGN KEY REFERENCES [ProductCategory](ProductCategoryIdPk)  NULL,
	[CreatedBy] VARCHAR(512) NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(512) NULL,
	[LastUpdatedOn] DATETIME NULL
) 
GO
ALTER TABLE [dbo].[ProductMaster] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
