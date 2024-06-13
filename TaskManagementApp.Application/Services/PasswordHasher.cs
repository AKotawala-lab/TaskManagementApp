using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TaskManagementApp.Application.Interfaces;

namespace TaskManagementApp.Application.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32);

            var hashBytes = new byte[48];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hashed, 0, hashBytes, 16, 32);
            
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);
            var hash = new byte[32];
            Buffer.BlockCopy(hashBytes, 16, hash, 0, 32);

            var verifyHash = KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32);

            return hash.SequenceEqual(verifyHash);
        }
    }
}
