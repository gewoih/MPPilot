using Microsoft.EntityFrameworkCore;
using MPBoom.Domain.Models.Account;

namespace MPBoom.API.Infrastructure.Contexts
{
    public class MPBoomContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        
        public MPBoomContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(a => a.CreatedDate)
                .HasDefaultValue(DateTimeOffset.Now)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Account>()
                .Property(a => a.UpdatedDate)
                .HasDefaultValue(DateTimeOffset.Now)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();
        }
    }
}
