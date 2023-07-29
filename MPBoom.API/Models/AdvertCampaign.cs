using MPBoom.Services.PricesLoader.Models.Enums;

namespace MPBoom.Services.PricesLoader.Models
{
    public class AdvertCampaign
    {
        public Guid Id { get; set; }
        public string AdvertId { get; set; }
        public string Name { get; set; }
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
