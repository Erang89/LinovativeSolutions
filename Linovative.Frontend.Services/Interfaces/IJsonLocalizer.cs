namespace Linovative.Frontend.Services.Interfaces
{
    public interface IJsonLocalizer
    {
        string this[string key] { get; }
        string Format(string key, params object[] args);
        Task EnsureLoadedAsync(string culture);    // load file JSON untuk culture tsb
        string CurrentCulture { get; }
    }
}
