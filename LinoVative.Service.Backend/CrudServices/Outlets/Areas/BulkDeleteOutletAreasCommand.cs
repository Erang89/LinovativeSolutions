using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Areas
{
    public class BulkDeleteOutletAreasCommand : IRequest<Result>, IBulkDeleteDto
    {
        public List<Guid> Ids { get; set; } = new();
    }

    public class BulkDeleteOutletAreasHandlerService : SaveDeleteServiceBase<OutletArea, BulkDeleteOutletAreasCommand>, IRequestHandler<BulkDeleteOutletAreasCommand, Result>
    {
        public BulkDeleteOutletAreasHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        public override Task<Result> Handle(BulkDeleteOutletAreasCommand request, CancellationToken token = default)
        {
            var tables = _dbContext.OutletTables.Where(x => !x.IsDeleted && request.Ids.Contains(x.AreaId!.Value)).ToList();

            foreach (var table in tables)
            {
                table.Delete(_actor);
            }

            return base.Handle(request, token);
        }
    }
}
