
CREATE TABLE [dbo].[StatusMaster](
	[StatusIdPk] INT Primary key NOT NULL,
	[StatusCode] VARCHAR(50)  NOT NULL,
	[StatusName] VARCHAR(256) NOT NULL,
	[Description] VARCHAR(max) NULL
)
