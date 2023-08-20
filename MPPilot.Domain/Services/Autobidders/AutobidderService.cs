using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MPPilot.Domain.Infrastructure;
using MPPilot.Domain.Models.Adverts;
using MPPilot.Domain.Models.Autobidders;
using MPPilot.Domain.Services.Accounts;

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
				var foundAutobidder = await FindById(autobidder.Id);
				foundAutobidder.Mode = autobidder.Mode;
				foundAutobidder.DailyBudget = autobidder.DailyBudget;
				foundAutobidder.IsEnabled = autobidder.IsEnabled;

				_context.Autobidders.Update(foundAutobidder);
				await _context.SaveChangesAsync();

				_logger.LogInformation("Настройки автобиддера {Id} успешно обновлены.", autobidder.Id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка при обновлении автобиддера {Id} для РК {AdvertId}", autobidder.Id, autobidder.AdvertId);
				throw;
			}
		}

		public async Task<Autobidder> Create(Autobidder autobidder)
		{
			try
			{
				var currentAccount = await _accountService.GetCurrentAccount();
				autobidder.Account = currentAccount;
				await _context.Autobidders.AddAsync(autobidder);
				await _context.SaveChangesAsync();

				_logger.LogInformation("Автобиддер {Id} успешно создан!", autobidder.Id);

				return autobidder;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка при создании автобиддера для РК {AdvertId}", autobidder.AdvertId);
				throw;
			}
		}

		public async Task<List<Advert>> LoadAutobidders(List<Advert> adverts)
		{
			var advertIds = adverts.Select(advert => advert.AdvertId);
			var autobidders = await _context.Autobidders
									.Where(autobidder => advertIds.Contains(autobidder.AdvertId))
									.ToListAsync();

			foreach (var advert in adverts)
			{
				advert.Autobidder = autobidders.FirstOrDefault(autobidder => autobidder.AdvertId == advert.AdvertId);
			}

			return adverts;
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
