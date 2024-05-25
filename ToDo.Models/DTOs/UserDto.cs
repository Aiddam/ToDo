using ToDo.Models.Enum;

namespace ToDo.Models.Models
{
    public class UserDto
    {
        public string Login { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int TenantId {  get; set; }
    }
}
