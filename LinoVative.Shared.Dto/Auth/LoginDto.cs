using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.Auth
{
    [LocalizerKey(nameof(LoginDto))]
    public class LoginDto 
    {
        [LocalizedRequired]
        public string? UserName { get; set; }

        [LocalizedRequired]
        public string? Password { get; set; }
    }
}
