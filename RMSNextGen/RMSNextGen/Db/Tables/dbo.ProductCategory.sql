
CREATE TABLE [dbo].[ProductCategory](
	[ProductCategoryIdPk] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[ProductCategoryCode] VARCHAR(100)  NOT NULL,
	[ProductCategoryName] VARCHAR(512) NOT NULL,
	[Description] VARCHAR(512) NULL,
	[CreatedBy] VARCHAR(512) NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(512) NULL,
	[LastUpdatedOn] DATETIME NULL
) 
GO
ALTER TABLE [dbo].[ProductCategory] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
