using ToDo.Models.Enum;

namespace ToDo.Models.Models
{
    public class User : BaseEntity
    {
        public string Login { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int TenantId { get; set; }
    }
}
