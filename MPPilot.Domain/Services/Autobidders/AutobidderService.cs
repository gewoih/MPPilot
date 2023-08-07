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

        public async Task AddBid(Autobidder autobidder, AdvertBid bid)
        {
            autobidder.Bids = new List<AdvertBid> { bid };
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Autobidder autobidder)
        {
            autobidder.IsActive = false;

            await _context.Autobidders.AddAsync(autobidder);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Создан новый автобиддер для РК '{autobidder.AdvertId}'");
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
