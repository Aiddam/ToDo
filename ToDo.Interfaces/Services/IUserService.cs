using ToDo.Models.Models;

namespace ToDo.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> LoginAsync(string login, string password, CancellationToken cancellationToken = default);
        Task<User> RegistrationAsync(UserDto userDto, CancellationToken cancellationToken = default);
    }
}
