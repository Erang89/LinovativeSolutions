namespace LinoVative.Shared.Dto.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; }  =string.Empty;
        public DateTime ExpireAtUtcTime { get; set; }
        public Guid RefreshToken { get; set; }
        public Guid? CompanyId { get; set; }
        public string? Nikname { get; set; }
        public string? CompanyName { get; set; }
    }
}
