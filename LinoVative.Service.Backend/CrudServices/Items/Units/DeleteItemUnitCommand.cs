using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Backend.LocalizerServices;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.Units
{
    public class DeleteItemUnitCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteItemUnitHandlerService : SaveDeleteServiceBase<ItemUnit, DeleteItemUnitCommand>, IRequestHandler<DeleteItemUnitCommand, Result>
    {
        public DeleteItemUnitHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

    }
}
