using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.Sources;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ICountryService : IReadOnlyService<CountryDto> { }

    internal class CountryService : ReadOnlyServiceAbstract<CountryDto>, ICountryService
    {
        public CountryService(IHttpClientFactory httpFactory, ILogger logger) : base(httpFactory, logger, "/currencies")
        {
            
        }

        protected override bool IsPublicEndpoint => true;
    }
}
