using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.PriceTypes
{
    public class BulkDeletePriceTypeCommand : IRequest<Result>, IBulkDeleteDto
    {
        public List<Guid> Ids { get; set; } = new();
    }

    public class BulkDeletePriceTypeHandlerService : SaveDeleteServiceBase<PriceType, BulkDeletePriceTypeCommand>, IRequestHandler<BulkDeletePriceTypeCommand, Result>
    {
        public BulkDeletePriceTypeHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }
    }
}
