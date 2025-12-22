using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemPriceTags
{
    public class DeleteCostumePriceTagCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteCostumePriceTagHandlerService : SaveDeleteServiceBase<CostumePriceTag, DeleteCostumePriceTagCommand>, IRequestHandler<DeleteCostumePriceTagCommand, Result>
    {
        public DeleteCostumePriceTagHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

    }
}
