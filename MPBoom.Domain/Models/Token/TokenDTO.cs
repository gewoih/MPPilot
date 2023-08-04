namespace MPBoom.Domain.Models.Token
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
    }
}
