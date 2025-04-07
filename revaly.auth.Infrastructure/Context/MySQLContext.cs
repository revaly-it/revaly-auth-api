using Microsoft.EntityFrameworkCore;
using revaly.auth.Domain.Entities;
using revaly.auth.Domain.Entities.Enums;

namespace revaly.auth.Infrastructure.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options)
            : base(options){}

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the primary key for the User entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            // Parsing Enum to string
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (Role)Enum.Parse(typeof(Role), v));

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MySQLContext).Assembly);
        }
    }
}
