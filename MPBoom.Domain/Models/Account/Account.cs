using MPBoom.Domain.Models.Base;

namespace MPBoom.Domain.Models.Account
{
    public class Account : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
		public string Password { get; set; }

        public AccountSettings? AccountSettings { get; set; }
        public Guid? AccountSettingsId { get; set; }
    }
}
