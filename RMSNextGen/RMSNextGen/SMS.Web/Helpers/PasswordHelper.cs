using BCrypt.Net;
namespace SMS.Web.Helpers
{
    public class PasswordHelper
    {
        // Hash the password using BCrypt
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verify the password using BCrypt
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
