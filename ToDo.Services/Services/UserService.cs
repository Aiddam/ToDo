using AutoMapper;
using ToDo.Utilities.Exceptions;
using ToDo.Interfaces.DataAccessInterfaces;
using ToDo.Interfaces.Services;
using ToDo.Interfaces.Utilities;
using ToDo.Models.Models;
namespace ToDo.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<User> LoginAsync(string login, string password, CancellationToken cancellationToken = default)
        {
            var userRepository = await _unitOfWork.GetRepository<User>();

            var user = await userRepository.ReadEntitiesByPredicate(u => u.Login == login, cancellationToken: cancellationToken);

            if (user == null || !user.Any())
            {
                throw new InvalidLoginException(login);
            }

            var foundUser = user.First();
            var pass = await _passwordHasher.CreatePasswordHashAsync(password);

            var passwordValidationResult = await _passwordHasher.VerifyPasswordHashAsync(password, foundUser.PasswordHash);

            if (!passwordValidationResult)
            {
                throw new InvalidPasswordException();
            }

            return foundUser;
        }

        public async Task<User> RegistrationAsync(UserDto userDto, CancellationToken cancellationToken = default)
        {
            var userRepository = await _unitOfWork.GetRepository<User>();

            var foundUsers = await userRepository.ReadEntitiesByPredicate(u => u.Login == userDto.Login, cancellationToken: cancellationToken);

            var foundUser = foundUsers.FirstOrDefault();

            if (foundUser is not null)
            {
                throw new InvalidRegistrationException(userDto.Login);
            }

            User user = _mapper.Map<User>(userDto);
            user.PasswordHash = await _passwordHasher.CreatePasswordHashAsync(userDto.Password);
            user.Salt = _passwordHasher.GetPasswordSaltSeperator(); ;

            var newUser = await userRepository.CreateAsync(user);

            await _unitOfWork.CommitAsync();

            return newUser;
        }
    }
}
