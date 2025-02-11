-- GetUserByEmail.sql
CREATE PROCEDURE dbo.GetUserByEmailProcedure
    @Email NVARCHAR(255)
AS
BEGIN
    SELECT UserId, Email, PasswordHash, RoleId
    FROM Users (NOLOCK)
    WHERE Email = @Email;
END
