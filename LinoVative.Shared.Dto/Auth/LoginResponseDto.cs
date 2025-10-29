namespace LinoVative.Shared.Dto.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; }  =string.Empty;
        public DateTime ExpiryUTCTime { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
