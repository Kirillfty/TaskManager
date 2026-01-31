using Microsoft.EntityFrameworkCore;
using TaskManager.Entities;

namespace TaskManager
{
    public class ApplicationDB : DbContext
    {
        public ApplicationDB(DbContextOptions<ApplicationDB> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<User> Users { get; set; } = null;

        public DbSet<Tasks> Tasks { get; set; } = null;

      

       

        
    }
}
