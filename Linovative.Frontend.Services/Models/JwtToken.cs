namespace Linovative.Frontend.Services.Models
{
    public class JwtToken
    {
        public string Token { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public Guid? CompanyId { get; set; }
        //public Guid? OutletId { get; set; }
        public DateTime ExpireAtUtcTime { get; set; }
        public string? Nikname { get; set; }
        //public List<string> Roles { get; set; } = new();
        public string? CompanyName { get; set; }
        //public string? OutletName { get; set; }
        //public string? UIDHash { get; set; }

    }
}
