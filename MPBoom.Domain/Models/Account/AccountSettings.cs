using MPBoom.Domain.Models.Base;

namespace MPBoom.Domain.Models.Account
{
    public class AccountSettings : Entity
    {
        public Account Account { get; set; }
        public Guid AccountId { get; set; }

        public string WildberriesApiKey { get; set; }
    }
}
