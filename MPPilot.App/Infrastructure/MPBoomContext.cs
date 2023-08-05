using Microsoft.EntityFrameworkCore;
using MPBoom.Domain.Models.Account;

namespace MPPilot.App.Infrastructure
{
    public class MPBoomContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountSettings> AccountSettings { get; set; }

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

            modelBuilder.Entity<Account>()
                .HasOne(a => a.AccountSettings)
                .WithOne(s => s.Account)
                .HasForeignKey<AccountSettings>(s => s.AccountId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
