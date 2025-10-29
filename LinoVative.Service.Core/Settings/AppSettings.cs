namespace LinoVative.Service.Core.Settings
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public int TokenExpiryMinutes { get; set; }
    }

    public class AppSettings
    {
        public JwtSettings JwtSettings { get; set; } = new();
    }
}
