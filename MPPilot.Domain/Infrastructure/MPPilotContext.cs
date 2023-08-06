using Microsoft.EntityFrameworkCore;
using MPPilot.Domain.Models.Account;

namespace MPPilot.Domain.Infrastructure
{
    public class MPPilotContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountSettings> AccountSettings { get; set; }

        public MPPilotContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Account
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


            //AccountSettings
            modelBuilder.Entity<AccountSettings>()
                .Property(a => a.CreatedDate)
                .HasDefaultValue(DateTimeOffset.Now)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<AccountSettings>()
                .Property(a => a.UpdatedDate)
                .HasDefaultValue(DateTimeOffset.Now)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<AccountSettings>()
                .HasIndex(a => a.WildberriesApiKey)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
