namespace MPPilot.App.Models
{
	public class AdvertEditModel
	{
		public int AdvertId { get; set; }
		public bool? IsEnabled { get; set; }
		public string? NewName { get; set; }
		public string? NewKeyword { get; set; }
	}
}
