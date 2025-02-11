-- CreateUser.sql
CREATE PROCEDURE dbo.CreateUserProcedure
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(MAX),
    @RoleId INT,
    @OutResponse INT = 0 OUTPUT 
AS
BEGIN

    IF EXISTS (SELECT * FROM Users WHERE Email = @Email)
    BEGIN
        PRINT 'User already exists'
        SET @OutResponse = 0
        RETURN;
    END

    INSERT INTO Users (Email, PasswordHash, RoleId)
    VALUES (@Email, @PasswordHash, @RoleId);

    -- Return the new UserId (optional)
    SELECT @OutResponse = SCOPE_IDENTITY();
END
