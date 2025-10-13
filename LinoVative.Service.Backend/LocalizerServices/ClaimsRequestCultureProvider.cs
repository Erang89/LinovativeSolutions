using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace LinoVative.Service.Backend.LocalizerServices
{
    public sealed class ClaimsRequestCultureProvider : RequestCultureProvider
    {
        public string ClaimType { get; init; } = "lang"; // your JWT claim name

        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext context)
        {
            var lang = context.User?.FindFirst(ClaimType)?.Value;

            if (string.IsNullOrWhiteSpace(lang))
                return Task.FromResult<ProviderCultureResult?>(null); // fall through to next provider

            // normalize to supported culture names if needed
            var culture = new CultureInfo(lang).Name; // e.g. "id" or "id-ID"
            return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(culture, culture));
        }
    }
}
