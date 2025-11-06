using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Backend.LocalizerServices;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemGroups
{
    public class DeleteItemGroupCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteItemGroupHandlerService : SaveDeleteServiceBase<ItemGroup, DeleteItemGroupCommand>, IRequestHandler<DeleteItemGroupCommand, Result>
    {
        public DeleteItemGroupHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

    }
}
