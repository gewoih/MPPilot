using MPBoom.Core.Enums;

namespace MPBoom.Core.Models
{
	public class AdvertCampaign
	{
		public string AdvertId { get; set; }
		public string Name { get; set; }
		public string Keyword { get; set; }
		public List<Product> Products { get; set; }
		public int BudgetSize { get; set; }
		public int CPM { get; set; }
		public AdvertCampaignType Type { get; set; }
		public AdvertCampaignStatus Status { get; set; }
		public DateTimeOffset CreatedDate { get; set; }
		public DateTimeOffset LastUpdateDate { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }
	}
}
