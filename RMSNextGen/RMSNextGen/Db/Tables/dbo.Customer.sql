
CREATE TABLE [dbo].[Customer](
	[CustomerIdPk] INT PRIMARY KEY   IDENTITY(1,1) NOT NULL,
	[StatusIdFk] INT Foreign key References [StatusMaster](StatusIdPk)   NULL,
	[CustomerCode] VARCHAR(32)  NOT NULL,
	[CustomerName] VARCHAR(512) NOT NULL,
	[ContactNumber] VARCHAR(15)  NOT NULL,
	[Email] VARCHAR(256)  NOT NULL,
	[CreatedBy] VARCHAR(512) NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(512) NULL,
	[LastUpdatedOn] DATETIME NULL
) 
GO
ALTER TABLE [dbo].[Customer] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
