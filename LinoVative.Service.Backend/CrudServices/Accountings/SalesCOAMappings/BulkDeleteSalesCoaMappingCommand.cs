using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.SalesCOAMappings
{
    public class BulkDeleteSalesCoaMappingCommand : IRequest<Result>, IBulkDeleteDto
    {
        public List<Guid> Ids { get; set; } = new();
    }

    public class BulkDeleteSalesCoaMappingHandlerService : SaveDeleteServiceBase<SalesCOAMapping, BulkDeleteSalesCoaMappingCommand>, IRequestHandler<BulkDeleteSalesCoaMappingCommand, Result>
    {
        public BulkDeleteSalesCoaMappingHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }
    }
}
