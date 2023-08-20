namespace MPPilot.App.Models
{
	public class AutobidderEditModel
	{
		public Guid? Id { get; set; }
		public int? AdvertId { get; set; }
		public double? DailyBudget { get; set; }
		public bool? IsEnabled { get; set; }
	}
}
