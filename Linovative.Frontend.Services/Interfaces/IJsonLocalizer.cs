namespace Linovative.Frontend.Services.Interfaces
{
    public interface IJsonLocalizer
    {
        string this[string key] { get; }
        string Format(string key, params object[] args);
        Task EnsureLoadedAsync(string key, string? libraryName = default);

    }
}
