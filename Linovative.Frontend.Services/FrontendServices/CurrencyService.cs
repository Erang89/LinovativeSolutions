using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.Sources;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ICurrencyService : IReadOnlyService<CurrencyDto> {
        
    }

    public class CurrencyService : ReadOnlyServiceAbstract<CurrencyDto>, ICurrencyService
    {
        public CurrencyService(IHttpClientFactory httpFactory, ILogger<CurrencyService> logger) : base(httpFactory, logger, "currencies")
        {
            
        }

        protected override bool IsPublicEndpoint => true;
    }
}
