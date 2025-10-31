using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.Sources;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ITimezoneService : IReadOnlyService<TimezoneDto> {
        
    }

    public class TimezoneService : ReadOnlyServiceAbstract<TimezoneDto>, ITimezoneService
    {
        public TimezoneService(IHttpClientFactory httpFactory, ILogger<TimezoneService> logger) : base(httpFactory, logger, "timezones")
        {
            
        }

        protected override bool IsPublicEndpoint => true;
    }
}
