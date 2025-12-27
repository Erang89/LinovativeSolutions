namespace LinoVative.Service.Backend.Interfaces
{
    public interface ILangueageService
    {
        string this[string key] { get; }
        string Format(string key, object value);
        public void EnsureLoad(string key);
        public void EnsureLoad(AvailableLanguageKeys? key);
    }

    public enum AvailableLanguageKeys
    {
        BulkUploadCommand,
        ChangeDefaultCompanyCommand,
        RefreshTokenCommand,
        CreateOrUpdateOutletAreaCommand,
        CreateOutletCommand,
        InputItems
    }
}
