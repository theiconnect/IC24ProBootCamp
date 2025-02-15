CREATE TABLE [dbo].[LokeshStudent]
(
	[Studentcode] VARCHAR(100) NOT NULL, 
    [StudentName] VARCHAR(100) NOT NULL, 
    [Gender] VARCHAR(20) NOT NULL, 
    [Grade] VARCHAR(20) NOT NULL, 
    [DOB] DATE NOT NULL, 
    [IsTransport] VARCHAR(50) NOT NULL, 
    [Comments] VARCHAR(MAX) NOT NULL
)
