using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Commons;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Sources
{
    public class GetAllCountryCommand : IRequest<Result>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllCountryHandlerService : PaginationQueryServiceBase<Country, GetAllCountryCommand>, IRequestHandler<GetAllCountryCommand, Result>
    {
        public GetAllCountryHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
            : base(dbContext, actor, mapper, appCache)
        {
            
        }

        protected override IQueryable<Country> OnPaginationQueryFilter(IQueryable<Country> query, GetAllCountryCommand request)
        {
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
                query = query.Where(x => string.Concat(x.Name, x.Region!.Name, x.Code).Contains(request.SearchKeyword??""));

            return query;
        }

        public Task<Result> Handle(GetAllCountryCommand request, CancellationToken ct)
            => base.GetPaginationResult<IdWithCodeDto>(request);
    }

}
