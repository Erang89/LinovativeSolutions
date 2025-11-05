using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Shifts;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Shifts
{
    public class BulkDeleteShiftCommand : IRequest<Result>, IBulkDeleteDto
    {
        public List<Guid> Ids { get; set; } = new();
    }

    public class BulkDeleteShiftHandlerService : SaveDeleteServiceBase<Shift, BulkDeleteShiftCommand>, IRequestHandler<BulkDeleteShiftCommand, Result>
    {
        public BulkDeleteShiftHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }
    }
}
