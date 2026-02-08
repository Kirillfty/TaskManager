using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int TaskId { get; set; }
        [ForeignKey(nameof(TaskId))] public Task Task {  get; set; }
        public string Role { get; set; } = "user";
        public string? RefreshToken { get; set; }

    }
}
