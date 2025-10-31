using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.Sources;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ICountryService : IReadOnlyService<CountryDto> {
        
    }

    public class CountryService : ReadOnlyServiceAbstract<CountryDto>, ICountryService
    {
        public CountryService(IHttpClientFactory httpFactory, ILogger<CountryService> logger) : base(httpFactory, logger, "countries")
        {
            
        }

        protected override bool IsPublicEndpoint => true;
    }
}
