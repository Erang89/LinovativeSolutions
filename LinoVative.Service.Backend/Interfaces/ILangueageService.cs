namespace LinoVative.Service.Backend.Interfaces
{
    public interface ILangueageService
    {
        string this[string key] { get; }
        string Format(string key, object value);
        public void EnsureLoad(string key);
        public void EnsureLoad(Func<AvailableLanguageKeys, string> key);
    }

    public class AvailableLanguageKeys
    {
        public string ChangeDefaultCompanyCommand => nameof(ChangeDefaultCompanyCommand);
        public string RefreshTokenCommand => nameof(RefreshTokenCommand);
    }
}
