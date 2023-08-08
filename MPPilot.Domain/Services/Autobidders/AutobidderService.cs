using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MPPilot.Domain.Infrastructure;
using MPPilot.Domain.Models.Autobidders;

namespace MPPilot.Domain.Services.Autobidders
{
    public class AutobidderService
    {
        private readonly ILogger<AutobidderService> _logger;
        private readonly MPPilotContext _context;

        public AutobidderService(MPPilotContext context, ILogger<AutobidderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Autobidder> Create(int advertId)
        {
            var autobidder = new Autobidder
            {
                AdvertId = advertId,
                Mode = AutobidderMode.Conservative,
            };

            await _context.Autobidders.AddAsync(autobidder);
            await _context.SaveChangesAsync();
            return autobidder;
        }

        public async Task<Autobidder> GetByAdvert(int advertId)
        {
            var autobidder = await _context.Autobidders.FirstOrDefaultAsync(autobidder => autobidder.AdvertId == advertId);
			return autobidder is null ? throw new ArgumentException($"Для РК '{advertId}' не найден автобиддер") : autobidder;
		}

		public async Task AddBid(Autobidder autobidder, AdvertBid bid)
        {
            autobidder.Bids = new List<AdvertBid> { bid };
            await _context.SaveChangesAsync();
        }

        public async Task StopAsync(Guid id)
        {
            var autobidder = await FindById(id);
            if (autobidder is not null)
            {
                autobidder.IsActive = false;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Автобиддер '{id}' для РК '{autobidder.AdvertId}' выключен");
            }
        }

        public async Task StartAsync(Guid id)
        {
            var autobidder = await FindById(id);
            if (autobidder is not null)
            {
                autobidder.IsActive = true;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Автобиддер '{id}' для РК '{autobidder.AdvertId}' включен");
            }
        }

        public async Task<List<Autobidder>> GetActiveAutobidders()
        {
            return await _context.Autobidders
                        .Include(autobidder => autobidder.Account)
                            .ThenInclude(account => account.Settings)
                        .Where(autobidder => autobidder.IsActive)
                        .Where(autobidder => !string.IsNullOrEmpty(autobidder.Account.Settings.WildberriesApiKey))
                        .ToListAsync();
        }

        private async Task<Autobidder?> FindById(Guid id)
        {
            return await _context.Autobidders.FindAsync(id);
        }
    }
}
