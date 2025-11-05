using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Outlets
{
    public class BulkDeleteOutletCommand : IRequest<Result>, IBulkDeleteDto
    {
        public List<Guid> Ids { get; set; } = new();
    }

    public class BulkDeleteOutletHandlerService : SaveDeleteServiceBase<Outlet, BulkDeleteOutletCommand>, IRequestHandler<BulkDeleteOutletCommand, Result>
    {
        public BulkDeleteOutletHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }
    }
}
