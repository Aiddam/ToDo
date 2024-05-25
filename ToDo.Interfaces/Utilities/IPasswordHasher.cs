namespace ToDo.Interfaces.Utilities
{
    public interface IPasswordHasher
    {
        Task<string> CreatePasswordHashAsync(string password, CancellationToken cancellationToken = default);
        public Task<bool> VerifyPasswordHashAsync(string password, string passwordHash, CancellationToken cancellationToken = default);
        public string GetPasswordSaltSeperator();
    }
}
