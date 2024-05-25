using System.ComponentModel.DataAnnotations;

namespace ToDo.Models.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
