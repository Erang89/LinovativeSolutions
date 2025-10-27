using Linovative.Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;

namespace Linovative.Frontent.WebBlazor.Extensions
{
    public static class WebAssemblyHostBuilderExtensions
    {
        public static async Task RegisterLocalizer(this WebAssemblyHostBuilder builder)
        {
            var host = builder.Build();
            var lang = host.Services.GetRequiredService<ILanguageProvider>();
            var loc = host.Services.GetRequiredService<IJsonLocalizer>();
            var culture = await lang.GetLanguage() ?? lang.DefaultCulture;
            //await loc.EnsureLoadedAsync(culture);
            var ci = new CultureInfo(culture switch
            {
                "id" => "id-ID",
                "en" => "en-US",
                _ => culture
            });
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;

        }
    }
}
