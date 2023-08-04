using MPBoom.Domain.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MPBoom.Domain.Models.Account
{
    public class Account : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
		public string Password { get; set; }
    }
}
