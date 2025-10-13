using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace LinoVative.Service.Backend.LocalizerServices
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly string _basePath;

        public JsonStringLocalizerFactory(IOptions<JsonLocalizationOptions> options)
        {
            _basePath = options.Value.BasePath ?? Path.Combine(AppContext.BaseDirectory, "Resources");
        }

        public IStringLocalizer Create(Type resourceSource)
            => new JsonStringLocalizer(_basePath, "validation"); // one file group for validation

        public IStringLocalizer Create(string baseName, string location)
            => new JsonStringLocalizer(_basePath, "validation");
    }

    public class JsonLocalizationOptions
    {
        public string? BasePath { get; set; }
    }

    public static class JsonLocalizationExtensions
    {
        public static IServiceCollection AddJsonLocalization(this IServiceCollection services, Action<JsonLocalizationOptions>? configure = null)
        {
            services.Configure(configure ?? (_ => { }));
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer>(sp =>
                sp.GetRequiredService<IStringLocalizerFactory>().Create(typeof(object)));
            return services;
        }
    }
}
