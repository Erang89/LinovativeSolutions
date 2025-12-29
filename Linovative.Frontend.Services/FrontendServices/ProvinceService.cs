using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.Sources;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IProvinceService : IReadOnlyService<ProvinceDto> {
        
    }

    public class ProvinceService : ReadOnlyServiceAbstract<ProvinceDto>, IProvinceService
    {
        public ProvinceService(IHttpClientFactory httpFactory, ILogger<ProvinceService> logger) : base(httpFactory, logger, "provinces")
        {
            
        }

        protected override bool IsPublicEndpoint => true;
    }
}
