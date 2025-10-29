namespace LinoVative.Service.Backend.Interfaces
{
    public interface ILangueageService
    {
        string this[string key] { get; }
        string Format(string key, object value);
        public Task EnsureLoad(string key);
    }
}
