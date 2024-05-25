namespace ToDo.Utilities.Exceptions
{
    public class InvalidLoginException : Exception
    {
        private readonly string _login;

        public InvalidLoginException(string login)
        {
            _login = login;
        }

        public override string ToString() => $"Unable to get user with {_login} login";
    }
}
