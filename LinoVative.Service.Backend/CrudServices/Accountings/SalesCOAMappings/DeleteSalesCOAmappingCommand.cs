using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.SalesCOAMappings
{
    public class DeleteSalesCOAmappingCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteSalesCOAMappingHandlerService : SaveDeleteServiceBase<SalesCOAMapping, DeleteSalesCOAmappingCommand>, IRequestHandler<DeleteSalesCOAmappingCommand, Result>
    {
        public DeleteSalesCOAMappingHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

    }
}
