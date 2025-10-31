using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.Sources;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ICountryService : IReadOnlyService<CountryDto> { }
    internal class CountryService : ReadOnlyService<CountryDto>, ICountryService
    {
        public CountryService(IHttpClientFactory httpFactory, ILogger logger) : base(httpFactory, logger, "/source/currencies")
        {
            
        }
    }
}
