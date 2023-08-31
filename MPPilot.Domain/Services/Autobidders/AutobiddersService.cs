﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MPPilot.Domain.Infrastructure;
using MPPilot.Domain.Models.Adverts;
using MPPilot.Domain.Models.Autobidders;
using MPPilot.Domain.Services.Users;

namespace MPPilot.Domain.Services.Autobidders
{
	public class AutobiddersService : IAutobiddersService
	{
		private readonly ILogger<AutobiddersService> _logger;
		private readonly MPPilotContext _context;
		private readonly IUsersService _accountService;

		public AutobiddersService(IUsersService accountService, MPPilotContext context, ILogger<AutobiddersService> logger)
		{
			_accountService = accountService;
			_context = context;
			_logger = logger;
		}

		public async Task CreateAsync(Autobidder autobidder)
		{
			try
			{
				var currentAccount = await _accountService.GetCurrentUserAsync();
				currentAccount.Autobidders ??= new List<Autobidder>();

				currentAccount.Autobidders.Add(autobidder);
				await _context.SaveChangesAsync();

				_logger.LogInformation("Автобиддер {Id} успешно создан!", autobidder.Id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка при создании автобиддера для РК {AdvertId}", autobidder.AdvertId);
				throw;
			}
		}

		public async Task UpdateAsync(Autobidder autobidder)
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

		public async Task LoadAutobiddersForAdvertsAsync(List<Advert> adverts)
		{
			//var advertIds = adverts.Select(advert => advert.AdvertId);
			var autobidders = await _context.Autobidders
												.AsNoTracking()
												.ToListAsync();

			foreach (var advert in adverts)
			{
				advert.Autobidder = autobidders.FirstOrDefault(autobidder => autobidder.AdvertId == advert.AdvertId);
			}
		}

		public async Task AddBidAsync(Autobidder autobidder, AdvertBid bid)
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

		public async Task<List<AdvertBid>> GetBidsAsync(Guid autobidderId)
		{
			var bids = await _context.AdvertBids
				.Where(bid => bid.AutobidderId.Equals(autobidderId))
				.OrderByDescending(bid => bid.CreatedDate)
				.ToListAsync();

			return bids;
		}

		public async Task StartBidsAsync(Autobidder autobidder)
		{
			autobidder.BidsPausedTill = null;
			_context.Autobidders.Update(autobidder);
			await _context.SaveChangesAsync();
		}

		public async Task PauseBidsAsync(Autobidder autobidder, DateTimeOffset tillDate)
		{
			autobidder.BidsPausedTill = tillDate;
			_context.Autobidders.Update(autobidder);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Autobidder>> GetActiveAutobiddersAsync()
		{
			return await _context.Autobidders
						.Include(autobidder => autobidder.User)
							.ThenInclude(account => account.Settings)
						.Where(autobidder => autobidder.IsEnabled)
						.Where(autobidder => !string.IsNullOrEmpty(autobidder.User.Settings.WildberriesApiKey))
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
