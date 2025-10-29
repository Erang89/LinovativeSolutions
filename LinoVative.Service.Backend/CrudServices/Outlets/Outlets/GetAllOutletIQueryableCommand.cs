using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto.Outlets;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Outlets
{
    public class GetAllOutletIQueryableCommand : IRequest<IQueryable<OutletViewDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllOutletQueryableHandlerService : QueryServiceBase<Outlet, GetAllOutletIQueryableCommand>, IRequestHandler<GetAllOutletIQueryableCommand, IQueryable<OutletViewDto>>
    {
        public GetAllOutletQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<Outlet> OnGetAllFilter(IQueryable<Outlet> query, GetAllOutletIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => x.Name!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<OutletViewDto>> Handle(GetAllOutletIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<OutletViewDto>(_mapper.Config));

    }
}
