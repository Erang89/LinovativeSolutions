using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto.Outlets;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Areas
{
    public class GetAllOutletAreaGroupIQueryableCommand : IRequest<IQueryable<OutletAreaViewDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllOutletAreaQueryableHandlerService : QueryServiceBase<OutletArea, GetAllOutletAreaGroupIQueryableCommand>, IRequestHandler<GetAllOutletAreaGroupIQueryableCommand, IQueryable<OutletAreaViewDto>>
    {
        public GetAllOutletAreaQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<OutletArea> OnGetAllFilter(IQueryable<OutletArea> query, GetAllOutletAreaGroupIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => x.Name!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<OutletAreaViewDto>> Handle(GetAllOutletAreaGroupIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).AsNoTracking().ProjectToType<OutletAreaViewDto>(_mapper.Config));

    }
}
