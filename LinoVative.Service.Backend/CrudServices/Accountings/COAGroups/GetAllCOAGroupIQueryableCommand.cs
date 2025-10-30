using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto.MasterData.Accountings;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.COAGroups
{
    public class GetAllCOAGroupIQueryableCommand : IRequest<IQueryable<COAGroupDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllCOAGroupQueryableHandlerService : QueryServiceBase<COAGroup, GetAllCOAGroupIQueryableCommand>, IRequestHandler<GetAllCOAGroupIQueryableCommand, IQueryable<COAGroupDto>>
    {
        public GetAllCOAGroupQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<COAGroup> OnGetAllFilter(IQueryable<COAGroup> query, GetAllCOAGroupIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => x.Name!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<COAGroupDto>> Handle(GetAllCOAGroupIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<COAGroupDto>(_mapper.Config));

    }
}
