using Microsoft.EntityFrameworkCore;
using MPPilot.Domain.Models.Accounts;
using MPPilot.Domain.Models.Autobidders;

namespace MPPilot.Domain.Infrastructure
{
    public class MPPilotContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountSettings> AccountSettings { get; set; }
        public DbSet<Autobidder> Autobidders { get; set; }
        public DbSet<AdvertBid> AdvertBids { get; set; }

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


			//Autobidders
			modelBuilder.Entity<Autobidder>()
				.Property(a => a.CreatedDate)
				.HasDefaultValue(DateTimeOffset.Now)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Autobidder>()
				.Property(a => a.UpdatedDate)
				.HasDefaultValue(DateTimeOffset.Now)
				.ValueGeneratedOnAddOrUpdate();

			modelBuilder.Entity<Autobidder>()
				.HasIndex(a => a.AdvertId)
				.IsUnique();


			//AdvertBids
			modelBuilder.Entity<AdvertBid>()
				.Property(a => a.CreatedDate)
				.HasDefaultValue(DateTimeOffset.Now)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<AdvertBid>()
				.Property(a => a.UpdatedDate)
				.HasDefaultValue(DateTimeOffset.Now)
				.ValueGeneratedOnAddOrUpdate();

			base.OnModelCreating(modelBuilder);
        }
    }
}
