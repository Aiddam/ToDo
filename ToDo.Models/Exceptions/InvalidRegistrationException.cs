namespace ToDo.Utilities.Exceptions
{
    public class InvalidRegistrationException : Exception
    {
        private readonly string _login;

        public InvalidRegistrationException(string login)
        {
            _login = login;
        }

        public override string ToString() => $"User with login {_login} already exists";
    }
}
