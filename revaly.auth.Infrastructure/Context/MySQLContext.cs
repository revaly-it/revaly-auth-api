using Microsoft.EntityFrameworkCore;
using revaly.auth.Domain.Entities;
using revaly.auth.Domain.Entities.Enums;

namespace revaly.auth.Infrastructure.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options)
            : base(options){}

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the primary key for the RefreshToken entity
            modelBuilder.Entity<RefreshToken>()
                .HasKey(rt => rt.Id);

            // Configuring the primary key for the User entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            // Relationship between User and RefreshToken (one-to-many, means one user can have many refresh tokens)
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

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
