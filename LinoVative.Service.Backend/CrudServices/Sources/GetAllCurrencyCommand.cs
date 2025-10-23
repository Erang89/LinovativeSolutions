using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Commons;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Sources
{
    public class GetAllCurrencyCommand : IRequest<Result>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllCurrencyHandlerService : PaginationQueryServiceBase<Currency, GetAllCurrencyCommand>, IRequestHandler<GetAllCurrencyCommand, Result>
    {
        public GetAllCurrencyHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
            : base(dbContext, actor, mapper, appCache)
        {
            
        }

        protected override IQueryable<Currency> OnPaginationQueryFilter(IQueryable<Currency> query, GetAllCurrencyCommand request)
        {
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
                query = query.Where(x => string.Concat(x.Name, x.Symbol, x.Code).Contains(request.SearchKeyword??""));

            return query;
        }

        public Task<Result> Handle(GetAllCurrencyCommand request, CancellationToken ct)
            => base.GetPaginationResult<IdWithCodeDto>(request);
    }

}
