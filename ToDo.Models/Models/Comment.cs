
namespace ToDo.Models.Models
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTimeOffset CreationDateOffset { get => CreationDate; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; } = null!;
    }
}
