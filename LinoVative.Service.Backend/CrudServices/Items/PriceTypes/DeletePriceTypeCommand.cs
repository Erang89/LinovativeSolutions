using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.PriceTypes
{
    public class DeletePriceTypeCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeletePriceTypeHandlerService : SaveDeleteServiceBase<PriceType, DeletePriceTypeCommand>, IRequestHandler<DeletePriceTypeCommand, Result>
    {
        public DeletePriceTypeHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

    }
}
