using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Commons;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Sources
{
    public class GetAllTimezoneCommand : IRequest<Result>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllTimezoneHandlerService : PaginationQueryServiceBase<AppTimeZone, GetAllTimezoneCommand>, IRequestHandler<GetAllTimezoneCommand, Result>
    {
        public GetAllTimezoneHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
            : base(dbContext, actor, mapper, appCache)
        {
            
        }

        protected override IQueryable<AppTimeZone> OnPaginationQueryFilter(IQueryable<AppTimeZone> query, GetAllTimezoneCommand request)
        {
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
                query = query.Where(x => string.Concat(x.Name, x.GMT, x.Zona, x.TimeZone).Contains(request.SearchKeyword??""));

            return query;
        }

        public Task<Result> Handle(GetAllTimezoneCommand request, CancellationToken ct)
            => base.GetPaginationResult<IdWithCodeDto>(request);
    }

}
