using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MPPilot.Domain.Models.Autobidders;
using MPPilot.Domain.Models.Base;
using MPPilot.Domain.Models.Users;

namespace MPPilot.Domain.Infrastructure
{
	public class MPPilotContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<UserSettings> AccountSettings { get; set; }
        public DbSet<Autobidder> Autobidders { get; set; }
        public DbSet<AdvertBid> AdvertBids { get; set; }
		public DbSet<LoginHistory> LoginsHistory { get; set; }

        public MPPilotContext(DbContextOptions<MPPilotContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Account
            modelBuilder.Entity<User>()
                .HasIndex(a => a.Email)
                .IsUnique();

            //AccountSettings
            modelBuilder.Entity<UserSettings>()
                .HasIndex(a => a.WildberriesApiKey)
                .IsUnique();

			//Autobidders
			modelBuilder.Entity<Autobidder>()
				.HasIndex(a => a.AdvertId)
				.IsUnique();

			base.OnModelCreating(modelBuilder);
		}

		private void HandleEntitiesUpdates()
		{
			var entries = ChangeTracker
							.Entries()
							.Where(e => e.Entity is Entity && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

			foreach (var entityEntry in entries)
			{
				var entity = (Entity)entityEntry.Entity;

				if (entityEntry.State == EntityState.Added)
					entity.CreatedDate = DateTimeOffset.UtcNow;

				if (entityEntry.State == EntityState.Deleted)
				{
					entityEntry.State = EntityState.Modified;
					entity.DeletedDate = DateTimeOffset.UtcNow;
				}

				entity.UpdatedDate = DateTimeOffset.UtcNow;
			}
		}

		public override int SaveChanges()
		{
			HandleEntitiesUpdates();
			return base.SaveChanges();
		}
		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			HandleEntitiesUpdates();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			HandleEntitiesUpdates();
			return base.SaveChangesAsync(cancellationToken);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			HandleEntitiesUpdates();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
	}
}
