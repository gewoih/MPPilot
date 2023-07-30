namespace MPBoom.CoreLibrary.Models
{
    public class AdvertCampaignSearchPosition
    {
        public int RealPlace { get; set; }
        public int AdPlace { get; set; }
        public int Page => AdPlace / 30 + 1;
    }
}
