using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Shifts;
using LinoVative.Shared.Dto.MasterData.Outlets;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Shifts
{
    public class GetAllShiftIQueryableCommand : IRequest<IQueryable<ShiftViewDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllOutletQueryableHandlerService : QueryServiceBase<Shift, GetAllShiftIQueryableCommand>, IRequestHandler<GetAllShiftIQueryableCommand, IQueryable<ShiftViewDto>>
    {
        public GetAllOutletQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<Shift> OnGetAllFilter(IQueryable<Shift> query, GetAllShiftIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => x.Name!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<ShiftViewDto>> Handle(GetAllShiftIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<ShiftViewDto>(_mapper.Config));

    }
}
