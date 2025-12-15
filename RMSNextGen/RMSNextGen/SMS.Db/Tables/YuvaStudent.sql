CREATE TABLE [dbo].[YuvaStudent]
(
    [StudentCode] VARCHAR(50) NOT NULL, 
    [StudentName] VARCHAR(100) NOT NULL, 
    [DOB] DATE NOT NULL, 
    [Grade] VARCHAR(50) NOT NULL, 
    [Gender] VARCHAR(50) NOT NULL, 
    [IsOwnTransport] BIT NOT NULL, 
    [Comments] VARCHAR(MAX) NOT NULL
)
