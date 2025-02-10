CREATE TABLE [dbo].[Users] (
    [UserId]       INT           IDENTITY (1, 1) NOT NULL,
    [Email]        VARCHAR (512) NOT NULL,
    [PasswordHash] VARCHAR (255) NOT NULL,
    [RoleId]       INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

