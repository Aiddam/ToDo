using ToDo.Utilities.Configurations;
using ToDo.Interfaces.Utilities;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace ToDo.Utilities
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly string _passwordSaltSeparator;

        public PasswordHasher(IConfiguration configuration)
        {
            var passwordHasherConfiguration = configuration.GetSection("PasswordHasherConfiguration").Get<PasswordHasherConfiguration>()
                ?? throw new ArgumentNullException("Unable to get password hasher configuration");
            _passwordSaltSeparator = passwordHasherConfiguration.PasswordSaltSeparator;
        }

        //public PasswordHasher(PasswordHasherConfiguration passwordHasherConfiguration)
        //{
        //    _passwordSaltSeparator = passwordHasherConfiguration.PasswordSaltSeparator;
        //}

        public Task<string> CreatePasswordHashAsync(string password, CancellationToken cancellationToken = default)
        {
            using var hmac = new HMACSHA512();

            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var pass = Convert.ToBase64String(passwordHash);

            return Task.FromResult($"{Convert.ToBase64String(passwordHash)}{_passwordSaltSeparator}{Convert.ToBase64String(passwordSalt)}");
        }

        public string GetPasswordSaltSeperator()
        {
            return _passwordSaltSeparator;
        }

        public Task<bool> VerifyPasswordHashAsync(string password, string passwordHash, CancellationToken cancellationToken = default)
        {
            var passwordHashParts = passwordHash.Split(_passwordSaltSeparator);
            (var storedPasswordHash, var salt) = (Convert.FromBase64String(passwordHashParts[0]), Convert.FromBase64String(passwordHashParts[1]));

            using var hmac = new HMACSHA512(salt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Task.FromResult(computedHash.SequenceEqual(storedPasswordHash));
        }
    }
}
