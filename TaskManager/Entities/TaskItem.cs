using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Entities
{
    
    [Table("Tasks")]
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; }
        
        public string Status { get; set; } = "Pending";
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))] public User user { get; set; }

    }
}
