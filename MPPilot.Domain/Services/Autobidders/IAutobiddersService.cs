using MPPilot.Domain.Models.Adverts;
using MPPilot.Domain.Models.Autobidders;

namespace MPPilot.Domain.Services.Autobidders
{
	public interface IAutobiddersService
	{
		public Task CreateAsync(Autobidder autobidder);
		public Task UpdateAsync(Autobidder autobidder);
		public Task LoadAutobiddersForAdvertsAsync(List<Advert> adverts);
		public Task<List<Autobidder>> GetActiveAutobiddersAsync();
		public Task<List<AdvertBid>> GetBidsAsync(Guid autobidderId);
		public Task AddBidAsync(Autobidder autobidder, AdvertBid bid);
		public Task StartBidsAsync(Autobidder autobidder);
		public Task PauseBidsAsync(Autobidder autobidder, DateTimeOffset tillDate);
	}
}
