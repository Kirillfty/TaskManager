using Microsoft.EntityFrameworkCore;
using TaskManager.Entities;

namespace TaskManager
{
    public class ApplicationDB : DbContext
    {
        public ApplicationDB(DbContextOptions<ApplicationDB> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<TaskItem>().HasKey(t => t.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}