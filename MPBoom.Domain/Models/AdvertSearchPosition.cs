namespace MPBoom.Domain.Models
{
	public class AdvertSearchPosition
	{
		public int RealPlace { get; set; }
		public int AdPlace { get; set; }
		public int Page => AdPlace / 30 + 1;
	}
}
