using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Entities
{
    
    [Table("Tasks")]
    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsCompleted { get; set; } = false;
        public int UserId {  get; set; }

        [ForeignKey(nameof(UserId))] public User user { get; set; }

    }
}
