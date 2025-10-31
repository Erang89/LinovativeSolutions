using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto.Sources;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Sources
{
    public class GetAllTimezoneIQueryableCommand : IRequest<IQueryable<TimezoneDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllTimezoneQueryableHandlerService : QueryServiceBase<AppTimeZone, GetAllTimezoneIQueryableCommand>, IRequestHandler<GetAllTimezoneIQueryableCommand, IQueryable<TimezoneDto>>
    {
        public GetAllTimezoneQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<AppTimeZone> OnGetAllFilter(IQueryable<AppTimeZone> query, GetAllTimezoneIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => string.Concat(x.Name, x.GMT)!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<TimezoneDto>> Handle(GetAllTimezoneIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<TimezoneDto>(_mapper.Config));

    }
}
