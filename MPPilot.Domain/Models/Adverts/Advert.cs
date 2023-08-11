using MPPilot.Domain.Models.Autobidders;
using MPPilot.Domain.Utils;

namespace MPPilot.Domain.Models.Adverts
{
    public class Advert
    {
        public Autobidder Autobidder { get; set; }
        public bool IsAutobidderEnabled => Autobidder is not null && Autobidder.IsEnabled;

        public int AdvertId { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        
        public AdvertType Type { get; set; }
        public string TypeString => Type.GetDescription();
       
        public AdvertStatus Status { get; set; }
        public string StatusString => Status.GetDescription();

        public bool IsEnabled => Status == AdvertStatus.InProgress;
        public bool IsArchived => Status == AdvertStatus.Stopped;

        public string ProductArticle { get; set; }
        public int CategoryId { get; set; }
        public int BudgetSize { get; set; }
        public int CPM { get; set; }
        public int TotalViews { get; set; }
        public int Clicks { get; set; }
        public int UniqueViews { get; set; }
        public double TotalSpent { get; set; }
        public int AddedToCart { get; set; }
        public int Orders { get; set; }
        public double OrdersSum { get; set; }

        public double CTR => TotalViews > 0 ? (double)Clicks / TotalViews * 100 : 0;
        public double CPC => Clicks > 0 ? TotalSpent / Clicks : 0;
        public double Frequency => UniqueViews > 0 ? (double)TotalViews / UniqueViews : 0;
        public double OrderCost => Orders > 0 ? TotalSpent / Orders : 0;
        public double ConversionRate => Clicks > 0 ? (double)Orders / Clicks * 100 : 0;

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset LastUpdateDate { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
