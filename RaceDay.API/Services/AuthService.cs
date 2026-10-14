using Microsoft.AspNetCore.Identity;

namespace RaceDay.API.Services
{
    public class AuthService
    {
        private readonly PasswordHasher<object> _passwordHasher;

        public AuthService()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        // Hash a password before saving it to the database
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null!, password);
        }

        // Check whether a password matches its stored hash
        public bool VerifyPassword(string storedHash, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(
                null!,
                storedHash,
                password
            );

            return result != PasswordVerificationResult.Failed;
        }
    }
}