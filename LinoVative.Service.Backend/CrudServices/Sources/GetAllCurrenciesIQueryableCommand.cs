using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto.Sources;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Sources
{
    public class GetAllCurrencyIQueryableCommand : IRequest<IQueryable<CurrencyDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllCurrencyQueryableHandlerService : QueryServiceBase<Currency, GetAllCurrencyIQueryableCommand>, IRequestHandler<GetAllCurrencyIQueryableCommand, IQueryable<CurrencyDto>>
    {
        public GetAllCurrencyQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<Currency> OnGetAllFilter(IQueryable<Currency> query, GetAllCurrencyIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => string.Concat(x.Name, x.Code, x.Symbol)!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<CurrencyDto>> Handle(GetAllCurrencyIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<CurrencyDto>(_mapper.Config));

    }
}
