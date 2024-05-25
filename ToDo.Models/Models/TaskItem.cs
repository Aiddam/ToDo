
using ToDo.Models.Enum;

namespace ToDo.Models.Models
{
    public class TaskItem: BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TenantId { get; set; }
        public Status Status { get; set; } = Status.ToDo;
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
