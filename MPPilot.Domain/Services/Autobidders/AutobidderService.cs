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
		private readonly AccountsService _accountService;

		public AutobidderService(AccountsService accountService, MPPilotContext context, ILogger<AutobidderService> logger)
		{
			_accountService = accountService;
			_context = context;
			_logger = logger;
		}

		public async Task Update(Autobidder autobidder)
		{
			try
			{
				var foundAutobidder = await GetByAdvert(autobidder.AdvertId);
				if (foundAutobidder is not null)
				{
					foundAutobidder.Mode = autobidder.Mode;
					foundAutobidder.DailyBudget = autobidder.DailyBudget;
					foundAutobidder.IsEnabled = autobidder.IsEnabled;
				}
				else
				{
					var currentAccount = await _accountService.GetCurrentAccount();
					autobidder.Account = currentAccount;

					await _context.Autobidders.AddAsync(autobidder);
				}

				await _context.SaveChangesAsync();

				_logger.LogInformation("Настройки автобиддера {Id} успешно обновлены.", autobidder.Id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка при обновлении автобиддера {Id} для РК {AdvertId}", autobidder.Id, autobidder.AdvertId);
				throw;
			}
		}

		public async Task<Autobidder> Create(int advertId)
		{
			try
			{
				var autobidder = new Autobidder
				{
					AdvertId = advertId,
					Mode = AutobidderMode.Conservative,
				};

				await _context.Autobidders.AddAsync(autobidder);
				await _context.SaveChangesAsync();

				_logger.LogInformation("Автобиддер {Id} успешно создан!", autobidder.Id);

				return autobidder;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка при создании автобиддера для РК {AdvertId}", advertId);
				throw;
			}
		}

		public async Task<Autobidder?> GetByAdvert(int advertId)
		{
			var autobidder = await _context.Autobidders.FirstOrDefaultAsync(autobidder => autobidder.AdvertId == advertId);
			if (autobidder is null)
				_logger.LogWarning("Не найден автобиддер для РК {AdvertId}!", advertId);

			return autobidder;
		}

		public async Task AddBid(Autobidder autobidder, AdvertBid bid)
		{
			try
			{
				autobidder.Bids = new List<AdvertBid> { bid };
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Произошла ошибка при добавлении ставки для автобиддера {Id}!", autobidder.Id);
				throw;
			}
		}

		public async Task<List<AdvertBid>> GetBids(Guid autobidderId)
		{
			try
			{
				var bids = await _context.AdvertBids
								.Where(bid => bid.AutobidderId.Equals(autobidderId))
								.OrderByDescending(bid => bid.CreatedDate)
								.ToListAsync();

				return bids;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public async Task StopAsync(Guid id)
		{
			var autobidder = await FindById(id);
			if (autobidder is not null)
			{
				autobidder.IsEnabled = false;
				await _context.SaveChangesAsync();

				_logger.LogInformation("Автобиддер '{Id}' для РК '{AdvertId}' выключен.", id, autobidder.AdvertId);
			}
		}

		public async Task StartAsync(Guid id)
		{
			var autobidder = await FindById(id);
			if (autobidder is not null)
			{
				autobidder.IsEnabled = true;
				await _context.SaveChangesAsync();

				_logger.LogInformation("Автобиддер '{Id}' для РК '{AdvertId}' включен.", id, autobidder.AdvertId);
			}
		}

		public async Task StartBids(Autobidder autobidder)
		{
			autobidder.BidsPausedTill = null;
			_context.Autobidders.Update(autobidder);
			await _context.SaveChangesAsync();
		}

		public async Task PauseBids(Autobidder autobidder, DateTime tillDate)
		{
			autobidder.BidsPausedTill = tillDate;
			_context.Autobidders.Update(autobidder);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Autobidder>> GetActiveAutobidders()
		{
			return await _context.Autobidders
						.Include(autobidder => autobidder.Account)
							.ThenInclude(account => account.Settings)
						.Where(autobidder => autobidder.IsEnabled)
						.Where(autobidder => !string.IsNullOrEmpty(autobidder.Account.Settings.WildberriesApiKey))
						.Where(autobidder => autobidder.BidsPausedTill == null || DateTime.UtcNow > autobidder.BidsPausedTill)
						.ToListAsync();
		}

		private async Task<Autobidder?> FindById(Guid id)
		{
			var foundAutobidder = await _context.Autobidders.FindAsync(id);
			if (foundAutobidder is null)
				_logger.LogWarning("Автобиддер {Id} не найден!", id);

			return foundAutobidder;
		}
	}
}
