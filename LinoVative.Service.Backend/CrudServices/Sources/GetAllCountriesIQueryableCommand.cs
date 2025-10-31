using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto.Sources;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Sources
{
    public class GetAllCountryIQueryableCommand : IRequest<IQueryable<CountryDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllCountryQueryableHandlerService : QueryServiceBase<Country, GetAllCountryIQueryableCommand>, IRequestHandler<GetAllCountryIQueryableCommand, IQueryable<CountryDto>>
    {
        public GetAllCountryQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<Country> OnGetAllFilter(IQueryable<Country> query, GetAllCountryIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => string.Concat(x.Name, x.Code)!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<CountryDto>> Handle(GetAllCountryIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<CountryDto>(_mapper.Config));

    }
}
