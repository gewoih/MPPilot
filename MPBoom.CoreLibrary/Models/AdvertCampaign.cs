﻿using MPBoom.Core.Enums;

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
		public bool IsEnabled { get; set; }
		public bool IsAvailableToEnable { get; set; }
		public int TotalViews { get; set; }
		public int Clicks { get; set; }
		public int UniqueViews { get; set; }
		public double TotalSpent { get; set; }
		public int AddedToCart { get; set; }
		public int Orders { get; set; }
		public double OrdersSum { get; set; }
		public double CTR => TotalViews > 0 ? Clicks / TotalViews * 100 : 0;
		public double CPC => Clicks > 0 ? TotalSpent / Clicks : 0;
		public double Frequency => UniqueViews > 0 ? TotalViews / UniqueViews : 0;
		public double OrderCost => Orders > 0 ? TotalSpent / Orders : 0;
		public double ConversionRate => Clicks > 0 ? Orders / Clicks * 100 : 0;
		public AdvertCampaignType Type { get; set; }
		public AdvertCampaignStatus Status { get; set; }
		public DateTimeOffset CreatedDate { get; set; }
		public DateTimeOffset LastUpdateDate { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }
	}
}
