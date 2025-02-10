using SMS.DAL;
using SMS.Models;
using SMS.Services.Helpers;

namespace SMS.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> AuthenticateUser(string email, string password)
        {
            UserDTO user = await _userRepository.GetUserByEmail(email);

            if (user != null && PasswordHelper.VerifyPassword(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            // 1. Check if the email already exists
            if (await _userRepository.GetUserByEmail(email) != null)
            {
                // Email already exists
                return false;
            }

            // 2. Hash the password
            string hashedPassword = PasswordHelper.HashPassword(password);

            // 3. Create a new User entity
            var newUser = new UserDTO
            {
                Email = email,
                PasswordHash = hashedPassword,
                RoleId = 3 // Default RoleId (e.g., "User" or "Student")
            };

            // 4. Save the user to the database
            return await _userRepository.CreateUser(newUser);
        }
    }
}
