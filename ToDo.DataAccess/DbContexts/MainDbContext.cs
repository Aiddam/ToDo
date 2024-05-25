using ToDo.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ToDo.DataAccess.DbContexts
{
    public class MainDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MainDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SQLProviderConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
            modelBuilder.Entity<TaskItem>()
           .HasMany(a => a.Comments)
           .WithOne(b => b.TaskItem)
           .HasForeignKey(b => b.TaskItemId);

            modelBuilder.Entity<Comment>()
            .Property(e => e.CreationDate)
            .HasDefaultValueSql("GETDATE()");
        }
    }
}
